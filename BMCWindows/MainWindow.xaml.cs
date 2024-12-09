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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.NavigationService.Navigate(new StartPage());
            string savedLanguage = Properties.Settings.Default.languageCode;

            if (string.IsNullOrEmpty(savedLanguage))
            {
                savedLanguage = "en-US"; 
            }

            SetLanguage(savedLanguage);

        }

        private void MinimizeWindow(Object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.CanResize;
            }
        }

        private void CloseWindow(Object sender, EventArgs e)
        { 
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
        }



    }
}
