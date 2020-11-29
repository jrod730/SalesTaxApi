using Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace UnitTests
{
    public abstract class TestBase<T>
    {
        protected Mock<AbstractLogger<T>> LoggerMock { get; private set; }

        public void TestBaseInit()
        {
            LoggerMock = new Mock<AbstractLogger<T>>();
        }

        public void TestBaseCleanup()
        {
            LoggerMock.VerifyAll();
        }
    }

    public abstract class AbstractLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            formatter.ThrowIfNull(nameof(formatter));
            Log(logLevel, exception, formatter(state, exception));
        }

        public abstract void Log(LogLevel logLevel, Exception ex, string information);
    }
}
