using Domain.Interfaces.Storage;
using Helpers;
using Microsoft.Extensions.Logging;

namespace Storage.Repositories
{
    public abstract class RepositoryBase <T>
    {
        protected IConnectionFactory ConnectionFactory { get; }
        protected ILogger Logger { get; }

        public RepositoryBase(ILogger logger, IConnectionFactory connectionFactory)
        {
            Logger = logger.ThrowIfNull(nameof(logger));
            ConnectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
        }
    }
}
