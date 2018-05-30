using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class EnumEnumeratorTests
    {
        /// <summary>
        /// Our Enum Enumerator instance that will be used in our Unit tests
        /// </summary>
        public IEnumEnumerator<OpponentType> OpponentTypeEnumerator { get; set; }

        /// <summary>
        /// Our Enum Enumerator instance that will be used in our Unit tests
        /// </summary>
        public IEnumEnumerator<ContestOutcome> ContestOutcomeEnumerator { get; set; }

        /// <summary>
        /// Our Enum Enumerator instance that will be used in our Unit tests
        /// </summary>
        public IEnumEnumerator<GameAction> GameActionEnumerator { get; set; }

        [TestMethod]
        public void EnumerateEnum_OpponentTypeEnumerator_ReturnsListOfOpponentTypes()
        {
            //Arrange
            OpponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            const int expectedNumberOfItems = 3;

            //Act
            var result = OpponentTypeEnumerator.EnumerateEnum();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfItems, result.Count);
        }

        [TestMethod]
        public void EnumerateEnum_ContestOutcomeEnumerator_ReturnsListOfOpponentTypes()
        {
            //Arrange
            ContestOutcomeEnumerator = new EnumEnumerator<ContestOutcome>();
            const int expectedNumberOfItems = 2;

            //Act
            var result = ContestOutcomeEnumerator.EnumerateEnum();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfItems, result.Count);
        }

        [TestMethod]
        public void EnumerateEnum_GameActionEnumerator_ReturnsListOfGameActions()
        {
            //Arrange
            GameActionEnumerator = new EnumEnumerator<GameAction>();
            const int expectedNumberOfItems = 3;

            //Act
            var result = GameActionEnumerator.EnumerateEnum();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedNumberOfItems, result.Count);
        }
    }
}
