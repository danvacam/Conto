using Conto.Data;

namespace Conto.Wpf.Settings
{
    public class SettingsObjectDataProvider
    {
        private readonly ContoData _dataAccessLayer;

        public SettingsObjectDataProvider()
        {
            _dataAccessLayer = new ContoData();
        }

        public SettingUIObject GetSettings()
        {
            CommonDataObject settings = _dataAccessLayer.GetCommonData();
            return new SettingUIObject(settings);
        }
    }
}
