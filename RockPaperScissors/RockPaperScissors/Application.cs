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
        /// Run Method details the game pipeline, involves the sequence of events that govern the game
        /// </summary>
        public void Run()
        {
            //var playagain = false;

            //do
            //{
            /*Enter Application Loop*/
            //  Display Title Splash to the User: "“Rock, Paper, Scissors by James Kibirige”" -->>>Display Message to the User
            //  Controller.Output(Title);

            //  Request Opponent Type based on list of options- "Select Opponent Type" - The list of options could also be configurable --->>>Initialise Players -->>>User Input request
            // var opponentType = Controller.RequestInput()
            //  Validate Input: Try Parse opponentType to OpponentType enum

            //  Request Human Player Details --->>>Initialise Players -->>>User Input request
            //  var playerName = Controller.RequestInput()
            //  Request Match Length: "How many Games in a Match?" -->>>Set Number of Games -->>>User Input request
            //  Validate Input: Between 1 and 7 Games

            /*Enter Match Loop*/
            //var matchResult = Match.Play()

            //  Display Match Result: -->>>Display Message to the User --MatchResult
            //  Controller.Output(matchResult.ToString());

            //  Prompt to ask if Player wants to play again - Do you want to play again? -->>>User Input request
            //  Controller.RequestInput()
            //  Validate Input
            //  If Yes return to Application initialisation and play again --playagain = true;
            //  If No - End and close application --break loop --playagain = false
            // }while(playagain);
            throw new NotImplementedException();
        }
    }
}
