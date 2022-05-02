using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class GetDeliveryShiftsTester
    {
        [Test]
        public async Task TodayTest()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var todayShifts = await TestingSetup.Client.GetDeliveryShifts();
            Assert.GreaterOrEqual(todayShifts.Length, 1);
            Assert.AreEqual(today, todayShifts[0].Date);
        }

        [Test]
        public async Task YesterdayTest()
        {
            var yesterday = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)));

            var yesterdayShifts = await TestingSetup.Client.GetDeliveryShifts(yesterday);
            Assert.AreEqual(0, yesterdayShifts.Length);
        }
    }
}
