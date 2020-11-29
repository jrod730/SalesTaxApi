using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SalesTaxApi.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase<T>: ControllerBase
    {
        protected ILogger Logger { get; }

        public ApiControllerBase(ILogger logger)
        {
            Logger = logger.ThrowIfNull(nameof(logger));
        }
    }
}
