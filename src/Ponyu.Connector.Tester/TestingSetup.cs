using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    [SetUpFixture]
    internal class TestingSetup
    {
        public static Client Client { get; private set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var testLoggerProvider = new TestLoggerProvider();
            var loggerFactory = new LoggerFactory(new ILoggerProvider[] { testLoggerProvider });

            Client = new Client(new Settings(Environment.GetEnvironmentVariable("API_KEY"))
            {
                BaseUrl = Environment.GetEnvironmentVariable("PONYU_BASE_URL"),
            }, loggerFactory.CreateLogger<Client>());
        }
    }
}
