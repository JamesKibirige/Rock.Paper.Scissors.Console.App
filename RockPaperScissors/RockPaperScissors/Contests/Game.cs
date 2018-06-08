using RockPaperScissors.ContestResults;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors
{
    /// <summary>
    /// A Game is a Playable instance that features a collection of Moves played by a collection of players.
    /// </summary>
    public class Game : Contest
    {
        /// <summary>
        /// A Game has a collection of played Moves
        /// </summary>
        public List<IMove> Moves { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="players"></param>
        /// <param name="moves"></param>
        /// <param name="interactionController"></param>
        public Game(int id, List<IPlayer> players, List<IMove> moves, IUserInteraction interactionController)
        {
            Id = id;
            Participants = players;
            Moves = moves;
            Controller = interactionController;
        }

        /// <summary>
        /// A game can be played according to a set of Game rules to generate a Game Result
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public override IContestResult Play(IGameRules rules)
        {
            var result = new GameResult(ContestOutcome.None, Id, Moves, null);

            //Display Message to User: Game<Id>: Commence...
            Controller.Output(CommencementMessage);

            //Request Moves from Each Player
            foreach (var player in Participants)
            {
                var playerMove = player.SelectMove();
                Moves.Add(playerMove);
            }

            //Display Message to User: Player 2: <name> Plays <Move.Action>
            //  if this is a game against a non Human Player
            var countHumans = Participants.Count(p => p.GetType() == typeof(HumanPlayer));

            switch (countHumans)
            {
                case 1:
                    //  if this is a game against a non Human Player
                    //  Display Message: Player 2: <name> Played <Move.Action>
                    var player2MoveMessage = Moves.First(m => m.Owner.GetType() != typeof(HumanPlayer)).ToString();
                    Controller.Output(player2MoveMessage);
                    break;
                case 0:
                    //If neither Player is a Human
                    // Display Message:
                    // Player 1: <name> Played <Move.Action>
                    // Player 2: <name> Played <Move.Action>
                    foreach (var move in Moves)
                    {
                        var playermoveMessage = move.ToString();
                        Controller.Output(playermoveMessage);
                    }
                    break;
            }

            //Use Game Rules to Play each Players selected Moves against each other
            //  Check for a Draw: Player 1 Move action is the same as Player 2 move action: player1Move.Action == player2Move.Action :Draw
            if (Moves[0].Action == Moves[1].Action)
            {
                //Draw
                Outcome = ContestOutcome.Draw;
                Winner = null;
                result = new GameResult(Outcome,Id,Moves,null);
            }

            //Get losing actions for the Moves that were played by each player
            //  A losing action is the action that a Players Move beats based on the mappings in the GameRules dictionary: rules
            var move0Losingactions = rules.Rules[Moves[0].Action];
            var move1Losingactions = rules.Rules[Moves[1].Action];

            //Figure out which Move wins and who played the winning Move
            //  Moves[0] wins: If Moves[1] Action IN Moves[0] losing actions:
            if (move0Losingactions.Contains(Moves[1].Action))
            {
                //Moves[0] wins
                Outcome = ContestOutcome.Win;
                Winner = Moves[0].Owner;
                result = new GameResult(Outcome,Id,Moves,Moves[0]);
            }

            //  Moves[1] wins: If Moves[0] Action IN Moves[1] losing actions:
            if (move1Losingactions.Contains(Moves[0].Action))
            {
                //Moves[1] wins
                Outcome = ContestOutcome.Win;
                Winner = Moves[1].Owner;
                result = new GameResult(Outcome, Id, Moves, Moves[1]);
            }

            return result;
        }
    }
}