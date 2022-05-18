using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class FullFlowTester
    {
        [Test]
        public async Task TestOutsideOfTimeslot()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(1000, 10000);

            var timeslots = await TestingSetup.Client.GetNextPickupSlotsAsync(Coordinates.SanLorenzoRoma, DateOnly.FromDateTime(localTimeOffset.Date));
            var tooLateTimeSlots = (timeslots.TimeSlots.LastOrDefault()?.Pickup ?? localTimeOffset).AddMinutes(15);

            Assert.ThrowsAsync<ServiceException>(async () =>
            {
                await TestingSetup.Client.CreateShipment(
                    $"FT{orderId}",
                    new Requests.OrderInformation
                    {
                        Note = $"Flow test order {orderId}",
                        PickupDueDate = tooLateTimeSlots,
                        RequestedDeliveryRangeStartDate = tooLateTimeSlots.AddMinutes(15),
                        RequestedDeliveryRangeEndDate = tooLateTimeSlots.AddMinutes(30),
                    },
                    ShipmentInformation.Sender,
                    ShipmentInformation.Receiver,
                    new Requests.PaymentInformation
                    {
                        CashPaymentOnDelivery = false,
                        DeliveryCharge = 2.9m,
                        Total = 35.9m,
                    },
                    orderId
                );
            });
        }

        [Test]
        public async Task TestWithinTimeslot()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(1000, 10000);

            var timeslots = await TestingSetup.Client.GetNextPickupSlotsAsync(Coordinates.SanLorenzoRoma, DateOnly.FromDateTime(localTimeOffset.Date));
            Assert.GreaterOrEqual(timeslots.TimeSlots.Length, 2);

            var pickup = timeslots.TimeSlots[1].Pickup.AddMinutes(30);
            Assert.LessOrEqual(pickup, timeslots.TimeSlots.Last().Pickup);

            var response = await TestingSetup.Client.CreateShipment(
                $"FT{orderId}",
                new Requests.OrderInformation
                {
                    Note = $"Flow test order {orderId}",
                    PickupDueDate = pickup,
                    RequestedDeliveryRangeStartDate = pickup.AddMinutes(15),
                    RequestedDeliveryRangeEndDate = pickup.AddMinutes(30),
                },
                ShipmentInformation.Sender,
                ShipmentInformation.Receiver,
                new Requests.PaymentInformation
                {
                    CashPaymentOnDelivery = false,
                    DeliveryCharge = 2.9m,
                    Total = 35.9m,
                },
                orderId
            );
            Assert.AreEqual($"FT{orderId}", response.OrderId);
            Assert.GreaterOrEqual(response.PickupDueDate, pickup); // Pickup must be at pickup time or later
            Assert.LessOrEqual(response.PickupDueDate, pickup.AddHours(1)); // Pickup must be within one hour of requested time
        }
    }
}
