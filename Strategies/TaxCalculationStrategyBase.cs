using Domain.Interfaces.Helpers;
using Helpers;
using Microsoft.Extensions.Logging;
using System;

namespace Strategies
{
    public abstract class TaxCalculationStrategyBase
    {
        protected readonly ILogger Logger;
        protected readonly ISettingsHelper SettingsHelper;

        public TaxCalculationStrategyBase(ILogger logger, ISettingsHelper settingsHelper)
        {
            Logger = logger.ThrowIfNull(nameof(logger));
            SettingsHelper = settingsHelper.ThrowIfNull(nameof(settingsHelper));
        }

        protected decimal RoundToNearestFiveCents(decimal taxPrice)
        {
            return Math.Ceiling(taxPrice / .05m) * .05m;
        }
    }
}
