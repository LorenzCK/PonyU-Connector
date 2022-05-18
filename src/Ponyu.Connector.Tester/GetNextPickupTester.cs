using NUnit.Framework;

#pragma warning disable CS0612 // Type or member is obsolete

namespace Ponyu.Connector.Tester
{
    internal class GetNextPickupTester
    {
        [Test]
        public async Task RomeTest()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var sanLorenzoResult = await TestingSetup.Client.GetNextPickupAsync(Coordinates.SanLorenzoRoma);
            Assert.AreEqual("Roma", sanLorenzoResult.ZoneName);
            Assert.GreaterOrEqual(sanLorenzoResult.NextPickup, localTimeOffset);

            Assert.ThrowsAsync<ServiceException>(async () =>
            {
                await TestingSetup.Client.GetNextPickupAsync(Coordinates.Frascati);
            });
        }
    }
}

#pragma warning restore CS0612 // Type or member is obsolete

