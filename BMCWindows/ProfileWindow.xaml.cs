using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Lógica de interacción para ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Page
    {
        public ProfileWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            labelUser.Content = player.Username;
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            LoadFriendList(player.Username);
            string bio = proxy.GetBioByUsername(player.Username);
            textBlockBio.Text = bio;

            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            var imageUrl = proxyProfile.GetProfileImage(player.Username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.EmptyImage");
            }
            else
            {
                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(imageUrl.ImageData);
                if (image == null)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.ImageNotFound");    
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProfileImageBrush.ImageSource = image;
                    });

                }

            }
        }
        private void UploadProfilePicture(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                string urlImage = openFileDialog.FileName;
                ProfileImageBrush.ImageSource = bitmap;
                byte[] byteImage = ConvertImageToByteArray(urlImage);
                ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
                var result = proxy.UpdateProfilePicture(player.Username, byteImage, urlImage);
                if(result.IsSuccess)
                {
                    string message = Properties.Resources.Info_ProfileImageUpdated;
                    MessageBox.Show(message);
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
                imageBytes = File.ReadAllBytes(imagePath); 
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ImageNotFound");
            }

            return imageBytes;
        }

        private void MakeUserEditable(object sender, RoutedEventArgs e) 
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
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
            player = UserSessionManager.getInstance().GetPlayerUserData();
            String newUsername = textBoxUser.Text;
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            var result = proxy.UpdateUsername(player.Username, newUsername);
            if (result.IsSuccess) 
            {
                Server.PlayerDTO playerUpdated = new Server.PlayerDTO();
                playerUpdated = UserSessionManager.getInstance().GetPlayerUserData();
                labelUser.Content = newUsername;
                textBoxUser.Visibility= Visibility.Hidden;
                labelUser.Visibility= Visibility.Visible;
                string message = Properties.Resources.Info_UserUpdated.ToString();
                MessageBox.Show(message);
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
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage(result.ErrorKey);
            }
        }

        private void CancelUsernameEdition(object sender, RoutedEventArgs e) 
        {
            buttonAccepNewUsername.Visibility = Visibility.Hidden;
            textBlockAcceptEdition.Visibility = Visibility.Hidden;
            buttonCancelNewUsername.Visibility = Visibility.Hidden;
            textBlockCancelUserNameEdition.Visibility = Visibility.Hidden;
            buttonEditUsername.Visibility = Visibility.Visible;
            labelUser.Visibility = Visibility.Visible;
            textBoxUser.Visibility = Visibility.Hidden;
        }

        private void MakeBioEditable(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            textBlockBio.Visibility = Visibility.Hidden;
            textBoxBio.Visibility = Visibility.Visible;
            textBoxBio.Text = textBlockBio.Text;
            buttonAcceptNewBio.Visibility = Visibility.Visible;
            imageAcceptBio.Visibility = Visibility.Visible;
            textBlockAcceptBio.Visibility = Visibility.Visible;
            buttonCancelNewBio.Visibility = Visibility.Visible;
            imageCancelBio.Visibility = Visibility.Visible;
            textBlockCancelBio.Visibility = Visibility.Visible;
            buttonChangeBio.Visibility = Visibility.Hidden;
        }

        private void AcceptBioUpdate(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            String newBio = textBoxBio.Text;
            ProfileServer.ProfileServiceClient proxy = new ProfileServer.ProfileServiceClient();
            var result = proxy.UpdateBio(newBio, player.Username);
            if (result.IsSuccess)
            {
                Server.PlayerDTO playerUpdated = new Server.PlayerDTO();
                playerUpdated = UserSessionManager.getInstance().GetPlayerUserData();
                textBoxBio.Visibility = Visibility.Hidden;
                textBlockBio.Visibility = Visibility.Visible;
                textBlockBio.Text = newBio;
                string message = Properties.Resources.Info_BioUpdated.ToString();
                MessageBox.Show(message);
                buttonAcceptNewBio.Visibility = Visibility.Hidden;
                imageAcceptBio.Visibility = Visibility.Hidden;
                textBlockAcceptBio.Visibility = Visibility.Hidden;
                buttonCancelNewBio.Visibility = Visibility.Visible;
                imageCancelBio.Visibility = Visibility.Hidden;
                textBlockCancelBio.Visibility = Visibility.Hidden;
                buttonChangeBio.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(result.ErrorKey);
            }
        }

        private void CancelBioUpdate(object sender, RoutedEventArgs e)
        {
            textBoxBio.Visibility = Visibility.Hidden;
            textBlockBio.Visibility = Visibility.Visible;
            buttonAcceptNewBio.Visibility = Visibility.Hidden;
            imageAcceptBio.Visibility = Visibility.Hidden;
            textBlockAcceptBio.Visibility = Visibility.Hidden;
            buttonCancelNewBio.Visibility = Visibility.Visible;
            imageCancelBio.Visibility = Visibility.Hidden;
            textBlockCancelBio.Visibility = Visibility.Hidden;
            buttonChangeBio.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void LoadFriendList(string username)
        {
            try
            {
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetFriendList(username);

                ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();

                if (response.IsSuccess)
                {
                    if (response.Friends != null && response.Friends.Any())
                    {
                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.Friends.Select(friendPlayer =>
                            {
                                var friendProfilePicture = profileProxy.GetProfileImage(friendPlayer.Username);
                                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(friendProfilePicture.ImageData);

                                return new Friend
                                {
                                    UserName = friendPlayer.Username,
                                    FriendPicture = image,
                                };



                            })
                        );
                        FriendsList.ItemsSource = friendsList;
                        
                    }
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(response.ErrorKey);
                }
            }
            catch (CommunicationException commEx)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException timeoutEx)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
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
