using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class MatchResultTests
    {
        /// <summary>
        /// Our MatchResult instance that we are testing
        /// </summary>
        public MatchResult MatchResult { get; set; }

        /// <summary>
        /// The identification number for the Contest involved
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// Winner of the match may be NUll is the Match Outcome is a draw
        /// </summary>
        public IPlayer Winner { get; set; }

        /// <summary>
        /// What was the Match Score
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// Player Participants in the Match
        /// </summary>
        public List<IPlayer> Participants { get; set; }

        /// <summary>
        /// The Outcome of the Match
        /// </summary>
        public ContestOutcome Outcome { get; set; }

        [TestMethod]
        public void ToString_ContestOutComeWin_ReturnsExpectedStringMessage()
        {
            //Arrange
            Outcome = ContestOutcome.Win;
            ContestId = 1;
            var expected = "Match<1>: Result\n\n" +
                           "Match Outcome<Win>\n\n" +
                           "Player<James Kibirige>: Score <2>\n" +
                           "Player<Jonita Laidley>: Score <1>\n\n"+
                           "Player<James Kibirige> Is the Winner!!!\n";

            Mock<IPlayer> player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);
            player1Mock.Setup(m => m.SelectMove());

            Mock<IPlayer> player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);
            player2Mock.Setup(m => m.SelectMove());

            Participants = new List<IPlayer>()
            {
                player1Mock.Object,
                player2Mock.Object
            };

            Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>()
            {
                {Participants[0],2},
                {Participants[1],1}
            };

            Score = new Score(scores);

            MatchResult = new MatchResult(ContestId,Participants[0],Score,Participants,Outcome);

            //Act
            var result = MatchResult.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToString_ContestOutComeDraw_ReturnsExpectedStringMessage()
        {
            //Arrange
            Outcome = ContestOutcome.Draw;
            ContestId = 2;
            var expected = "Match<2>: Result\n\n" +
                           "Match Outcome<Draw>\n\n" +
                           "Player<James Kibirige>: Score <2>\n" +
                           "Player<Jonita Laidley>: Score <2>\n\n";

            Mock<IPlayer> player1Mock = new Mock<IPlayer>();
            player1Mock.Setup(m => m.Name).Returns("James Kibirige");
            player1Mock.Setup(m => m.Rules);
            player1Mock.Setup(m => m.SelectMove());

            Mock<IPlayer> player2Mock = new Mock<IPlayer>();
            player2Mock.Setup(m => m.Name).Returns("Jonita Laidley");
            player2Mock.Setup(m => m.Rules);
            player2Mock.Setup(m => m.SelectMove());

            Participants = new List<IPlayer>()
            {
                player1Mock.Object,
                player2Mock.Object
            };

            Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>()
            {
                {Participants[0],2},
                {Participants[1],2}
            };

            Score = new Score(scores);

            MatchResult = new MatchResult(ContestId, Participants[0], Score, Participants, Outcome);

            //Act
            var result = MatchResult.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}