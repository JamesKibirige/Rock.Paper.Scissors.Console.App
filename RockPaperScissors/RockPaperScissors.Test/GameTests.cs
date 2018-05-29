using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class GameTests
    {
        /// <summary>
        /// Our Game instance that we will use to test the methods on the Game class
        /// </summary>
        public Contest Game { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IPlayer> Players { get; set; }

        /// <summary>
        /// Our Mock collection of Players that will be used to set up and run our unit tests
        /// </summary>
        public List<Mock<IPlayer>> PlayerMocks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IMove> Moves { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IUserInteraction UserInteractionController { get; set; }

        /// <summary>
        /// Our Mock user Interactioon controller
        /// </summary>
        public Mock<IUserInteraction> UiControllerMock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGameRules GameRules { get; set; }

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
            Players = new List<IPlayer>();
            Moves = new List<IMove>();
            UiControllerMock = new Mock<IUserInteraction>();
        }

        [TestMethod]
        public void CommencementMessage_SetUpGameWithId_ReturnsCorrectCommencementMessage()
        {
            //Arrange
            GameId = 0;
            var expected = $"Game<{GameId}>: Commence...";

            UserInteractionController = UiControllerMock.Object;

            Game = new Game(GameId, Players, Moves, UserInteractionController);

            //Act
            var result = Game.CommencementMessage;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void Play_OutcomeDrawNonHumanPlayers_ReturnsExpectedGameResult()
        {
            //Arrange
            GameId = 1;

            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");

            PlayerMocks.Add(player1Mock);
            var player1 = player1Mock.Object;
            Players.Add(player1);
            var player1Move = new Move(GameAction.Paper, Players[0]);
            player1Mock.Setup(m => m.SelectMove()).Returns(player1Move);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Other Player");

            PlayerMocks.Add(player2Mock);
            var player2 = player2Mock.Object;
            Players.Add(player2);
            var player2Move = new Move(GameAction.Paper, Players[1]);
            player2Mock.Setup(m => m.SelectMove()).Returns(player2Move);

            var commencementMessage = $"Game<{GameId}>: Commence...";

            UiControllerMock.SetupSequence(m => m.Output(commencementMessage)).Pass().Pass();
            UserInteractionController = UiControllerMock.Object;

            Game = new Game(GameId,Players,Moves,UserInteractionController);

            //Act
            var result = (GameResult)Game.Play(GameRules);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Draw,result.Outcome);
            Assert.AreEqual(result.Moves[0].Action,result.Moves[1].Action);
            UiControllerMock.Verify(m=>m.Output(It.IsAny<string>()),Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.SelectMove(),Times.Once);
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Once);
            PlayerMocks[0].Verify(m => m.Name, Times.Once);
            PlayerMocks[1].Verify(m => m.Name, Times.Once);
            Assert.IsNull(result.WinningMove);
        }

        [TestMethod]
        public void Play_OutcomeWinNonHumanPlayersMove1Wins_ReturnsExpectedGameResult()
        {
            //Arrange
            GameId = 2;

            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("John Hanley");

            PlayerMocks.Add(player1Mock);
            var player1 = player1Mock.Object;
            Players.Add(player1);
            var player1Move = new Move(GameAction.Rock, Players[0]);
            player1Mock.Setup(m => m.SelectMove()).Returns(player1Move);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Other Player");

            PlayerMocks.Add(player2Mock);
            var player2 = player2Mock.Object;
            Players.Add(player2);
            var player2Move = new Move(GameAction.Scissors, Players[1]);
            player2Mock.Setup(m => m.SelectMove()).Returns(player2Move);

            var commencementMessage = $"Game<{GameId}>: Commence...";

            UiControllerMock.SetupSequence(m => m.Output(commencementMessage)).Pass();
            UserInteractionController = UiControllerMock.Object;

            Game = new Game(GameId, Players, Moves, UserInteractionController);

            //Act
            var result = (GameResult)Game.Play(GameRules);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Once);
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Once);
            PlayerMocks[0].Verify(m => m.Name, Times.Once);
            PlayerMocks[1].Verify(m => m.Name, Times.Once);
            Assert.IsNotNull(result.WinningMove);
            Assert.AreEqual(result.WinningMove, player1Move);
        }

        [TestMethod]
        public void Play_OutcomeWinNonHumanPlayersMove2Wins_ReturnsExpectedGameResult()
        {
            //Arrange
            GameId = 3;

            var player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Rules).Returns(GameRules);
            player1Mock.Setup(m => m.Name).Returns("John Hanley");

            PlayerMocks.Add(player1Mock);
            var player1 = player1Mock.Object;
            Players.Add(player1);
            var player1Move = new Move(GameAction.Paper, Players[0]);
            player1Mock.Setup(m => m.SelectMove()).Returns(player1Move);

            var player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Rules).Returns(GameRules);
            player2Mock.Setup(m => m.Name).Returns("Other Player");

            PlayerMocks.Add(player2Mock);
            var player2 = player2Mock.Object;
            Players.Add(player2);
            var player2Move = new Move(GameAction.Scissors, Players[1]);
            player2Mock.Setup(m => m.SelectMove()).Returns(player2Move);

            var commencementMessage = $"Game<{GameId}>: Commence...";

            UiControllerMock.SetupSequence(m => m.Output(commencementMessage)).Pass();
            UserInteractionController = UiControllerMock.Object;

            Game = new Game(GameId, Players, Moves, UserInteractionController);

            //Act
            var result = (GameResult)Game.Play(GameRules);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ContestOutcome.Win, result.Outcome);
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Exactly(3));
            PlayerMocks[0].Verify(m => m.SelectMove(), Times.Once);
            PlayerMocks[1].Verify(m => m.SelectMove(), Times.Once);
            PlayerMocks[0].Verify(m => m.Name, Times.Once);
            PlayerMocks[1].Verify(m => m.Name, Times.Once);
            Assert.IsNotNull(result.WinningMove);
            Assert.AreEqual(result.WinningMove, player2Move);
        }
    }
}
