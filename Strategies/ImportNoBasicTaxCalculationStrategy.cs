using Domain;
using Domain.Enums;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Strategies;
using Microsoft.Extensions.Logging;

namespace Strategies
{
    public class ImportNoBasicTaxCalculationStrategy : TaxCalculationStrategyBase,  ITaxCalculationStrategy
    {
        public TaxType TaxType => TaxType.ImportNoBasic;

        public ImportNoBasicTaxCalculationStrategy(ILogger<ImportNoBasicTaxCalculationStrategy> logger, ISettingsHelper settingsHelper) 
            : base(logger, settingsHelper)
        { 
        }

        public decimal Calculate(decimal price)
        {
            Logger.LogInformation("Calulating import no basic tax");

            var importTaxValue = SettingsHelper.GetAppSettingValue<decimal>(Constants.ImportTax);

            return RoundToNearestFiveCents(price * importTaxValue) + price;
        }
    }
}
