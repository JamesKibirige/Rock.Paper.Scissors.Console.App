using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class GameResultTests
    {
        /// <summary>
        /// Our GameResult instance that we are testing
        /// </summary>
        public GameResult GameResult { get; set; }

        /// <summary>
        /// The Outcome instance that we will uise to initilaize our GameResult instance
        /// </summary>
        public ContestOutcome Outcome { get; set; }

        /// <summary>
        /// The identification number for the Game involved
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// The collection of Moves played
        /// </summary>
        public List<IMove> Moves { get; set; }

        /// <summary>
        /// The Winning Move
        /// </summary>
        public IMove WinningMove { get; set; }

        [TestMethod]
        public void ToString_ContestOutComeWin_ReturnsExpectedStringMessage()
        {
            //Arrange
            Outcome = ContestOutcome.Win;
            ContestId = 1;
            var expected = "Game<1>: Result\n\n" + "Player<James Kibirige>: Played <Paper>\n" +
                           "Player<Jonita Laidley>: Played <Rock>\n\n" + "Paper beats Rock\n\n" +
                           "Player<James Kibirige> wins Game<1>!!!\n";

            Mock<IPlayer> player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);
            player1Mock.Setup(m => m.SelectMove());

            Mock<IPlayer> player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);
            player2Mock.Setup(m => m.SelectMove());

            Moves = new List<IMove>()
            {
                new Move(GameAction.Paper,player1Mock.Object),
                new Move(GameAction.Rock,player2Mock.Object)
            };

            WinningMove = Moves.First(m => m.Action == GameAction.Paper);
            GameResult = new GameResult(Outcome,ContestId,Moves,WinningMove);

            //Act
            var result = GameResult.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result,expected);
        }

        [TestMethod]
        public void ToString_ContestOutComeDraw_ReturnsExpectedStringMessage()
        {
            //Arrange
            Outcome = ContestOutcome.Draw;
            ContestId = 2;
            var expected = "Game<2>: Result\n\n" + "Player<James Kibirige>: Played <Scissors>\n" +
                           "Player<Jonita Laidley>: Played <Scissors>\n\n" + "Draw\n";

            Mock<IPlayer> player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);
            player1Mock.Setup(m => m.SelectMove());

            Mock<IPlayer> player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);
            player2Mock.Setup(m => m.SelectMove());

            Moves = new List<IMove>()
            {
                new Move(GameAction.Scissors,player1Mock.Object),
                new Move(GameAction.Scissors,player2Mock.Object)
            };

            WinningMove = null;//Outcome is a draw and thus there is no winning Move
            GameResult = new GameResult(Outcome, ContestId, Moves, WinningMove);

            //Act
            var result = GameResult.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, expected);
        }
    }
}