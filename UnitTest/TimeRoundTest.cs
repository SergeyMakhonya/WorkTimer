using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkTimer;

namespace UnitTest
{
    [TestClass]
    public class TimeRoundTest
    {
        [TestMethod]
        public void Round()
        {
            TestCase(10, 1, 4, 1, 0);
            TestCase(10, 1, 5, 1, 0);
            TestCase(10, 0, 47, 0, 50);
            TestCase(10, 1, 6, 1, 10);
            TestCase(10, 0, 1, 0, 0);
            TestCase(10, 0, 2, 0, 0);
            TestCase(10, 0, 5, 0, 0);
            TestCase(10, 0, 6, 0, 10);
        }

        void TestCase(int round, int hours1, int minutes1, int hours2, int minutes2)
        {
            /*var time = new Time
            {
                Hours = hours1,
                Minutes = minutes1
            };

            var roundedTime = time.Round(round);

            Assert.AreEqual(roundedTime.Hours, hours2);
            Assert.AreEqual(roundedTime.Minutes, minutes2);*/
        }
    }
}
