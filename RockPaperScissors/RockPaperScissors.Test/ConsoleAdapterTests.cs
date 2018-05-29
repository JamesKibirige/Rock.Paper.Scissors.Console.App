using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Test
{
    [TestClass]
    public class ConsoleAdapterTests
    {
        public IConsoleAdapter TheConsoleAdapter { get; set; }
        public Mock<IConsoleAdapter> Mock { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            Mock = new Mock<IConsoleAdapter>();//Initialise mock instance of the console adapter
            Mock.Setup(m => m.ReadLine()).Returns("test");
            Mock.Setup(m => m.Write("test message \n over two lines!"));
            Mock.Setup(m => m.WriteLine("test message"));
            Mock.Setup(m => m.WriteLine());

            TheConsoleAdapter = Mock.Object;//Assign the mock to our IConsoleAdapter instance

            //Reset the standard output for the Console for each test
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };

            Console.SetOut(standardOut);
        }

        [TestMethod]
        public void ReadLine_ReadsLineFromConsole_ReturnsAString()
        {
            //Arrange
            //Act
            var result = TheConsoleAdapter.ReadLine();
            string expected = "test";

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, expected.GetType());
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void Write_WritesStringToConsole_MethodInvoked()
        {
            //Arrange
            //Act
            TheConsoleAdapter.Write("test message \n over two lines!");

            //Assert
            Mock.Verify(m => m.Write("test message \n over two lines!"),Times.Once);
        }

        [TestMethod]
        public void WriteLine_WritesLineToConsole_MethodInvoked()
        {
            //Arrange
            //Act
            TheConsoleAdapter.WriteLine("test message");

            //Assert
            Mock.Verify(m => m.WriteLine("test message"));
        }

        [TestMethod]
        public void ReadLine_ReadLineFromInputStream_ReturnsString()
        {
            using (StringReader sr = new StringReader("Test Input"))
            {
                //Arrange
                IConsoleAdapter aConsoleAdapter = new ConsoleAdapter();

                //Act
                Console.SetIn(sr);
                var result = aConsoleAdapter.ReadLine();
                var expected = "Test Input";

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual<string>(result, expected);
            }
        }

        [TestMethod]
        public void Write_WritesStringToOutputStream_ExpectedOutput()
        {
            using (StringWriter sw = new StringWriter())
            {
                //Arrange
                IConsoleAdapter aConsoleAdapter = new ConsoleAdapter();
                Console.SetOut(sw);

                //Act
                aConsoleAdapter.Write("test message \n over two lines!");
                var expected = "test message \n over two lines!";
                var result = sw.ToString();

                //Assert
                Assert.AreEqual<string>(expected, result);
            }
        }

        [TestMethod]
        public void WriteLine_WritesLineToOutputStream_ExpectedOutput()
        {
            using (StringWriter sw = new StringWriter())
            {
                //Arrange
                IConsoleAdapter aConsoleAdapter = new ConsoleAdapter();
                Console.SetOut(sw);

                //Act
                aConsoleAdapter.WriteLine("Press any key to Continue!");
                var expected = string.Format("Press any key to Continue!{0}", Environment.NewLine);

                var result = sw.ToString();

                //Assert
                Assert.AreEqual<string>(expected, result);
            }
        }

        [TestMethod]
        public void WriteLine_WriteLineNoParameters_MethodInvoked()
        {
            //Arrange
            //Act
            TheConsoleAdapter.WriteLine();

            //Assert
            Mock.Verify(m => m.WriteLine());
        }

        [TestMethod]
        public void WriteLine_WriteLineNoParameters_WritesCarriageReturnToOutputStream()
        {
            using (StringWriter sw = new StringWriter())
            {
                //Arrange
                IConsoleAdapter aConsoleAdapter = new ConsoleAdapter();
                Console.SetOut(sw);

                //Act
                aConsoleAdapter.WriteLine();
                var expected = string.Format("{0}", Environment.NewLine);

                var result = sw.ToString();

                //Assert
                Assert.AreEqual<string>(expected, result);
            }
        }
    }
}
