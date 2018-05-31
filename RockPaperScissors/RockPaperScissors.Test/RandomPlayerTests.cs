using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class RandomPlayerTests
    {
        /// <summary>
        /// The Player instance that we will be using to run our tests
        /// </summary>
        public Player thePlayer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGameRules GameRules { get; set; }

        /// <summary>
        /// Mock of the IGameRules interface that we will use to Mock the dependancy to the GameRules instance member
        /// </summary>
        public Mock<IGameRules> GameRulesMock { get; set; }

        /// <summary>
        /// The random number generator that our RandomComputer Player will use to select moves at random
        /// </summary>
        public Random Rand { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            var playerName = "RandomPlayer";

            GameRulesMock = new Mock<IGameRules>();
            GameRulesMock.Setup(m => m.Rules).Returns(new Dictionary<GameAction, List<GameAction>>()
            {
                {
                    GameAction.Rock,
                    new List<GameAction>(){GameAction.Scissors}
                },
                {
                    GameAction.Paper,
                    new List<GameAction>(){GameAction.Rock}
                },
                {
                    GameAction.Scissors,
                    new List<GameAction>(){GameAction.Paper}
                }
            });
            GameRulesMock.Setup(m => m.PossibleGameActions()).Returns( new List<GameAction>()
            {
                GameAction.Rock,
                GameAction.Paper,
                GameAction.Scissors
            });

            GameRules = GameRulesMock.Object;

            Rand = new Random();

            thePlayer = new RandomPlayer(playerName,GameRules, Rand);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void SelectMove_SelectsValidMoveAtRandom_ReturnsValidMove(int var)
        {
            //Arrange
            //Act
            var result = thePlayer.SelectMove();
            var validGameAction = Enum.IsDefined(typeof(GameAction), result.Action);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Move));
            Assert.AreEqual(result.Owner.Name, "RandomPlayer");
            Assert.IsTrue(validGameAction);
        }
    }
}