using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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
            var consoleLoggerProvider = new ConsoleLoggerProvider(
                new OptionsMonitor<ConsoleLoggerOptions>(new ConsoleLoggerOptions())
            );
            var loggerFactory = new LoggerFactory(new ILoggerProvider[] { consoleLoggerProvider });

            Client = new Client(new Settings(Environment.GetEnvironmentVariable("API_KEY"))
            {
                BaseUrl = Environment.GetEnvironmentVariable("PONYU_BASE_URL"),
            }, loggerFactory.CreateLogger<Client>());
        }
    }
}
