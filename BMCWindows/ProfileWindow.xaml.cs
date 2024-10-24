using BMCWindows.Patterns.Singleton;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                string urlImage = openFileDialog.FileName;
                imageUserProfile.Source = bitmap;
                byte[] byteImage = ConvertImageToByteArray(urlImage);
                ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
                var result = proxy.UpdateProfilePicture(player.Username, byteImage, urlImage);
                if(result.IsSuccess)
                {
                    MessageBox.Show("Imagen de perfil actualizada exitosamente");
                }
                else
                {
                    MessageBox.Show(result.ErrorKey);
                }


            
            }

        }

        private byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageBytes = null;

            try
            {
                imageBytes = File.ReadAllBytes(imagePath); // Leer la imagen como arreglo de bytes
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer la imagen: " + ex.Message);
            }

            return imageBytes;
        }

        private void MakeUserEditable(object sender, RoutedEventArgs e) 
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Visibility = Visibility.Hidden;
            textBoxUser.Visibility = Visibility.Visible;
            textBoxUser.Text = player.Username;
            buttonAccepNewUsername.Visibility = Visibility.Visible;
            imageButtonAcceptUsernameEdition.Visibility = Visibility.Visible;
            buttonCancelNewUsername.Visibility = Visibility.Visible;
            textBlockAcceptEdition.Visibility = Visibility.Visible;
            imageButtonCancelUsernameEdition.Visibility = Visibility.Visible;
            textBlockCancelUserNameEdition.Visibility = Visibility.Visible;
            buttonEditUsername.Visibility = Visibility.Hidden;
            imageButtonEditUsername.Visibility = Visibility.Hidden;
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
                imageButtonAcceptUsernameEdition.Visibility = Visibility.Hidden;
                textBlockAcceptEdition.Visibility = Visibility.Hidden;
                buttonCancelNewUsername.Visibility = Visibility.Hidden;
                textBlockCancelUserNameEdition.Visibility = Visibility.Hidden;
                imageButtonCancelUsernameEdition.Visibility = Visibility.Hidden;
                buttonEditUsername.Visibility = Visibility.Visible;
                imageButtonEditUsername.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(result.ErrorKey);
            }
        }

        private void CancelUsernameEdition(object sender, RoutedEventArgs e) 
        {
            buttonAccepNewUsername.Visibility = Visibility.Hidden;
            textBlockAcceptEdition.Visibility = Visibility.Hidden;
            buttonCancelNewUsername.Visibility = Visibility.Hidden;
            textBlockCancelUserNameEdition.Visibility = Visibility.Hidden;
            buttonEditUsername.Visibility = Visibility.Visible;
        }


    }
}
