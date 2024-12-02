using BMCWindows.Patterns.Singleton;
using BMCWindows.Properties;
using BMCWindows.Server;
using BMCWindows.Utilities;
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
        public ObservableCollection<Message> FriendChatMessages { get; set; } = new ObservableCollection<Message>();
        private FriendChatCallBackHandler _friendChatCallbackHandler; 


        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            _friendChatCallbackHandler = new FriendChatCallBackHandler();
            _friendChatCallbackHandler.FriendMessageReceived += MessageReceived;
            Messages = new ObservableCollection<Message>();
            generalMessages.ItemsSource = Messages; 
            ChatMessages.ItemsSource = FriendChatMessages;
            //generalMessages.ItemsSource = Messages; 
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            InstanceContext context = new InstanceContext(this);
            _proxy = new ChatServer.ChatServiceClient(context);
            _proxy.RegisterUser(player.Username);
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
                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(imageUrl.ImageData);
                if (image == null)
                {
                    MessageBox.Show("Image conversion failed.");
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

        private void SendGeneralMessage(object sender, RoutedEventArgs e)
        {

            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();

            if (!string.IsNullOrEmpty(textboxGeneralChat.Text))
            {
                var formattedMessage = $"{player.Username}: {textboxGeneralChat.Text}";
                _proxy.SendMessage(player.Username, formattedMessage);
                ReceiveMessage(textboxGeneralChat.Text);
                textboxGeneralChat.Clear();
            }
        }

        public void ReceiveMessage(string message)
        {
            Server.PlayerDTO player = UserSessionManager.getInstance().GetPlayerUserData();

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
                                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(friendProfilePicture.ImageData);

                                return new Friend
                                {
                                    UserName = friendPlayer.Username,
                                    FriendPicture = image,
                                };


                                
                            })
                        );
                        FriendsList.ItemsSource = friendsList;
                        ChatList.ItemsSource = friendsList;
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
            Server.PlayerDTO player = UserSessionManager.getInstance().GetPlayerUserData();
            UserSessionManager.getInstance().LogoutPlayer();   
            this.NavigationService.Navigate(new StartPage());
            _proxy.DisconnectUser(player.Username);

        }


        private void LoadImage(byte[] imageData)
        {
            
            BitmapImage image = ImageConvertor.ConvertByteArrayToImage(imageData);
            ProfileImageBrush.ImageSource = image;

        }

        // TODO: Pasar a utilities
 

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

        private void SelectFriendChat(object sender, SelectionChangedEventArgs e)
        {
            var selectedFriend = (Friend)ChatList.SelectedItem;

            if (selectedFriend != null) 
            {
                ChatGrid.Visibility = Visibility.Visible;
                FriendChatMessages.Clear();
                LoadFriendChatMessages(selectedFriend.UserName);
                labelFriendName.Content = selectedFriend.UserName;


            }
        }

        


        private void LoadFriendChatMessages(string username)
        {
            try
            {
                Server.PlayerDTO player = UserSessionManager.getInstance().GetPlayerUserData();
                var context = new InstanceContext(new FriendChatCallBackHandler());
                ChatFriendServer.ChatFriendServiceClient friendChatProxy = new ChatFriendServer.ChatFriendServiceClient(context);
                var response = friendChatProxy.GetChatHistory(username, player.Username);
                if (response.IsSuccess)
                {
                   
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

        private void SendMessageToFriend(object sender, RoutedEventArgs e)
        {
            try
            {
                Server.PlayerDTO player = UserSessionManager.getInstance().GetPlayerUserData();
                var context = new InstanceContext(new FriendChatCallBackHandler());
                ChatFriendServer.ChatFriendServiceClient friendChatProxy = new ChatFriendServer.ChatFriendServiceClient(context);
                friendChatProxy.SendMessageToFriend(player.Username, labelFriendName.Content.ToString(), textBoxFriendMessage.Text);
                Message friendMessage = new Message();
                friendMessage.Sender = player.Username;
                friendMessage.Messages = textBoxFriendMessage.Text;
                textBoxFriendMessage.Clear();
                MessageReceived(player.Username, labelFriendName.Content.ToString(), textBoxFriendMessage.Text);
                
            } catch (CommunicationException commEx)
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

        private void MessageReceived(string sender, string receiver, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FriendChatMessages.Add(new Message { Sender = sender, Messages = message, Alignment = HorizontalAlignment.Left });
            });

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
        public DateTime LastVisit {  get; set; }
        public Byte[] ProfileImage { get; set; }
        public int RequestId { get; set; }
        public BitmapImage FriendPicture { get; set; }
    }



    



}

