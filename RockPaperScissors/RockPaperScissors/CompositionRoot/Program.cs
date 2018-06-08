using RockPaperScissors.Enums;
using RockPaperScissors.Properties;
using RockPaperScissors.RegistryDSL;
using StructureMap;
using System.Collections.Generic;

namespace RockPaperScissors.CompositionRoot
{
    /// <summary>
    /// Comnposition Root class and Application entry point
    /// </summary>
    public class Program
    {
        public static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            //Initialise Application Dependancies
            var title = Resources.ApplicationTitle;
            var author = Resources.ApplicationAuthor;
            var players = new List<IPlayer>();

            var rulesDictionary = new Dictionary<GameAction, List<GameAction>>()
            {
                {
                    GameAction.Rock,
                    new List<GameAction>() {GameAction.Scissors}
                },
                {
                    GameAction.Paper,
                    new List<GameAction>() {GameAction.Rock}
                },
                {
                    GameAction.Scissors,
                    new List<GameAction>() {GameAction.Paper}
                }
            };

            var rules = new GameRules(rulesDictionary);
            Dictionary<IPlayer, int> scoresDictionary = new Dictionary<IPlayer, int>();
            var score = new Score(scoresDictionary);
            var consoleAdapter = new ConsoleAdapter();
            var interactionController = new ConsoleInteractionController(consoleAdapter);

            //Initialise Application
            var application = new Application(title, author, players, rules, interactionController, null, score, null);

            //Run Application
            application.Run();
        }

        /// <summary>
        /// Configure Dependancy Injection Container
        /// </summary>
        public static void ConfigureContainer()
        {
            //Register Dependancies
            Container = new Container(c =>
            {
                c.AddRegistry<ApplicationRegistry>();
                c.AddRegistry<UserInteractionRegistry>();
                c.AddRegistry<ContestRegistry>();
            });
        }
    }
}
