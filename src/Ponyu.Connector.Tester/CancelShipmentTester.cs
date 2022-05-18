using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class CancelShipmentTester
    {
        [Test]
        public async Task CreateShipmentAndCancel()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(100, 200);

            var shipment = await TestingSetup.Client.CreateShipment(
                $"CX{orderId}",
                new Requests.OrderInformation
                {
                    Note = "Test order",
                    PickupDueDate = localTimeOffset.Add(TimeSpan.FromMinutes(30)),
                    RequestedDeliveryRangeStartDate = localTimeOffset.Add(TimeSpan.FromMinutes(60)),
                    RequestedDeliveryRangeEndDate = localTimeOffset.Add(TimeSpan.FromMinutes(90)),
                },
                new Requests.ContactInformation
                {
                    Name = "Mario Rossi",
                    PhoneNumber = "+393331234567",
                    Address = "Piazza Antonio Meucci, 1A",
                    City = "Roma",
                    ProvinceCode = "RM",
                    Country = "Italia",
                    Postcode = "00146",
                    AdditionalInformation = "Suonare al campanello",
                },
                new Requests.ContactInformation
                {
                    Name = "Luigi Verdi",
                    PhoneNumber = "+393341234567",
                    Address = "Via Raffaello Giovagnoli, 35",
                    City = "Roma",
                    ProvinceCode = "RM",
                    Country = "Italia",
                    Postcode = "00152",
                    AdditionalInformation = "Consegnare in portineria"
                },
                new Requests.PaymentInformation
                {
                    CashPaymentOnDelivery = false,
                    DeliveryCharge = 2.9m,
                    Total = 35.9m,
                },
                orderId
            );

            await Task.Delay(500);

            await TestingSetup.Client.CancelShipment(shipment.OrderId);
        }
    }
}
