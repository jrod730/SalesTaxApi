using Helpers;
using Microsoft.Extensions.Logging;

namespace Engines
{
    public class EngineBase <T>
    {
        protected ILogger Logger { get; }

        public EngineBase(ILogger logger)
        {
            Logger = logger.ThrowIfNull(nameof(logger));
        }
    }
}
