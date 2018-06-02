using System;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Players
{
    /// <summary>
    /// Class representing a HumanPlayer in the Rock Paper Scissors game. Human players select Moves based on User input
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// Human Players need an instance of the UiController to Request input from the User
        /// </summary>
        public IUserInteraction Controller { get; set; }

        /// <summary>
        /// Constructor injecting dependancies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rules"></param>
        /// <param name="controller"></param>
        public HumanPlayer(string name, IGameRules rules, IUserInteraction controller)
        {
            Name = name;
            Rules = rules;
            Controller = controller;
        }

        /// <summary>
        /// Human Players select Moves based on User Input 
        /// </summary>
        /// <returns></returns>
        public override Move SelectMove()
        {
            bool validInput = false;
            GameAction playerAction;
            var possibleActions = string.Join(",", Rules.PossibleGameActions());//String literal listing possible game actions

            do
            {
                var promptInputMessage = string.Format("Player <{0}>\nSelect a Move from the possible choices:\n{{{1}}}", Name, possibleActions);
                var userInput = Controller.RequestInput(promptInputMessage).ToString();//Request input from User 

                validInput = Enum.TryParse<GameAction>(userInput,out playerAction);

                if (!validInput)//Output Message telling the user that their input was invalid
                {
                    var validationMessage = string.Format("Invalid Input: {0}\nPlease provide a Valid Action\nThe set of acceptable Actions are as follows: {{{1}}}", userInput, possibleActions);
                    Controller.Output(validationMessage);
                }
            } while (!validInput);//Loop while users input is invalid

            //Set Up Move based on Users input Game Action -- var result = new Move(playerAction,this)
            var result = new Move(playerAction,this);

            return result;
        }
    }
}
