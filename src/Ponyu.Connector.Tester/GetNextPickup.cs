using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class GetNextPickup
    {
        [Test]
        public async Task RomeTest()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var vaticanCity = await TestingSetup.Client.GetNextPickupAsync(new Coordinate(41.902108, 12.457250));
            Assert.AreEqual("Roma", vaticanCity.ZoneName);
            Assert.GreaterOrEqual(vaticanCity.NextPickup, localTimeOffset);

            Assert.ThrowsAsync<ServiceException>(async () =>
            {
                await TestingSetup.Client.GetNextPickupAsync(new Coordinate(41.809623, 12.682193));
            });
        }
    }
}
