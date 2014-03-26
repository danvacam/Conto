using System.Windows;

namespace Conto.Wpf.Resources
{
    public static class AppProperties
    {
        public static bool FormHaveModifications
        {
            get
            {
                if (Application.Current.Properties["FormHaveModifications"] == null)
                    Application.Current.Properties["FormHaveModifications"] = false;
                return (bool)Application.Current.Properties["FormHaveModifications"];
            }
            set { Application.Current.Properties["FormHaveModifications"] = value; }
        }

        public static string FirstInfo
        {
            get
            {
                if (Application.Current.Properties["FirstInfo"] == null)
                    Application.Current.Properties["FirstInfo"] = string.Empty;
                return Application.Current.Properties["FirstInfo"].ToString();
            }
            set { Application.Current.Properties["FirstInfo"] = value; }
        }

        public static string SecondInfo
        {
            get
            {
                if (Application.Current.Properties["FirstInfo"] == null)
                    Application.Current.Properties["FirstInfo"] = string.Empty;
                return Application.Current.Properties["FirstInfo"].ToString();
            }
            set { Application.Current.Properties["FirstInfo"] = value; }
        }
    }
}
