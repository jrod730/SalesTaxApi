using Domain.Enums;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Strategies;
using Helpers;
using Microsoft.Extensions.Logging;

namespace Strategies
{
    public class NoTaxCalculationStrategy : TaxCalculationStrategyBase, ITaxCalculationStrategy
    {
        public TaxType TaxType => TaxType.None;

        public NoTaxCalculationStrategy(ILogger<NoTaxCalculationStrategy> logger, ISettingsHelper settingsHelper) 
            : base(logger, settingsHelper)
        { 
        }

        public decimal Calculate(decimal price)
        {
            Logger.LogInformation("Calculating no tax");
            return price;
        }
    }
}
