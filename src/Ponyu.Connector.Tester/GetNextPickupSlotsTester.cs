using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class GetNextPickupSlotsTester
    {
        [Test]
        public async Task RomeTest()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var sanLorenzoResults = await TestingSetup.Client.GetNextPickupSlotsAsync(Coordinates.SanLorenzoRoma, DateOnly.FromDateTime(localTimeOffset.Date));
            Assert.AreEqual("Roma", sanLorenzoResults.ZoneName);
            Assert.GreaterOrEqual(sanLorenzoResults.NextAvailablePickup, localTimeOffset);
            Assert.GreaterOrEqual(sanLorenzoResults.NextAvailableDeliveryStart, sanLorenzoResults.NextAvailablePickup);
            Assert.GreaterOrEqual(sanLorenzoResults.NextAvailableDeliveryEnd, sanLorenzoResults.NextAvailableDeliveryStart);
            Assert.GreaterOrEqual(sanLorenzoResults.TimeSlots.Length, 1);

            Assert.ThrowsAsync<ServiceException>(async () =>
            {
                await TestingSetup.Client.GetNextPickupSlotsAsync(Coordinates.Napoli);
            });
        }
    }
}