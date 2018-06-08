using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        /// The instance that will be used to track Scores in the game
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// A Match Result instance that wil be used to keep track of played match results
        /// </summary>
        public IContestResult MatchResult { get; set; }

        /// <summary>
        /// Read only property exposing a Title splash message. The Title Splash message details the Title and Author of the application.
        /// </summary>
        public string TitleSplash
        {
            get
            {
                var myVar = $"{Title}\n\nby {Author}";
                return myVar;
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="title"></param>
       /// <param name="author"></param>
       /// <param name="players"></param>
       /// <param name="rules"></param>
       /// <param name="interactionController"></param>
       /// <param name="match"></param>
       /// <param name="score"></param>
       /// <param name="matchResult"></param>
       public Application(string title, string author, List<IPlayer> players, IGameRules rules,
           IUserInteraction interactionController, Contest match, IScore score, IContestResult matchResult)
        {
            Title = title;
            Author = author;
            Players = players;
            Rules = rules;
            InteractionController = interactionController;
            Match = match;
            Score = score;
            MatchResult = matchResult;
        }

        /// <summary>
        /// Display the Title Splash to the user
        /// </summary>
        public void DisplayTitleSplash()
        {
            InteractionController.Output(TitleSplash);
        }

        /// <summary>
        /// This method initialises the main player of the application returning an instance that implements the IPlayer interface
        /// </summary>
        /// <returns></returns>
        public IPlayer InitialisePlayer()
        {
            //  Request Human Player Details
            var requestMessage = "Please Enter your name:\n\n{A-Z,a-z, }";
            var playerName = (string)InteractionController.RequestInput(requestMessage);

            //  Validate Input
            var rgx = new Regex(@"^[\p{L} \.'\-]+$");
            var isValid = rgx.IsMatch(playerName);
            if (!isValid)
            {
                playerName = InteractionController.RequestValidInput(requestMessage, rgx);
            }

            //Return Human Player instance
            var player = new HumanPlayer(playerName, Rules, InteractionController);

            return player;
        }

        /// <summary>
        /// Initialise the Opponent Player for the match users can choose from a finite collection of choices.
        /// </summary>
        /// <returns></returns>
        public IPlayer InitialiseOpponent()
        {
            //Initialise Opponent
            // Request Opponent Type based on list of options:
            var opponentTypeEnumerator = new EnumEnumerator<OpponentType>();
            var opponentTypeOptions = string.Join(",", opponentTypeEnumerator.EnumerateEnum());
            var requestMessage = $"Select an Opponent Type from the choices:\n\n{{{opponentTypeOptions}}}";

            var opponentType = (string)InteractionController.RequestInput(requestMessage);
            
            // Validate Input
            var isValid = opponentTypeOptions.Split(",").ToList().Contains(opponentType); ;
            if (!isValid)
            {
                var validationrequest = $"Please provide a Valid choice – The acceptable Opponent Types are as follows:\n\n{opponentTypeOptions}";
                opponentType = InteractionController.RequestValidInput(validationrequest, opponentTypeOptions);
            }

            Enum.TryParse<OpponentType>(opponentType, out var oppType);

            // Switch on OpponentType and instantiate appropriate Player
            IPlayer opponentPlayer = null;
            switch (oppType)
            {
                case OpponentType.Human:
                    opponentPlayer = new HumanPlayer("Human",Rules,InteractionController);
                    break;
                case OpponentType.Random:
                    opponentPlayer = new RandomPlayer("Random", Rules, new Random());
                    break;
                case OpponentType.Tactical:
                    opponentPlayer = new TacticalPlayer("Tactical", Rules,new Stack<IMove>(),new Random());
                    break;
            }

            return opponentPlayer;
        }

        /// <summary>
        /// Initialises an instance of the Match that will be played in our application
        /// </summary>
        /// <returns></returns>
        public Contest InitialiseMatch(int matchId)
        {
            var initMessage = "Initialising Match...\n\n";
            InteractionController.Output(initMessage);

            Regex rgx = new Regex(@"^[0-9]+$");//Regular expression that only matches when complete string is a number

            //  Request Match Length: "Specify Number of Games To Play?"
            var requestMessage = $"Please Specify number of rounds to play in Match:\n\n{{number using digits 1-9 between 1 and 10}}";
            var numRoundsString = InteractionController.RequestInput(requestMessage).ToString();
            var canParse = int.TryParse(numRoundsString, out var numberRounds);

            //  Validate Input: Between 1 and 9 Games: Try parse
            while (!(canParse && numberRounds > 0 && numberRounds < 10))
            {
                var validationRequest = $"Please provide a Valid choice:\n\n{{Must be a number greater than 0 and less than 10}}";
                var validNumRounds = InteractionController.RequestValidInput(validationRequest, rgx);
                canParse = int.TryParse(validNumRounds, out numberRounds);
            }

            //  Initialise the Score instance with the two Player instances already created.
            Score.Scores[Players[0]] = 0;
            Score.Scores[Players[1]] = 0;

            //Initialise and return Match instance
            var match = new Match(matchId, Players, numberRounds, Score, InteractionController, new List<IContestResult>(), new List<Contest>());

            //  return match
            return match;
        }

        /// <summary>
        /// Plays the Match and returns the MatchResult
        /// </summary>
        /// <returns></returns>
        public IContestResult PlayMatch()
        {
            //Execute Match.Play()
            var matchResult = Match.Play(Rules);

            //Return the MatchResult instance
            return matchResult;
        }

        /// <summary>
        /// Displays the results of a Match to the User
        /// </summary>
        public void DisplayMatchResult()
        {
            var matchResultMessage = MatchResult.ToString();
            InteractionController.Output(matchResultMessage);
        }

        /// <summary>
        /// Run Method details the game pipeline, involves the sequence of events that govern the game
        /// </summary>
        public void Run()
        {
            var playagain = false;
            var matchCounter = 0;

            do
            {
                /*Enter Application Loop*/
                DisplayTitleSplash();

                var player1 = InitialisePlayer();
                Players.Add(player1);

                var player2 = InitialiseOpponent();
                Players.Add(player2);

                matchCounter++;
                Match = InitialiseMatch(matchCounter);

                MatchResult = Match.Play(Rules);

                DisplayMatchResult();

                var yesNoOptions = "Yes,No";
                var playAgainRequestMessage = $"Do want to Play Again?\n\n{{{yesNoOptions}}}";

                //Does user want to play again?
                var userPlayAgainInput = (string)InteractionController.RequestInput(playAgainRequestMessage);
                var isValid = yesNoOptions.Split(",").ToList().Contains(userPlayAgainInput);
                if (!isValid)
                {
                    var validationrequest = $"Please provide a Valid choice – The acceptable options are as follows:\n\n{yesNoOptions}";
                    userPlayAgainInput = InteractionController.RequestValidInput(validationrequest, yesNoOptions);
                }

                Enum.TryParse<YesNo>(userPlayAgainInput, out var playAgainEnum);
                switch (playAgainEnum)
                {
                    case YesNo.Yes:
                        playagain = true;

                        //Reset instance variables for second play through
                        Players.Clear();
                        Match = null;
                        MatchResult = null;
                        Score.Scores.Clear();
                        break;
                    case YesNo.No:
                    default:
                        playagain = false;
                        break;
                }
            } while (playagain);
        }
    }
}
