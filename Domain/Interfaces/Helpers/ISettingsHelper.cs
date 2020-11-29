namespace Domain.Interfaces.Helpers
{
    public interface ISettingsHelper
    {
        T GetAppSettingValue<T>(string settingName);
    }
}