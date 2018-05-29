using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class MoveTests
    {
        /// <summary>
        /// 
        /// </summary>
        public IMove Move { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPlayer Player { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Mock<IPlayer> PlayerMock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GameAction Action { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            PlayerMock = new Mock<IPlayer>();
            PlayerMock.Setup(m => m.Name).Returns("Jonita Laidley");
            Player = PlayerMock.Object;
            Action = GameAction.Rock;
            Move = new Move(Action,Player);
        }

        [TestMethod]
        public void ToString_CorrectStringRepresentingMoveReturned_ExpectedStringValue()
        {
            //Arrange
            var expectedResult = string.Format("Player<{0}>: Played <{1}>", Player.Name, Action.ToString());
            //Act
            var result = Move.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedResult);
        }
    }
}