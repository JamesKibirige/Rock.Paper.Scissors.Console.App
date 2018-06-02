using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class ApplicationTests
    {
        /// <summary>
        /// The instance of the Application that we will be using to test
        /// </summary>
        public IRunnable Application { get; set; }

        /// <summary>
        /// A descriptive string detailing the Name of the application
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A descriptive string detailing the Author of the application
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The collection of Players that will be used when playing Matches and games
        /// </summary>
        public List<IPlayer> Players { get; set; }

        /// <summary>
        /// The rules that Matches and Games will be governed by
        /// </summary>
        public IGameRules Rules { get; set; }

        /// <summary>
        /// A Game Controller used to enforce a seperation of concerns for aspects of the Game pipeline
        /// </summary>
        public IUserInteraction InteractionController { get; set; }

        /// <summary>
        /// Mock of the User Interaction Controller that we will be using to set up our User Interaction controller instance
        /// </summary>
        public Mock<IUserInteraction> UiControllerMock { get; set; }

        /// <summary>
        /// The Match that will be played
        /// </summary>
        public Contest Match { get; set; }

        /// <summary>
        /// Mock instance of our Match that will be used to test methods
        /// </summary>
        public Mock<Contest> MatchMock { get; set; }

        /// <summary>
        /// The instance that will be used to track Scores in the game
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// Our Mock instance of a Match result that we will use in our tests
        /// </summary>
        public Mock<IContestResult> MatchResultMock { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            Title = "Rock,Paper,Scissors!!";
            Author = "James Kibirige";
            Players = new List<IPlayer>();

            UiControllerMock = new Mock<IUserInteraction>();
            InteractionController = UiControllerMock.Object;

            var rulesDictionary = new Dictionary<GameAction, List<GameAction>>()
            {
                {
                    GameAction.Rock,
                    new List<GameAction>() {GameAction.Scissors}
                },
                {
                    GameAction.Paper,
                    new List<GameAction>() {GameAction.Rock}
                },
                {
                    GameAction.Scissors,
                    new List<GameAction>() {GameAction.Paper}
                }
            };

            Rules = new GameRules(rulesDictionary);

            Dictionary<IPlayer, int> scoresDictionary = new Dictionary<IPlayer, int>();
            Score = new Score(scoresDictionary);

            Application = new Application(Title,Author,Players,Rules,InteractionController,Match, Score,null);
        }

        [TestMethod]
        public void DisplayTitleSplash_DisplayTitleSplashToTheUser_ExpectedMethodCalls()
        {
            //Arrange
            var titleSplash = $"{Title}\n\nby {Author}";
            UiControllerMock.Setup(m => m.Output(titleSplash));

            //Act
            var theApp = (Application) Application;
            theApp.DisplayTitleSplash();

            //Assert
            UiControllerMock.Verify(m=>m.Output(It.IsAny<string>()),Times.Once);
        }

        [TestMethod]
        public void InitialisePlayer_InitialisesPlayerInstance_ReturnsExpectedPlayerInstance()
        {
            //Arrange
            UiControllerMock.Setup(m => m.RequestInput("Please Enter your name:\n\n{A-Z,a-z, }")).Returns("James Kibirige");

            //Act
            var theApp = (Application)Application;
            var result = theApp.InitialisePlayer();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IPlayer));
            Assert.AreEqual("James Kibirige",result.Name);
            Assert.IsNotNull(result.Rules);
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.Once);
        }

        [DataTestMethod]
        [DataRow("Human")]
        [DataRow("Random")]
        [DataRow("Tactical")]
        public void InitialiseOpponent_InitialisesOpponentInstance_ReturnsExpectedPlayerInstance(string opponentType)
        {
            //Arrange
            var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            var requestMessage = $"Select an Opponent Type from the choices:\n\n{{{opponentTypeOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(requestMessage)).Returns(opponentType);

            Type expectedType;
            switch (opponentType)
            {
                case "Human":
                    expectedType = typeof(HumanPlayer);
                    break;
                case "Random":
                    expectedType = typeof(RandomPlayer);
                    break;
                case "Tactical":
                    expectedType = typeof(TacticalPlayer);
                    break;
                default:
                    expectedType = null;
                    break;
            }

            //Act
            var theApp = (Application)Application;
            var result = theApp.InitialiseOpponent();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, expectedType);
            Assert.AreEqual(opponentType, result.Name);
            Assert.IsNotNull(result.Rules);
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.Once);
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("5")]
        [DataRow("9")]
        public void InitialiseMatch_InitialisesMatchInstanceWithValidNumRounds_ReturnsExpectedMatchInstance(string input)
        {
            //Arrange
            var initialisingMessage = $"Initialising Match...";
            var requestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";

            UiControllerMock.Setup(m => m.Output(initialisingMessage));
            UiControllerMock.Setup(m => m.RequestInput(requestMessage)).Returns(input);
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);

            Players.Add(player1Mock.Object);
            Players.Add(player2Mock.Object);

            Int32.TryParse(input, out var expectedNumRounds);
            var expectedScoreItemCount = 2;

            //Act
            var theApp = (Application)Application;
            var result = (Match)theApp.InitialiseMatch(1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Match));
            Assert.AreEqual(expectedNumRounds, result.NumRounds);
            Assert.AreEqual(expectedScoreItemCount, result.Score.Scores.Count);
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Once);
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.Once);
        }

        [DataTestMethod]
        [DataRow("10")]
        [DataRow("15")]
        [DataRow("20")]
        public void InitialiseMatch_InitialisesMatchInstanceWithInValidNumRounds_LoopsUntilValidInputProvided(string input)
        {
            //Arrange
            var initialisingMessage = $"Initialising Match...";
            var requestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";

            Regex rgx = new Regex(@"^[0-9]+$");

            const int expectedNumRounds = 5;
            UiControllerMock.Setup(m => m.Output(initialisingMessage));
            UiControllerMock.Setup(m => m.RequestInput(requestMessage)).Returns(input);
            UiControllerMock.Setup(m => m.RequestValidInput(It.IsAny<string>(),It.IsAny<Regex>())).Returns(expectedNumRounds.ToString());

            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);

            Players.Add(player1Mock.Object);
            Players.Add(player2Mock.Object);

            var expectedScoreItemCount = 2;

            //Act
            var theApp = (Application)Application;
            var result = (Match)theApp.InitialiseMatch(1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Match));
            Assert.AreEqual(expectedNumRounds, result.NumRounds);
            Assert.AreEqual(expectedScoreItemCount, result.Score.Scores.Count);
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Once);
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.Once);
            UiControllerMock.Verify(m => m.RequestValidInput(It.IsAny<string>(), It.IsAny<Regex>()), Times.Once);
        }

        [TestMethod]
        public void PlayMatch_PlayMatchInstance_ReturnsExpectedMatchResult()
        {
            //Arrange
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);

            Players.Add(player1Mock.Object);
            Players.Add(player2Mock.Object);

            Score.Scores.Add(Players[0],2);
            Score.Scores.Add(Players[1], 1);

            MatchMock = new Mock<Contest>();
            IContestResult matchResult = new MatchResult(1, Players[0],Score,Players,ContestOutcome.Win);
            MatchMock.Setup(m => m.Play(Rules)).Returns(matchResult);
            UiControllerMock.Setup(m => m.Output(matchResult.ToString()));

            //Act
            var theApp = (Application)Application;
            theApp.Match = MatchMock.Object;
            var result = (MatchResult)theApp.PlayMatch();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.ContestId);
            Assert.AreEqual(Players[0].Name, result.Winner.Name);
            Assert.AreEqual(2, result.Score.Scores[Players[0]]);
            Assert.AreEqual(1, result.Score.Scores[Players[1]]);
            Assert.AreEqual(2, result.Participants.Count);
            Assert.AreEqual(Players[0].Name, result.Participants[0].Name);
            Assert.AreEqual(Players[1].Name, result.Participants[1].Name);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
        }

        [TestMethod]
        public void DisplayMatchResults_DisplaysMatchResultMessageToUser_ExecutesExpectedMethods()
        {
            //Arrange
            var displayMatchResultString = "Match<1>: Result\n\nMatch Outcome<Win>\n\nPlayer<James Kibirige>: Score <2>\nPlayer<Jonita Laidley>: Score <0>\n\nPlayer<James Kibirige> Is the Winner!!!\n";
            MatchResultMock = new Mock<IContestResult>();
            MatchResultMock.Setup(m => m.ToString()).Returns(displayMatchResultString);

            UiControllerMock.Setup(m => m.Output(displayMatchResultString));
            var theApp = (Application)Application;
            theApp.MatchResult = MatchResultMock.Object;

            //Act
            theApp.DisplayMatchResult();

            //Assert
            UiControllerMock.Verify(m=>m.Output(It.IsAny<string>()),Times.Once);
        }

        [TestMethod]
        public void Run_ExecutesGamePipeLineWithRandomOpponent_DoesAllExpectedWork()
        {
            //Arrange
            const string playerName = "James Kibirige";
            const string nameRequestMessage = "Please Enter your name:\n\n{A-Z,a-z, }";
            UiControllerMock.Setup(m => m.RequestInput(nameRequestMessage)).Returns("James Kibirige");

            var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            var opponentTypeRequestMessage = $"Select an Opponent Type from the choices:\n\n{{{opponentTypeOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(opponentTypeRequestMessage)).Returns("Random");

            var numRoundsrequestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";
            UiControllerMock.Setup(m => m.RequestInput(numRoundsrequestMessage)).Returns("3");

            var possibleActions = string.Join(",", Rules.PossibleGameActions());//String literal listing possible game actions
            var playerSelectMoveMessage = string.Format("Player <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", playerName, possibleActions);
            UiControllerMock.SetupSequence(m => m.RequestInput(playerSelectMoveMessage)).Returns("Rock").Returns("Paper").Returns("Scissors");

            var yesNoOptions = "Yes,Y,No,N";
            var playAgainRequestMessage = $"Do want to Play Again?\n\n{{{yesNoOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(playAgainRequestMessage)).Returns("no");

            var theApp = (Application)Application;

            //Act
            theApp.Run();

            //Assert
            UiControllerMock.Verify(m=>m.Output(It.IsAny<string>()),Times.AtLeast(3));
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.AtLeast(3));
        }

        [TestMethod]
        public void Run_ExecutesGamePipeLineWithRandomOpponent5Games_DoesAllExpectedWork()
        {
            //Arrange
            const string playerName = "James Kibirige";
            const string nameRequestMessage = "Please Enter your name:\n\n{A-Z,a-z, }";
            UiControllerMock.Setup(m => m.RequestInput(nameRequestMessage)).Returns("James Kibirige");

            var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            var opponentTypeRequestMessage = $"Select an Opponent Type from the choices:\n\n{{{opponentTypeOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(opponentTypeRequestMessage)).Returns("Random");

            var numRoundsrequestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";
            UiControllerMock.Setup(m => m.RequestInput(numRoundsrequestMessage)).Returns("5");

            var possibleActions = string.Join(",", Rules.PossibleGameActions());//String literal listing possible game actions
            var playerSelectMoveMessage = string.Format("Player <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", playerName, possibleActions);
            UiControllerMock.SetupSequence(m => m.RequestInput(playerSelectMoveMessage)).Returns("Rock").Returns("Paper").Returns("Scissors").Returns("Rock").Returns("Paper");

            var yesNoOptions = "Yes,Y,No,N";
            var playAgainRequestMessage = $"Do want to Play Again?\n\n{{{yesNoOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(playAgainRequestMessage)).Returns("no");

            var theApp = (Application)Application;

            //Act
            theApp.Run();

            //Assert
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.AtLeast(5));
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.AtLeast(5));
        }

        [TestMethod]
        public void Run_ExecutesGamePipeLineWithTacticalOpponent_DoesAllExpectedWork()
        {
            //Arrange
            const string playerName = "James Kibirige";
            const string nameRequestMessage = "Please Enter your name:\n\n{A-Z,a-z, }";
            UiControllerMock.Setup(m => m.RequestInput(nameRequestMessage)).Returns("James Kibirige");

            var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            var opponentTypeRequestMessage = $"Select an Opponent Type from the choices:\n\n{{{opponentTypeOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(opponentTypeRequestMessage)).Returns("Tactical");

            var numRoundsrequestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";
            UiControllerMock.Setup(m => m.RequestInput(numRoundsrequestMessage)).Returns("3");

            var possibleActions = string.Join(",", Rules.PossibleGameActions());//String literal listing possible game actions
            var playerSelectMoveMessage = string.Format("Player <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", playerName, possibleActions);
            UiControllerMock.SetupSequence(m => m.RequestInput(playerSelectMoveMessage)).Returns("Rock").Returns("Paper").Returns("Scissors");

            var yesNoOptions = "Yes,Y,No,N";
            var playAgainRequestMessage = $"Do want to Play Again?\n\n{{{yesNoOptions}}}";
            UiControllerMock.Setup(m => m.RequestInput(playAgainRequestMessage)).Returns("no");

            var theApp = (Application)Application;

            //Act
            theApp.Run();

            //Assert
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.AtLeast(3));
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()), Times.AtLeast(3));
        }
    }
}