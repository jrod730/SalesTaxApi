using Domain;
using Domain.Enums;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Strategies;
using Microsoft.Extensions.Logging;

namespace Strategies
{
    public class BasicTaxCalculationStratgey : TaxCalculationStrategyBase, ITaxCalculationStrategy
    {
        public TaxType TaxType => TaxType.Basic;

        public BasicTaxCalculationStratgey(ILogger<BasicTaxCalculationStratgey> logger, ISettingsHelper settingsHelper) 
            : base(logger, settingsHelper)
        {    
        }

        public decimal Calculate(decimal price)
        {
            Logger.LogInformation("Calulating basic tax");

            var taxValue = SettingsHelper.GetAppSettingValue<decimal>(Constants.BasicTax);

            return RoundToNearestFiveCents(price * taxValue)+ price;
        }
    }
}
