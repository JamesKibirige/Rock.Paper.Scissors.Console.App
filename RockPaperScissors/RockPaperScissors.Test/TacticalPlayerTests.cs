using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class TacticalPlayerTests
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
        /// The stack of Moves that will be passed as a dependancy to our TacticalPlayer instance
        /// </summary>
        public Stack<IMove> Moves { get; set; }

        /// <summary>
        /// The random number generator that our RandomComputer Player will use to select moves at random
        /// </summary>
        public Random Rand { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            const string playerName = "TacticalPlayer";

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
            GameRulesMock.Setup(m => m.PossibleGameActions()).Returns(new List<GameAction>()
            {
                GameAction.Rock,
                GameAction.Paper,
                GameAction.Scissors
            });

            GameRules = GameRulesMock.Object;

            Rand = new Random();

            Moves = new Stack<IMove>();

            thePlayer = new TacticalPlayer(playerName, GameRules, Moves,Rand);
        }

        [TestMethod]
        public void SelectMove_SelectsFirstMoveRandomlyThenSubsequentMovesTactically_SubsequentMovesBeatPreviousSelectedMoves()
        {
            //Arrange
            var selectedMoves = new List<Move>();
            var subsequentMoveResults = new List<bool>();

            //Act
            for (int i = 0; i < 9; i++)
            {
                var currentMove = thePlayer.SelectMove();
                selectedMoves.Add(currentMove);

                if (i > 0)
                {
                    //The current move should beat the last move
                    var lastMove = selectedMoves[i - 1];

                    //Look in the GameRules dictionary using currentMove.Action to find the collection of GameActions that the current move beats
                    var currentmovebeats = GameRules.Rules[currentMove.Action];

                    //Compare the actions that the currentMove.Action beats to the lastMove.action
                    var moveresult = currentmovebeats.Contains(lastMove.Action);

                    //Add this to collection of subsequentMoveResults
                    subsequentMoveResults.Add(moveresult);
                }
            }

            //Assert
            CollectionAssert.AllItemsAreNotNull(selectedMoves);
            CollectionAssert.AllItemsAreInstancesOfType(selectedMoves,typeof(Move));
            CollectionAssert.DoesNotContain(subsequentMoveResults,false);
        }
    }
}