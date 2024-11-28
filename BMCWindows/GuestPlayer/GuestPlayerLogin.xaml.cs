using BMCWindows.Patterns.Singleton;
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
                MessageBox.Show("Por favor, ingresa un nombre de usuario.");
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
                    MessageBox.Show(result.ErrorKey);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Error en el servidor");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
