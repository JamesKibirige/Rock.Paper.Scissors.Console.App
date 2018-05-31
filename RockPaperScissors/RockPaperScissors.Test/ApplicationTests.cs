using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class ApplicationTests
    {
        public IRunnable Application { get; set; }

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
        /// 
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {

        }
    }
}