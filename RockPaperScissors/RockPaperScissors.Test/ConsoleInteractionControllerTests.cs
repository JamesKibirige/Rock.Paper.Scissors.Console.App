using System.Text.RegularExpressions;
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

        [TestMethod]
        public void RequestValidInput_RequestValidInputFromUser_UserEntersValidInput()
        {
            //Arrange
            const string validationMessage = "Please Select Valid Input from Options\n\n{Test1,Test2,Test3}";
            const string validationExpression = "Test1,Test2,Test3";
            const string expectedresult = "test1";
            MockConsoleAdapter.Setup(m => m.ReadLine()).Returns("test1");
            ConsoleAdapter = MockConsoleAdapter.Object;

            //Act
            var result = ConsoleInteractionController.RequestValidInput(validationMessage, validationExpression);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedresult, result);
            MockConsoleAdapter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Once);
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Once);
        }

        [TestMethod]
        public void RequestValidInput_RequestValidInputFromUserUserEntersInvalidInput_LoopsUnitValidInputProvided()
        {
            //Arrange
            const string validationMessage = "Please Select Valid Input from Options\n\n{Test1,Test2,Test3}";
            const string validationExpression = "Test1,Test2,Test3";
            const string expectedresult = "test2";
            MockConsoleAdapter.SetupSequence(m => m.ReadLine()).Returns("test5").Returns("test2");
            ConsoleAdapter = MockConsoleAdapter.Object;

            //Act
            var result = ConsoleInteractionController.RequestValidInput(validationMessage, validationExpression);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedresult, result);
            MockConsoleAdapter.Verify(m =>  m.WriteLine(It.IsAny<string>()),Times.Exactly(2));
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Exactly(2));
        }

        [TestMethod]
        public void RequestValidInput_OverloadRegExRequestValidInputFromUser_UserEntersValidInput()
        {
            //Arrange
            const string validationMessage = "Please Enter Your Name:\n\n{A-Z,a-z,1-9, }";
            var rgx = new Regex(@"");
            const string expectedresult = "James Kibirige";
            MockConsoleAdapter.Setup(m => m.ReadLine()).Returns("James Kibirige");
            ConsoleAdapter = MockConsoleAdapter.Object;

            //Act
            var result = ConsoleInteractionController.RequestValidInput(validationMessage, rgx);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedresult, result);
            MockConsoleAdapter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Once);
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Once);
        }

        [DataTestMethod]
        [DataRow("+++++")]
        [DataRow("&%hqyrt$5")]
        [DataRow("Jamal&&&Edwards$$$")]
        public void RequestValidInput_OverloadRegExRequestValidInputFromUserUserEntersInvalidInput_LoopsUnitValidInputProvided(string input)
        {
            //Arrange
            const string validationMessage = "Please Enter Valid Name:\n\n{A-Z,a-z, }";
            var rgx = new Regex(@"^[\p{L} \.'\-]+$");
            const string expectedresult = "Jonita Laidley";

            MockConsoleAdapter.SetupSequence(m => m.ReadLine()).Returns(input).Returns("Jonita Laidley");
            ConsoleAdapter = MockConsoleAdapter.Object;

            //Act
            var result = ConsoleInteractionController.RequestValidInput(validationMessage, rgx);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedresult, result);
            MockConsoleAdapter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Exactly(2));
            MockConsoleAdapter.Verify(m => m.ReadLine(), Times.Exactly(2));
        }
    }
}