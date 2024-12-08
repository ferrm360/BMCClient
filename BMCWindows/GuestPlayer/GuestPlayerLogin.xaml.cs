using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace BMCWindows.GuestPlayer
{
    /// <summary>
    /// Interaction logic for GuestPlayerLogin.xaml
    /// </summary>
    public partial class GuestPlayerLogin : Page
    {
        public GuestPlayerLogin()
        {
            InitializeComponent();
        }

        private void LoginAsGuest(object sender, RoutedEventArgs e)
        {
            string username = textBoxUsername.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.InvalidUsername");
                return;
            }

            GuestPlayerServer.GuestPlayerServiceClient proxy = new GuestPlayerServer.GuestPlayerServiceClient();

            try
            {
               var result = proxy.RegisterGuestPlayer(username);
                if (result.IsSuccess) {
                    Server.PlayerDTO player = new Server.PlayerDTO
                    {
                        Username = username
                    };

                    UserSessionManager.getInstance().LoginPlayer(player, true);
                    this.NavigationService.Navigate(new GameOptionsWindow());

                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(result.ErrorKey);
                }
            }
            catch (EndpointNotFoundException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void CheckLimit(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int maxLength = int.Parse(textBox.Tag.ToString());

            if (textBox.Text.Length >= maxLength)
            {
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.IsReadOnly = false;
            }
        }
    }
}
