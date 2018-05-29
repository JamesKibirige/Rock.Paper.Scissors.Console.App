using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class HumanPlayerTests
    {
        public IPlayer HumanPlayer { get; set; }
        public IGameRules Rules { get; set; }
        public IUserInteraction UiController { get; set; }

        public Mock<IGameRules> GameRulesMock { get; set; }
        public Mock<IUserInteraction> UiControllerMock { get; set; }

        [TestInitialize]
        public void Setup()
        {
            //Set up Mocks and Mock instances for all of HumanPlayers dependancies so that we can Unit test HumanPlayer
            GameRulesMock = new Mock<IGameRules>();
            var gameActionDictionary = new Dictionary<GameAction, List<GameAction>>()
            {
                {
                    GameAction.Rock,
                    new List<GameAction>(){GameAction.Scissors}
                },
                {
                    GameAction.Scissors,
                    new List<GameAction>(){GameAction.Paper}
                },
                {
                    GameAction.Paper,
                    new List<GameAction>(){GameAction.Rock}
                }
            };
            GameRulesMock.Setup(m => m.Rules).Returns(gameActionDictionary);

            var possibleGameActions = new List<GameAction>()
            {
                GameAction.Rock,
                GameAction.Paper,
                GameAction.Scissors
            };
            GameRulesMock.Setup(m => m.PossibleGameActions()).Returns(possibleGameActions);

            Rules = GameRulesMock.Object;

            var possibleActions = string.Join(",", possibleGameActions.ToArray());

            //Mock Set Up for Player with Valid Move: James Kibirige
            var player1Name = "James Kibirige";
            var validuserInput = "Rock";
            var promptInputMessagePlayer1 = string.Format("\nPlayer <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", player1Name, possibleActions);

            //Mock Set Up for Player with Invalid Move: Jonita Laidley
            var player2Name = "Jonita Laidley";
            var invaliduserInput = "Lizard";
            var validuserInput2 = "Paper";
            var promptInputMessagePlayer2 = string.Format("\nPlayer <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", player2Name, possibleActions);
            var validationMessage = string.Format("\nInvalid Input: {0}\nPlease provide a Valid Action\nThe set of acceptable Actions are as follows: {{{1}}}", invaliduserInput, possibleActions);

            UiControllerMock = new Mock<IUserInteraction>();
            UiControllerMock.Setup(m => m.RequestInput(promptInputMessagePlayer1)).Returns(validuserInput);
            UiControllerMock.SetupSequence(m => m.RequestInput(promptInputMessagePlayer2)).Returns(invaliduserInput).Returns(validuserInput2);
            UiControllerMock.Setup(m => m.Output(validationMessage));
            UiController = UiControllerMock.Object;
        }

        [TestMethod]
        public void SelectMove_SelectValidGameAction_ReturnsExpectedMove()
        {
            //Arrange
            var playerName = "James Kibirige";
            HumanPlayer = new HumanPlayer(playerName, Rules, UiController);
            var expected = new Move(GameAction.Rock,HumanPlayer);

            //Act
            var result = HumanPlayer.SelectMove();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Action, expected.Action);
            Assert.AreEqual(result.Owner, expected.Owner);
        }

        [TestMethod]
        public void SelectMove_SelectInvalidGameAction_LoopsUntilValidInputProvided()
        {
            //Arrange
            var playerName = "Jonita Laidley";
            HumanPlayer = new HumanPlayer(playerName, Rules, UiController);

            var invaliduserInput = "Lizard";
            var possibleActions = "Rock, Paper, Scissors";

            var promptInputMessage = string.Format("\nPlayer <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", playerName, possibleActions);
            var validationMessage = string.Format("\nInvalid Input: {0}\nPlease provide a Valid Action\nThe set of acceptable Actions are as follows: {{{1}}}", invaliduserInput, possibleActions);

            //Act
            var result = HumanPlayer.SelectMove();
            var expected = new Move(GameAction.Paper,HumanPlayer);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Owner, expected.Owner);
            Assert.AreEqual(result.Action, expected.Action);
            UiControllerMock.Verify(m => m.RequestInput(It.IsAny<string>()),Times.Exactly(2));
            UiControllerMock.Verify(m => m.Output(It.IsAny<string>()), Times.Once);
        }
    }
}
