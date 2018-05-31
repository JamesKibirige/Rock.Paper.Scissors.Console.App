using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// 
    /// </summary>
    public class Application : IRunnable
    {
        /// <summary>
        /// A descriptive string detailing the Name of the application
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A descriptive string detailing the Author of the application
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The collection of Players that will be used when playing Matches and games
        /// </summary>
        public List<IPlayer> Players { get; set; }

        /// <summary>
        /// The rules that Matches and Games will be governed by
        /// </summary>
        public IGameRules Rules { get; set; }

        /// <summary>
        /// A Game Controller used to enforce a seperation of concerns for aspects of the Game pipeline
        /// </summary>
        public IUserInteraction InteractionController { get; set; }

        /// <summary>
        /// The Match that will be played
        /// </summary>
        public Contest Match { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"></see> class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="players"></param>
        /// <param name="rules"></param>
        /// <param name="interactionController"></param>
        /// <param name="match"></param>
        public Application(string title, string author, List<IPlayer> players, IGameRules rules,
            IUserInteraction interactionController, Contest match)
        {
            Title = title;
            Author = author;
            Players = players;
            Rules = rules;
            InteractionController = interactionController;
            Match = match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPlayer InitialisePlayer()
        {
            //  Request Human Player Details
            //  var validationExpression = "A-Z,1-9"
            //  var requestMessage = "Please Enter your name:\n"
            //  var playerName = Controller.RequestInput(requestMessage)
            //  Validate Input
            //  if Input is Valid continue
            //  else loop until user provides valid Input
            //  var playerName = Controller.RequestValidInput(requestMessage,validationExpression)
            //  var player = new HumanPlayer(playerName, Rules, InteractionController)
            //  return Human Player
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPlayer InitialiseOpponent()
        {
            //Initialise Opponent
            // Request Opponent Type based on list of options:
            // var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            // var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            // var requestMessage = $"Select an Opponent Type from possible the choices:\n\n{opponentTypeOptions}"
            // var opponentType = Controller.RequestInput(requestMessage)
            // Validate Input: IF IS NOT VALID loop 
            //  var opponentType = Controller.RequestValidInput(requestMessage)
            // Parse opponentType to OpponentType enum: Enum.TryParse(opponentType)
            // Switch on OpponentType and instantiate appropriate Player
            // IPlayer opponent = new HumanPlayer or IPlayer opponent = new HumanPlayer/RandomPlayer/TacticalPlayer()
            //return opponent;

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Contest InitialiseMatch()
        {
            //  var initMessage = "Initialising Match...\n\n"
            //  Controller.Output(initMessage)
            //  Request Match Length: "Specify Number of Games To Play?"
            //  var requestMessage = "Specify Number of Games To Play?:\n"
            //  var numRounds = Controller.RequestInput(requestMessage)
            //  Validate Input: Between 1 and 7 Games: Int try parse
            //  If invalid Loop until valid input: numRounds = (int)Controller.RequestValidInput(requestMessage)
            //  var match = new Match(1,Players,NumRounds,Score,Controller,GameResults,Rounds)
            //  return match
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IContestResult PlayMatch()
        {
            //var matchResult = Match.Play()
            throw new NotImplementedException();
        }

        /// <summary>
        /// Run Method details the game pipeline, involves the sequence of events that govern the game
        /// </summary>
        public void Run()
        {
            //var playagain = false;

            //do
            //{
            /*Enter Application Loop*/
            //  Display Title Splash to the User: "“Rock, Paper, Scissors by James Kibirige”" -->>>Display Message to the User -- resource file
            //  Controller.Output(Title);

            //  InitialisePlayer()

            //  InitialiseOpponent() 


            //  Match = InitialiseMatch()

            /*Enter Match Loop*/
            //var matchResult = Match.Play() -->>>PlayMatch()

            //  Display Match Result: -->>>Display Message to the User --MatchResult -- DisplayMatchResult(matchResult)
            //  Controller.Output(matchResult.ToString());

            //  Prompt to ask if Player wants to play again - Do you want to play again? -->>>User Input request
            //  playagain = Controller.RequestInput()
            //  Validate Input

            //  If Yes return to Application initialisation and play again --playagain == true;
            //  If No - End and close application --break loop --playagain == false
            // }while(playagain);
            throw new NotImplementedException();
        }
    }
}
