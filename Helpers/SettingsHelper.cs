using Domain.Interfaces.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        private readonly ILogger<SettingsHelper> _logger;
        private readonly IConfiguration _configuration;

        public SettingsHelper(ILogger<SettingsHelper> logger, IConfiguration configuration)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public T GetAppSettingValue<T>(string settingName)
        {
            var valueString = _configuration[$"AppSettings:{settingName}"];

            if (string.IsNullOrWhiteSpace(valueString))
            {
                var errorMessage = $"Value of {settingName} setting could not be found and no default value was specified";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            return GetConvertedValue<T>(settingName, valueString);
        }

        private T GetConvertedValue<T>(string settingName, string valueString)
        {
            var valueType = typeof(T);
            try
            {
                return (T)Convert.ChangeType(valueString, valueType, CultureInfo.InvariantCulture);
            }
            catch (SystemException systemException) when (systemException is InvalidCastException || systemException is FormatException)
            {
                _logger.LogError("Value of {SettingName} setting could not be converted to type {ValueType}",
                    settingName, valueType);
                throw;
            }
        }
    }
}