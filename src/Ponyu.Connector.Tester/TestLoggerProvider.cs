using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Ponyu.Connector.Tester
{
    internal class TestLoggerProvider : ILoggerProvider
    {
        internal class TestLogger : ILogger
        {
            internal class FakeScope : IDisposable
            {
                public void Dispose()
                {
                    
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new FakeScope();
            }

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                TestContext.WriteLine(string.Format("{0}-{1}", logLevel, formatter(state, exception)));
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger();
        }

        public void Dispose()
        {
            
        }
    }
}
