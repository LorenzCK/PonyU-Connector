using Ponyu.Connector.Requests;

namespace Ponyu.Connector.Tester
{
    internal static class ShipmentInformation
    {
        public static readonly ContactInformation Sender = new()
        {
            Name = "Mario Rossi",
            PhoneNumber = "+393331234567",
            Address = "Piazza Antonio Meucci, 1A",
            City = "Roma",
            ProvinceCode = "RM",
            Country = "Italia",
            Postcode = "00146",
            AdditionalInformation = "Suonare al campanello",
        };

        public static readonly ContactInformation Receiver = new()
        {
            Name = "Luigi Verdi",
            PhoneNumber = "+393341234567",
            Address = "Via Raffaello Giovagnoli, 35",
            City = "Roma",
            ProvinceCode = "RM",
            Country = "Italia",
            Postcode = "00152",
            AdditionalInformation = "Consegnare in portineria"
        };
    }
}
