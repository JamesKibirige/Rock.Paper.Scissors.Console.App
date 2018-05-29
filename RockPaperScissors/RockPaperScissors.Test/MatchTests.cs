using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class MatchTests
    {
        /// <summary>
        /// Identification number for each Match
        /// </summary>
        public int MatchId { get; set; }

        /// <summary>
        /// A Match has a collection of Games that denote Rounds that are played by participating Players
        /// </summary>
        public List<Contest> Rounds { get; set; }

        /// <summary>
        /// A Match is limited to a finite number of rounds that can be played.
        /// </summary>
        public int NumRounds { get; set; }

        /// <summary>
        /// A Match has a score that is decifered based on the outcomes of the individual games of the match
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// A match stores and builds collection of Contest Results that reflect the results of the games played during the match
        /// </summary>
        public List<IContestResult> GameResults { get; set; }

        /// <summary>
        /// A match needs a User interaction controller to send and receive messages and input from the User
        /// </summary>
        public IUserInteraction Controller { get; set; }

        /// <summary>
        /// The Participants of a Contest
        /// </summary>
        public List<IPlayer> Participants { get; set; }

        /// <summary>
        /// The rules that the matches will be governed by
        /// </summary>
        public GameRules GameRules { get; private set; }

        /// <summary>
        /// Mock of a User Interaction controller
        /// </summary>
        public Mock<IUserInteraction> ControllerMock{ get; set; }

        /// <summary>
        /// Our player Mocks for the participants of the match
        /// </summary>
        public List<Mock<IPlayer>> PlayerMocks { get; set; }

        /// <summary>
        /// The Contest instance that will hold an instance of our Match that we will be testing
        /// </summary>
        public Contest Match { get; set; }

        [TestInitialize]
        public void SetUp()
        {
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

            GameRules = new GameRules(rulesDictionary);
            PlayerMocks = new List<Mock<IPlayer>>();
            Participants = new List<IPlayer>();
            Rounds = new List<Contest>();
            ControllerMock = new Mock<IUserInteraction>();
            NumRounds = 3;
            GameResults = new List<IContestResult>();
        }

        [TestMethod]
        public void Play_WinBestOf3_GeneratesExpectedMatchResult()
        {
            //Arrange
            MatchId = 1;
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            var player1 = player1Mock.Object;
            var player1Moves = new List<Move>()
            {
                new Move(GameAction.Rock, player1),
                new Move(GameAction.Paper, player1),
                new Move(GameAction.Scissors, player1)
            };

            player1Mock.SetupSequence(m => m.SelectMove())
                .Returns(player1Moves[0])
                .Returns(player1Moves[1])
                .Returns(player1Moves[2]);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            var player2 = player2Mock.Object;
            var player2Moves = new List<Move>()
            {
                new Move(GameAction.Scissors, player2),
                new Move(GameAction.Scissors, player2),
                new Move(GameAction.Rock, player2)
            };
            player2Mock.SetupSequence(m => m.SelectMove())
                .Returns(player2Moves[0])
                .Returns(player2Moves[1])
                .Returns(player2Moves[2]);

            PlayerMocks.Add(player1Mock);
            PlayerMocks.Add(player2Mock);

            Participants.Add(player1);
            Participants.Add(player2);

            Score = new Score
            (
                new Dictionary<IPlayer, int>()
                {
                    {player1,0},
                    {player2,0}
                }
            );

            var matchcommencementMessage = $"Match<{MatchId}>: Commence...";

            ControllerMock = new Mock<IUserInteraction>();
            ControllerMock.SetupSequence(m => m.Output(matchcommencementMessage))
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass();

            Controller = ControllerMock.Object;

            const int expectedplayer1Score = 1;
            const int expectedplayer2Score = 2;
            const int expectedNumRoundsPlayed = 3;

            //Set Up Match Instance
            Match = new Match(MatchId,Participants,NumRounds,Score, Controller, GameResults, Rounds);

            //Act
            var result = (MatchResult)Match.Play(GameRules);
            var theMatch = (Match) Match;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
            ControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(13));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.Name, Times.Exactly(7));
            PlayerMocks[1].Verify(m => m.Name, Times.Exactly(8));
            Assert.IsNotNull(result.Winner);
            Assert.AreEqual(expectedplayer1Score, result.Score.Scores[Participants[0]]);
            Assert.AreEqual(expectedplayer2Score, result.Score.Scores[Participants[1]]);
            Assert.AreEqual(Participants[1], result.Winner);
            Assert.AreEqual(expectedNumRoundsPlayed, theMatch.Rounds.Count);
        }

        [TestMethod]
        public void Play_WinBestOf3OverallScoreGreaterThanGamesLeft_GeneratesExpectedMatchResult()
        {
            //Arrange
            MatchId = 2;
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            var player1 = player1Mock.Object;
            var player1Moves = new List<Move>()
            {
                new Move(GameAction.Scissors, player1),
                new Move(GameAction.Rock, player1)
            };

            player1Mock.SetupSequence(m => m.SelectMove())
                .Returns(player1Moves[0])
                .Returns(player1Moves[1]);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            var player2 = player2Mock.Object;
            var player2Moves = new List<Move>()
            {
                new Move(GameAction.Paper, player2),
                new Move(GameAction.Scissors, player2),
            };
            player2Mock.SetupSequence(m => m.SelectMove())
                .Returns(player2Moves[0])
                .Returns(player2Moves[1]);

            PlayerMocks.Add(player1Mock);
            PlayerMocks.Add(player2Mock);

            Participants.Add(player1);
            Participants.Add(player2);

            Score = new Score
            (
                new Dictionary<IPlayer, int>()
                {
                    {player1,0},
                    {player2,0}
                }
            );

            var matchcommencementMessage = $"Match<{MatchId}>: Commence...";

            ControllerMock = new Mock<IUserInteraction>();
            ControllerMock.SetupSequence(m => m.Output(matchcommencementMessage))
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass();

            Controller = ControllerMock.Object;

            const int expectedplayer1Score = 2;
            const int expectedplayer2Score = 0;

            //Set Up Match Instance
            Match = new Match(MatchId, Participants, NumRounds, Score, Controller, GameResults, Rounds);

            const int expectedNumRoundsPlayed = 2;

            //Act
            var result = (MatchResult)Match.Play(GameRules);
            var theMatch = (Match) Match;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
            ControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(9));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Exactly(2));
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Exactly(2));
            PlayerMocks[0].Verify(m => m.Name, Times.Exactly(6));
            PlayerMocks[1].Verify(m => m.Name, Times.Exactly(4));
            Assert.IsNotNull(result.Winner);
            Assert.AreEqual(expectedplayer1Score, result.Score.Scores[Participants[0]]);
            Assert.AreEqual(expectedplayer2Score, result.Score.Scores[Participants[1]]);
            Assert.AreEqual(Participants[0], result.Winner);
            Assert.AreEqual(expectedNumRoundsPlayed, theMatch.Rounds.Count);
        }

        [TestMethod]
        public void Play_WinBestOf32GamesDraw_ReturnsExpectedResult()
        {
            //Arrange
            MatchId = 3;
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            var player1 = player1Mock.Object;
            var player1Moves = new List<Move>()
            {
                new Move(GameAction.Paper, player1),
                new Move(GameAction.Rock, player1),
                new Move(GameAction.Scissors, player1)
            };

            player1Mock.SetupSequence(m => m.SelectMove())
                .Returns(player1Moves[0])
                .Returns(player1Moves[1])
                .Returns(player1Moves[2]);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            var player2 = player2Mock.Object;
            var player2Moves = new List<Move>()
            {
                new Move(GameAction.Paper, player2),
                new Move(GameAction.Paper, player2),
                new Move(GameAction.Scissors, player2)
            };
            player2Mock.SetupSequence(m => m.SelectMove())
                .Returns(player2Moves[0])
                .Returns(player2Moves[1])
                .Returns(player2Moves[2]);

            PlayerMocks.Add(player1Mock);
            PlayerMocks.Add(player2Mock);

            Participants.Add(player1);
            Participants.Add(player2);

            Score = new Score
            (
                new Dictionary<IPlayer, int>()
                {
                    {player1,0},
                    {player2,0}
                }
            );

            var matchcommencementMessage = $"Match<{MatchId}>: Commence...";

            ControllerMock = new Mock<IUserInteraction>();
            ControllerMock.SetupSequence(m => m.Output(matchcommencementMessage))
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass();

            Controller = ControllerMock.Object;

            const int expectedplayer1Score = 0;
            const int expectedplayer2Score = 1;

            //Set Up Match Instance
            Match = new Match(MatchId, Participants, NumRounds, Score, Controller, GameResults, Rounds);

            const int expectedNumRoundsPlayed = 3;

            //Act
            var result = (MatchResult)Match.Play(GameRules);
            var theMatch = (Match)Match;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
            ControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(13));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.Name, Times.Exactly(6));
            PlayerMocks[1].Verify(m => m.Name, Times.Exactly(7));
            Assert.IsNotNull(result.Winner);
            Assert.AreEqual(expectedplayer1Score, result.Score.Scores[Participants[0]]);
            Assert.AreEqual(expectedplayer2Score, result.Score.Scores[Participants[1]]);
            Assert.AreEqual(Participants[1], result.Winner);
            Assert.AreEqual(expectedNumRoundsPlayed, theMatch.Rounds.Count);
        }

        [TestMethod]
        public void Play_DrawBestOf3AllGamesDraw_ReturnsExpectedResult()
        {
            //Arrange
            MatchId = 4;
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            var player1 = player1Mock.Object;
            var player1Moves = new List<Move>()
            {
                new Move(GameAction.Rock, player1),
                new Move(GameAction.Paper, player1),
                new Move(GameAction.Scissors, player1)
            };

            player1Mock.SetupSequence(m => m.SelectMove())
                .Returns(player1Moves[0])
                .Returns(player1Moves[1])
                .Returns(player1Moves[2]);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            var player2 = player2Mock.Object;
            var player2Moves = new List<Move>()
            {
                new Move(GameAction.Rock, player2),
                new Move(GameAction.Paper, player2),
                new Move(GameAction.Scissors, player2)
            };
            player2Mock.SetupSequence(m => m.SelectMove())
                .Returns(player2Moves[0])
                .Returns(player2Moves[1])
                .Returns(player2Moves[2]);

            PlayerMocks.Add(player1Mock);
            PlayerMocks.Add(player2Mock);

            Participants.Add(player1);
            Participants.Add(player2);

            Score = new Score
            (
                new Dictionary<IPlayer, int>()
                {
                    {player1,0},
                    {player2,0}
                }
            );

            var matchcommencementMessage = $"Match<{MatchId}>: Commence...";

            ControllerMock = new Mock<IUserInteraction>();
            ControllerMock.SetupSequence(m => m.Output(matchcommencementMessage))
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass();

            Controller = ControllerMock.Object;

            const int expectedplayer1Score = 0;
            const int expectedplayer2Score = 0;

            //Set Up Match Instance
            Match = new Match(MatchId, Participants, NumRounds, Score, Controller, GameResults, Rounds);

            const int expectedNumRoundsPlayed = 3;

            //Act
            var result = (MatchResult)Match.Play(GameRules);
            var theMatch = (Match)Match;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Draw, result.Outcome);
            ControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(13));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.Name, Times.Exactly(6));
            PlayerMocks[1].Verify(m => m.Name, Times.Exactly(6));
            Assert.IsNull(result.Winner);
            Assert.AreEqual(expectedplayer1Score, result.Score.Scores[Participants[0]]);
            Assert.AreEqual(expectedplayer2Score, result.Score.Scores[Participants[1]]);
            Assert.AreEqual(expectedNumRoundsPlayed, theMatch.Rounds.Count);
        }

        [TestMethod]
        public void Play_DrawBestOf32GamesDraw_ReturnExpectedResults()
        {
            //Arrange
            MatchId = 5;
            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            var player1 = player1Mock.Object;
            var player1Moves = new List<Move>()
            {
                new Move(GameAction.Rock, player1),
                new Move(GameAction.Paper, player1),
                new Move(GameAction.Scissors, player1)
            };

            player1Mock.SetupSequence(m => m.SelectMove())
                .Returns(player1Moves[0])
                .Returns(player1Moves[1])
                .Returns(player1Moves[2]);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            var player2 = player2Mock.Object;
            var player2Moves = new List<Move>()
            {
                new Move(GameAction.Scissors, player2),
                new Move(GameAction.Paper, player2),
                new Move(GameAction.Rock, player2)
            };
            player2Mock.SetupSequence(m => m.SelectMove())
                .Returns(player2Moves[0])
                .Returns(player2Moves[1])
                .Returns(player2Moves[2]);

            PlayerMocks.Add(player1Mock);
            PlayerMocks.Add(player2Mock);

            Participants.Add(player1);
            Participants.Add(player2);

            Score = new Score
            (
                new Dictionary<IPlayer, int>()
                {
                    {player1,0},
                    {player2,0}
                }
            );

            var matchcommencementMessage = $"Match<{MatchId}>: Commence...";

            ControllerMock = new Mock<IUserInteraction>();
            ControllerMock.SetupSequence(m => m.Output(matchcommencementMessage))
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass()
                .Pass();

            Controller = ControllerMock.Object;

            const int expectedplayer1Score = 1;
            const int expectedplayer2Score = 1;

            //Set Up Match Instance
            Match = new Match(MatchId, Participants, NumRounds, Score, Controller, GameResults, Rounds);

            const int expectedNumRoundsPlayed = 3;

            //Act
            var result = (MatchResult)Match.Play(GameRules);
            var theMatch = (Match)Match;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Draw, result.Outcome);
            ControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(13));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.Name, Times.Exactly(7));
            PlayerMocks[1].Verify(m => m.Name, Times.Exactly(7));
            Assert.IsNull(result.Winner);
            Assert.AreEqual(expectedplayer1Score, result.Score.Scores[Participants[0]]);
            Assert.AreEqual(expectedplayer2Score, result.Score.Scores[Participants[1]]);
            Assert.AreEqual(expectedNumRoundsPlayed, theMatch.Rounds.Count);
        }
    }
}