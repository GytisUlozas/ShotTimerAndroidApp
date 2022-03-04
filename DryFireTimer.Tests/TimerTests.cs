using DryFireTimer.Models;
using Xunit;

namespace DryFireTimer.Tests
{
    public class TimerTests
    {
        [Fact]
        public void TimerToTime1()
        {
            //Arrange
            string str = " 5. 0; ";

            //Act
            bool tempbool = Timer.ToTime(str, out _);

            //Assert
            Assert.False(tempbool);
        }

        [Fact]
        public void TimerToTime2()
        {
            //Arrange
            string str = "15.0127";

            //Act
            bool tempbool = Timer.ToTime(str, out int time);

            //Assert
            Assert.True(tempbool);
            Assert.Equal(150, time);
        }

        [Fact]
        public void TimerToTime3()
        {
            //Arrange
            string str = "5";

            //Act
            bool tempbool = Timer.ToTime(str, out int time);

            //Assert
            Assert.True(tempbool);
            Assert.Equal(50, time);
        }
    }
}