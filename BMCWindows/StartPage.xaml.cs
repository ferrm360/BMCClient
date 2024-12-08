using BMCWindows.GuestPlayer;
using BMCWindows.Laguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BMCWindows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            //LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

        private void GoToLogInPage(Object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new LogIn());
        }

        private void GoToSignUpPage(Object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new SignUpWindow());
        }

        private void LoginAsGuest(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GuestPlayerLogin());

        }


        private void ChangeLanguageToSpanish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "es-MX";
            Properties.Settings.Default.Save();
            SetLanguage("es-MX");
        }

        private void ChangeLanguageToEnglish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
            SetLanguage("en-US");
        }

        private void OpenContextMenu(object sender, RoutedEventArgs e)
{
            var button = sender as Button;
            var contextMenu = button?.ContextMenu;

           
            if (contextMenu != null)
            {
                contextMenu.PlacementTarget = button; 
                contextMenu.IsOpen = true; 
            }
        }

        private void SetLanguage(string languageCode)
        {
            var cultureInfo = new System.Globalization.CultureInfo(languageCode);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var currentPage = Application.Current.MainWindow?.Content as Page;
            if (currentPage != null)
            {
                currentPage.Language = System.Windows.Markup.XmlLanguage.GetLanguage(cultureInfo.Name);
            }
            this.NavigationService.Navigate(new StartPage());
        }



    }
}
