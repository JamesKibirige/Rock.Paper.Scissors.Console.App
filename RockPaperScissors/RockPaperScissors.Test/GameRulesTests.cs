using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Enums;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class GameRulesTests
    {
        public GameRules Rules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            Rules = new GameRules(new Dictionary<GameAction, List<GameAction>>());
        }

        [TestMethod]
        public void PossibleGameActions_GetListOfPossibleGameActions_ReturnsExpectedResults()
        {
            //Arrange
            var expected = new List<GameAction>()
            {
                GameAction.Rock,
                GameAction.Paper,
                GameAction.Scissors
            };

            //Act
            var result = Rules.PossibleGameActions();

            //Assert
            Assert.IsNotNull(result);
            CollectionAssert.AllItemsAreNotNull(result);
            Assert.AreEqual(result.Count, expected.Count);
            CollectionAssert.AreEquivalent(result, expected);
            CollectionAssert.AllItemsAreInstancesOfType(result,typeof(GameAction));
        }
    }
}
