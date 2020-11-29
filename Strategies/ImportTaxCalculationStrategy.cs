using Domain;
using Domain.Enums;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Strategies;
using Microsoft.Extensions.Logging;

namespace Strategies
{
    public class ImportTaxCalculationStrategy : TaxCalculationStrategyBase,  ITaxCalculationStrategy
    {
        public TaxType TaxType => TaxType.Import;

        public ImportTaxCalculationStrategy(ILogger<ImportTaxCalculationStrategy> logger, ISettingsHelper settingsHelper) 
            : base(logger, settingsHelper)
        { 
        }

        public decimal Calculate(decimal price)
        {
            Logger.LogInformation("Calulating import tax");

            var importTaxValue = SettingsHelper.GetAppSettingValue<decimal>(Constants.ImportTax);
            var basicTaxValue = SettingsHelper.GetAppSettingValue<decimal>(Constants.BasicTax);

            return RoundToNearestFiveCents(price * basicTaxValue) + RoundToNearestFiveCents(price * importTaxValue) + price;
        }
    }
}
