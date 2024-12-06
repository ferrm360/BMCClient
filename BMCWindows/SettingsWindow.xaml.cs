using BMCWindows.Patterns.Singleton;
using BMCWindows.Validators;
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
using BMCWindows.Utilities;

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
            player = UserSessionManager.getInstance().GetPlayerUserData(); 
            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            var imageUrl = proxyProfile.GetProfileImage(player.Username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                MessageBox.Show("No image data returned.");
            }
            else
            {
                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(imageUrl.ImageData);
                if (image == null)
                {
                    MessageBox.Show("Image conversion failed.");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        imageUserProfilePicture.Source = image;
                    });

                }
            }

        }

        private void UpdatePassword(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
           
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            String username = player.Username;         
            String password = passwordBoxOldPassword.Password;
            String newPassword = passwordBoxNewPassword.Password;
            String confirmPassword = passwordBoxPassword.Password;
            if(!FieldValidator.AreFieldsEmpty(password, newPassword, confirmPassword) && FieldValidator.ValidatePassword(newPassword) && newPassword == confirmPassword)
            {
                var result = proxy.UpdatePassword(username, newPassword, password);
                if (result.IsSuccess)
                {
                    string message = Properties.Resources.PasswordUpdated;
                    MessageBox.Show(message);
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(result.ErrorKey);
                }

            } else
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("MessageBoxEmptyFields");
            }
            
            
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
