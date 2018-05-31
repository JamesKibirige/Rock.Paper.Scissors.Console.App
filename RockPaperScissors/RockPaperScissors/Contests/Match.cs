using RockPaperScissors.Interfaces;
using System;
using System.Collections.Generic;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;

namespace RockPaperScissors
{
    /// <summary>
    /// A Match is a Playable Contest that features a collection of Rounds that are played
    /// </summary>
    public class Match : Contest
    {
        /// <summary>
        /// A Match has a collection of Games that denote Rounds that are played by participating Players
        /// </summary>
        public List<Contest> Rounds { get; set; }

        /// <summary>
        /// A Match is limited to a finite number of rounds that can be played.
        /// </summary>
        public int NumRounds { get; set; }

        /// <summary>
        /// A Match has a score that is decifered based on the otucomes of the individual games of the match
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// A match stores and builds collection of Contest Results that reflect the results of the games played during the match
        /// </summary>
        public List<IContestResult> GameResults { get; set; }

        /// <summary>
        ///  Constructor injecting in the Match instance dependancies
        /// </summary>
        /// <param name="id"></param>
        /// <param name="players"></param>
        /// <param name="numGames"></param>
        /// <param name="score"></param>
        /// <param name="interactionController"></param>
        /// <param name="gameResults"></param>
        /// <param name="rounds"></param>
        public Match(int id, List<IPlayer> players, int numGames, IScore score, IUserInteraction interactionController, List<IContestResult> gameResults, List<Contest> rounds)
        {
            Id = id;
            Participants = players;
            NumRounds = numGames;
            Score = score;
            Controller = interactionController;
            GameResults = gameResults;
            Rounds = rounds;
        }

        /// <summary>
        /// Play  method that involves the match playing pipeline, it injects a GameRules instance to use to play the match.
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public override IContestResult Play(IGameRules rules)
        {
            //  Play each Game that needs to be played based on the specified number of rounds: NumRounds
            //  Display Message to User: Commence Match: Match<1>: Commence...
            Controller.Output(CommencementMessage);

            var gameCounter = 0;
            int numGamesLeft;
            int player1Score;
            int player2Score;

            /*Enter Match Loop*/
            do
            {
                //Increment Game counter
                gameCounter++;

                //Initialise a new Game
                var currentGame = new Game(gameCounter, Participants, new List<IMove>(), Controller);

                //Play the current game and capture the game result:
                var currentGameResult = (GameResult)currentGame.Play(rules);

                //Add gameResult to our GameResults collection:
                GameResults.Add(currentGameResult);

                //Display Game Result Message to User:
                Controller.Output(currentGameResult.ToString());

                //If currentGameResult Outcome is a Win
                if (currentGameResult.Outcome == ContestOutcome.Win)
                {
                    //Increment Match Score for the Player of the winning Move:
                    Score.Scores[currentGameResult.WinningMove.Owner]++; 
                }

                //Add current game to Rounds collection:
                Rounds.Add(currentGame);

                //Check if the Match is over: Match ENDS When:  Number of games left == 0 OR (Either Players Score) > number of games left                           

                numGamesLeft = NumRounds - Rounds.Count; //  number of games left = (Total Number of Games) - (Number games played)
                player1Score = Score.Scores[Participants[0]];//  Total Number of Games = NumRounds
                player2Score = Score.Scores[Participants[1]]; //  Number games played = Rounds.Count

            } while (!(numGamesLeft == 0 || player1Score > numGamesLeft || player2Score > numGamesLeft));

            //Determine the Outcome and Winner of the Match
            if (player1Score == player2Score)
            {
                Outcome = ContestOutcome.Draw;
            }
            else
            {
                Outcome = ContestOutcome.Win;
                Winner = player1Score > player2Score ? Participants[0] : Participants[1];
            }

            //Initialize the Match Result
            var matchResult = new MatchResult(Id, Winner, Score, Participants, Outcome);

            //Return the Match Result
            return matchResult;
        }
    }
}
