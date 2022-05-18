using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class CreateShipmentTester
    {
        [Test]
        public async Task CreateSimpleShipmentWithinRome()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(100, 200);

            await TestingSetup.Client.CreateShipment(
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
        }

        [Test]
        public async Task CreateSimpleShipmentWithinRomeAndTrack()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(100, 200);
            var orderCode = $"CX{orderId}";

            var shipment = await TestingSetup.Client.CreateShipment(
                orderCode,
                new Requests.OrderInformation
                {
                    Note = "Test order",
                    PickupDueDate = localTimeOffset.Add(TimeSpan.FromHours(4)),
                    RequestedDeliveryRangeStartDate = localTimeOffset.Add(TimeSpan.FromHours(4)).AddMinutes(10),
                    RequestedDeliveryRangeEndDate = localTimeOffset.Add(TimeSpan.FromHours(5)),
                },
                new Requests.ContactInformation
                {
                    Name = "Mario Spedizionieri",
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
                    Name = "Luigi Mangioni",
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

            if(!string.IsNullOrWhiteSpace(shipment.TrackingCode))
            {
                var tracking = await TestingSetup.Client.GetTrackingInformation(shipment.TrackingCode);
                Assert.AreEqual(orderCode, tracking.OrderId);
                Assert.AreEqual(shipment.TrackingCode, tracking.TrackingCode);
                Assert.AreEqual(ShipmentStatus.Requested, tracking.ShipmentStatus);
            }
        }

        [Test]
        public void CreateSimpleShipmentInThePast()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(100, 200);

            Assert.ThrowsAsync<ServiceException>(async () =>
            {
                await TestingSetup.Client.CreateShipment(
                    $"CX{orderId}",
                    new Requests.OrderInformation
                    {
                        PickupDueDate = localTimeOffset.Subtract(TimeSpan.FromMinutes(30)),
                        RequestedDeliveryRangeStartDate = localTimeOffset.Subtract(TimeSpan.FromMinutes(60)),
                        RequestedDeliveryRangeEndDate = localTimeOffset.Subtract(TimeSpan.FromMinutes(90)),
                    },
                    new Requests.ContactInformation
                    {
                        Name = "Mario Spedizionieri",
                        PhoneNumber = "+393331234567",
                        Address = "Piazza Antonio Meucci, 1A",
                        City = "Roma",
                        ProvinceCode = "RM",
                        Country = "Italia",
                        Postcode = "00146",
                    },
                    new Requests.ContactInformation
                    {
                        Name = "Luigi Mangioni",
                        PhoneNumber = "+393341234567",
                        Address = "Via Raffaello Giovagnoli, 35",
                        City = "Roma",
                        ProvinceCode = "RM",
                        Country = "Italia",
                        Postcode = "00152",
                    },
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
        public async Task CreateSimpleAsapShipmentWithinRome()
        {
            var tzRome = TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzRome);
            var localTimeOffset = new DateTimeOffset(localTime, tzRome.GetUtcOffset(DateTime.UtcNow));

            var rnd = new Random();
            var orderId = rnd.Next(200, 300);

            await TestingSetup.Client.CreateShipment(
                $"CX{orderId}",
                new Requests.OrderInformation
                {
                    Note = "Test ASAP order",
                    PickupDueDate = localTimeOffset,
                    RequestedDeliveryRangeStartDate = localTimeOffset.Add(TimeSpan.FromMinutes(5)),
                    RequestedDeliveryRangeEndDate = localTimeOffset.Add(TimeSpan.FromMinutes(15)),
                    DeliveryAsap = true,
                },
                new Requests.ContactInformation
                {
                    Name = "Mario Spedizionieri",
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
                    Name = "Luigi Mangioni",
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
        }
    }
}
