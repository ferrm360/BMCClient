using BMCWindows.Patterns.Singleton;
using BMCWindows.Properties;
using BMCWindows.Server;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page, ChatServer.IChatServiceCallback
    {


        public ObservableCollection<Friend> Friends { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public ChatService chatService = new ChatService();
        public ChatServer.ChatServiceClient proxy;



        public HomePage()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();
            generalMessages.ItemsSource = Messages; 
            //generalMessages.ItemsSource = Messages; 
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            InstanceContext context = new InstanceContext(this);
            proxy = new ChatServer.ChatServiceClient(context);
            proxy.RegisterUser(player.Username);
            labelUserName.Content = player.Username;
            LoadFriendList(player.Username);
            buttonOpenContextMenu.Visibility = Visibility.Visible;
            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            var imageUrl = proxyProfile.GetProfileImage(player.Username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                Console.WriteLine("No image data returned.");
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
                        imageProfilePicture.Source = image;
                    });

                }
            }

            //LoadImage(imageUrl.ImageData);

        }





        private void SendGeneralMessage(object sender, RoutedEventArgs e)
        {

            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();

            if (!string.IsNullOrEmpty(textboxGeneralChat.Text))
            {
                proxy.SendMessage(player.Username, textboxGeneralChat.Text);
                ReceiveMessage(textboxGeneralChat.Text);
                textboxGeneralChat.Clear();
                //LoadRecentMessages();
                // Actualizar los mensajes mostrados en la interfaz
            }
        }

        public void ReceiveMessage(string message)
        {
            Server.PlayerDTO player = UserSessionManager.getInstance().getPlayerUserData();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(new Message { Sender = player.Username, Messages = message });
            });

        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingsWindow());
        }

        public void OpenContextMenu(object sender, RoutedEventArgs e)
        {
            menuOptions.Visibility = menuOptions.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoToSearchWindow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SearchWindow());
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
                                BitmapImage image = ConvertByteArrayToImage(friendProfilePicture.ImageData);

                                return new Friend
                                {
                                    UserName = friendPlayer.Username,
                                    friendPicture = image,
                                };


                                
                            })
                        );
                        FriendsList.ItemsSource = friendsList;
                        Chat.ItemsSource = friendsList;
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

        private void GoToProfileWindow(object sender, RoutedEventArgs e) 
        {
            this.NavigationService.Navigate(new ProfileWindow());  
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            
            UserSessionManager.getInstance().logoutPlayer();   
            this.NavigationService.Navigate(new StartPage());

        }


        private void LoadImage(byte[] imageData)
        {
            
            BitmapImage image = ConvertByteArrayToImage(imageData);
            imageProfilePicture.Source = image;

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

        private void GoToFriendRequestsWindow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FriendRequestsWindow());    
        }

        private void SelectFriend(object sender, SelectionChangedEventArgs e)
        {
            var selectedFriend = (Friend)FriendsList.SelectedItem;

            if (selectedFriend != null)
            {
                OpenPlayerProfileWindow(selectedFriend.UserName);
                Console.WriteLine($"Username enviado: {selectedFriend.UserName}");
            }
        }

        private void OpenPlayerProfileWindow(string username)
        {
            PlayerProfileWindow playerProfileWindow = new PlayerProfileWindow(username);

            this.NavigationService.Navigate(new PlayerProfileWindow(username));
        }

        private void GoToGames(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GameOptionsWindow());
        }

    }





    public class Friend
    {
        public string UserName { get; set; }
        public DateTime lastVisit {  get; set; }
        public Byte[] profileImage { get; set; }
        public int requestId { get; set; }
        public BitmapImage friendPicture { get; set; }
    }


}

