using BMCWindows.Patterns.Singleton;
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
    /// Lógica de interacción para ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Page
    {
        public ProfileWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Content = player.Username;
        }

        private void UploadProfilePicture(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Visibility = Visibility.Visible;
            textBoxUser.Visibility = Visibility.Visible;
            textBoxUser.Text = labelUser.ToString();

        }

        private void MakeUserEditable(object sender, RoutedEventArgs e) 
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Visibility = Visibility.Visible;
            textBoxUser.Visibility = Visibility.Visible;
            textBoxUser.Text = labelUser.ToString();
            buttonAccepNewUsername.Visibility = Visibility.Visible;
            buttonCancelNewUsername.Visibility = Visibility.Visible;
            buttonEditUsername.Visibility = Visibility.Hidden;
        }

        private void AcceptUsernameChange(object sender, RoutedEventArgs e) 
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            String newUsername = textBoxUser.Text;
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            var result = proxy.UpdateUsername(player.Username, newUsername);
            if (result.IsSuccess) 
            {
                labelUser.Content = player.Username;
                textBoxUser.Visibility= Visibility.Hidden;
                labelUser.Visibility= Visibility.Visible;
                MessageBox.Show("Nombre de usuario actualizado exitosamente");
                buttonAccepNewUsername.Visibility = Visibility.Hidden;
                buttonCancelNewUsername.Visibility = Visibility.Hidden;
                buttonEditUsername.Visibility = Visibility.Visible;
            }
        }

    }
}
