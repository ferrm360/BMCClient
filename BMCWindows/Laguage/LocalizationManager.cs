using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMCWindows.Laguage
{
    public static class LocalizationManager
    {
        public static event EventHandler LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            Properties.Settings.Default.languageCode = languageCode;
            Properties.Settings.Default.Save();

            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

