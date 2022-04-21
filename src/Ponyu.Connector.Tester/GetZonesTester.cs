using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class GetZonesTester
    {
        [Test]
        public async Task RomeTest()
        {
            var vaticanCity = await TestingSetup.Client.GetZonesAsync(new Coordinate(41.902108, 12.457250));
            Assert.AreEqual(1, vaticanCity.Length);
            Assert.AreEqual(343, vaticanCity[0].Id);
            Assert.AreEqual("Roma", vaticanCity[0].Name);

            var outsideGra = await TestingSetup.Client.GetZonesAsync(new Coordinate(41.809623, 12.682193));
            Assert.AreEqual(0, outsideGra.Length);

            var jonio = await TestingSetup.Client.GetZonesAsync(new Coordinate(41.946737, 12.527324));
            Assert.AreEqual(1, jonio.Length);
            Assert.AreEqual(343, jonio[0].Id);
            Assert.AreEqual("Roma", jonio[0].Name);
        }
    }
}
