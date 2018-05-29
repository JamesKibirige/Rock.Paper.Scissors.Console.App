using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class ConsoleInteractionControllerTests
    {
        public IConsoleAdapter ConsoleAdapter { get; set; }
        public Mock<IConsoleAdapter> MockConsoleAdapter { get; set; }
        public IUserInteraction ConsoleInteractionController { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            //Instantiate Mock ConsoleAdapter instance as UiController needs dependancy
            MockConsoleAdapter = new Mock<IConsoleAdapter>();

            //Set up Mock Console adapter so that we can Unit Test a UiController instance
            MockConsoleAdapter.Setup(m => m.ReadLine()).Returns("Test Input\n");
            MockConsoleAdapter.Setup(m => m.Write("Test Output Message"));
            MockConsoleAdapter.Setup(m => m.WriteLine("Test Output Message"));
            MockConsoleAdapter.Setup(m => m.WriteLine("Please enter Test value"));
            MockConsoleAdapter.Setup(m => m.WriteLine());

            ConsoleAdapter = MockConsoleAdapter.Object;
            ConsoleInteractionController = new ConsoleInteractionController(ConsoleAdapter);
        }

        [TestMethod]
        public void Output_OutputToConsole_MethodsInvoked()
        {
            //Arrange
            //Act
            ConsoleInteractionController.Output("Test Output Message");

            //Assert
            MockConsoleAdapter.Verify(m => m.WriteLine("Test Output Message"),Times.Once);
            MockConsoleAdapter.Verify(m => m.WriteLine(), Times.Once);
            MockConsoleAdapter.Verify(m => m.WriteLine("Press Any Key To continue..."), Times.Once);
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Once);
        }

        [TestMethod]
        public void RequestInput_ReadInputFromConsole_MethodsInvoked()
        {
            //Arrange
            var requestText = "Please enter Test value";
            //Act
            ConsoleInteractionController.RequestInput(requestText);

            //Assert
            MockConsoleAdapter.Verify(m => m.WriteLine(requestText),Times.Once);
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Once);
        }
    }
}