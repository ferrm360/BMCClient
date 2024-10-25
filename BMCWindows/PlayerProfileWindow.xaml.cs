using BMCWindows.Patterns.Singleton;
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
    /// Lógica de interacción para PlayerProfileWindow.xaml
    /// </summary>
    public partial class PlayerProfileWindow : Page
    {
        private string _username;
        public PlayerProfileWindow(string username)
        {
            InitializeComponent();
            _username = username;
            Console.WriteLine($"Username enviado: {_username}");
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Content = _username;
            LoadFriendList(_username);
            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            string bio = proxyProfile.GetBioByUsername(_username);
            textBlockBio.Text = bio;
            var imageUrl = proxyProfile.GetProfileImage(_username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                MessageBox.Show("No image data returned.");
            }
            else
            {
                BitmapImage image = ConvertByteArrayToImage(imageUrl.ImageData);
                if (image == null)
                {
                    MessageBox.Show("Image conversion failed.");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        imagePlayerProfilePicture.Source = image;
                    });

                }
            }

            var profile = proxyProfile.GetProfileByUsername(_username);
            //textBlockBio.Text = profile.Profile.Bio;

        }

        private void LoadFriendList(string username)
        {
            try
            {
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetFriendList(username);

                if (response.IsSuccess)
                {
                    if (response.Friends != null && response.Friends.Any())
                    {
                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.Friends.Select(friendPlayer => new Friend
                            {
                                UserName = friendPlayer.Username,
                            })
                        );
                        FriendsList.ItemsSource = friendsList;
                        
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response?.ErrorKey ?? "Unknown error"}");
                }
            }
            catch (CommunicationException commEx)
            {
                MessageBox.Show($"Communication error: {commEx.Message}");
            }
            catch (TimeoutException timeoutEx)
            {
                MessageBox.Show($"Timeout error: {timeoutEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}");
            }
        }

        private void LoadImage(byte[] imageData)
        {

            BitmapImage image = ConvertByteArrayToImage(imageData);
            imagePlayerProfilePicture.Source = image;

        }

        // TODO: Pasar a utilities
        public BitmapImage ConvertByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }
}
