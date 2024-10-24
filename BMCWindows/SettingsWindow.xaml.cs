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

namespace BMCWindows
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Page
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();

        }

        private void UpdatePassword(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
           
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            String username = player.Username;         
            String password = passwordBoxOldPassword.ToString();
            String newPassword = passwordBoxNewPassword.ToString();
            String confirmPassword = passwordBoxPassword.ToString();
            var result = proxy.UpdatePassword(username, password, newPassword);
            if (result.IsSuccess)
            {
                MessageBox.Show("Contraseña actualizada exitosamente");
            } else
            {
                MessageBox.Show(result.ErrorKey);
            }
            
        }
    }
}
