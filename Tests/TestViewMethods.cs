using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using Xunit;
using Moq;
using App;

namespace Tests
{
    public class TestViewMethods
    {
        [Fact]
        public void ShouldReturnEnumStartNew_AskForAction()
        {
            var testConsole = new TestConsole("p", new [] { "" });
            View sut = new View(testConsole, null);
            Assert.Equal(View.StartMenuAction.StartNew, sut.AskForAction());
        }


        [Fact]
        public void ShouldReturnEnumExit_AskForAction()
        {
            var testConsole = new TestConsole("e", new [] { "" });
            View sut = new View(testConsole, null);
            Assert.Equal(View.StartMenuAction.Exit, sut.AskForAction());
        }


		[Fact]
        public void ShouldPrintErrorMessage_AskForAction()
        {
            var testConsole = new TestConsole("jhkjh", new [] { "" });
            var expected = "Invalid choice, try again.";

            View sut = new View(testConsole, null);
            sut.AskForAction();
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }

        [Fact]
        public void ShouldPrintMenuMessage_ShowMenu()
        {
            var testConsole = new TestConsole("t", new [] { "t" });
            var expected = "Hello, Would you like to start a New Game?\n 'p' = Play\n 'e' = Exit";

            View sut = new View(testConsole, null);
            sut.ShowMenu();
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }

        [Fact]
        public void ShouldPrintStartGuessingMessage_ShowStartGuessingMessage()
        {
            var testConsole = new TestConsole("t", new [] { "t" });
            var expected = "Enter a number between 1 & 100..\n";

            View sut = new View(testConsole, null);
            sut.ShowStartGuessingMessage();
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }

        [Fact]
        public void ShouldReturnInterger_GetGuessedNumber()
        {
            var testConsole = new TestConsole("", new [] { "1" });
            var expected = 1;

            View sut = new View(testConsole, null);
            Assert.Equal(expected, sut.GetGuessedNumber());
        }
		
        [Theory]
        [InlineData(3, 3, 10)]
        [InlineData(-27, -27, 10)]
        public void ShouldPrintWinnerMessage_ShowGameOutcome(int x, int y, int z)
        {
            var mockModel = new Mock<GuessModel>(x);
            mockModel.Setup(m => m.HasWon()).Returns(true);
            var testConsole = new TestConsole("t", new [] { "test" });
            var expected = $"Congrats!! You guessed it!! The right answer is {x}.";

            View sut = new View(testConsole, mockModel.Object);
            sut.ShowGameOutcome(x, y, z);
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }


		[Theory]
        [InlineData(46, 66, 10)]
        [InlineData(-27, 7, 10)]
        [InlineData(-1, 100, 10)]
        public void ShouldPrintGuessTooHighMessage_ShowGameOutcome(int x, int y, int z)
        {
            var mockModel = new Mock<GuessModel>(x);
            mockModel.Setup(m => m.IsTooHigh(y)).Returns(true);
            var testConsole = new TestConsole("test", new [] { "test" });
            var expected = $"Your guess is Too High. Guesses left: ({z})";

            View sut = new View(testConsole, mockModel.Object);
            sut.ShowGameOutcome(x, y, z);
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }


        [Theory]
        [InlineData(46, 1, 10)]
        [InlineData(-27, -44, 10)]
        [InlineData(12, -6, 10)]
        public void ShouldPrintGuessTooLowMessage_ShowGameOutcome(int x, int y, int z)
        {
            var mockModel = new Mock<GuessModel>(x);
            var testConsole = new TestConsole("t", new [] { "test" });
            var expected = $"Your guess is Too Low. Guesses left: ({z})";

            View sut = new View(testConsole, mockModel.Object);
            sut.ShowGameOutcome(x, y, z);
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }


		[Theory]
        [InlineData(12, -6, 0)]
        public void ShouldPrintOutOfGuessesMessage_ShowGameOutcome(int x, int y, int z)
        {
            var mockModel = new Mock<GuessModel>(x);
			mockModel.Setup(m => m.GetRemainingGuesses()).Returns(z);
            var testConsole = new TestConsole("t", new [] { "test" });
            var expected = $"You Lost, this time!!";

            View sut = new View(testConsole, mockModel.Object);
            sut.ShowGameOutcome(x, y, z);
            Assert.EndsWith(expected + "\n", testConsole.GetOutput());
        }
    }
}