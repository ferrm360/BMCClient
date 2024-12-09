using BMCWindows.DTOs;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Properties;
using BMCWindows.Server;
using BMCWindows.Utilities;
using BMCWindows.Validators;
using BMCWindows.VerificationCode;
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
        private ObservableCollection<Friend> friends { get; set; }
        private ObservableCollection<Message> messages { get; set; }
        private ChatServer.ChatServiceClient _proxy;
        private ObservableCollection<Message> friendChatMessages { get; set; } = new ObservableCollection<Message>();
        private FriendChatCallBackHandler _friendChatCallbackHandler;
        private ObservableCollection<Score> playersScore { get; set; } = new ObservableCollection<Score>();


        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            _friendChatCallbackHandler = new FriendChatCallBackHandler();
            _friendChatCallbackHandler.FriendMessageReceived += MessageReceived;
            messages = new ObservableCollection<Message>();
            ListBoxGeneralMessages.ItemsSource = messages;
            ItemsControlChatMessages.ItemsSource = friendChatMessages;
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            InstanceContext context = new InstanceContext(this);
            ChatServiceManager.InitializeChatClient(context);
            _proxy = ChatServiceManager.ChatClient;
            _proxy.RegisterUser(player.Username);
            LabelUserName.Content = player.Username;
            LoadFriendList(player.Username);
            ButtonOpenContextMenu.Visibility = Visibility.Visible;
            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            var imageUrl = proxyProfile.GetProfileImage(player.Username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.EmptyImage");
            }
            else
            {
                try
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
                } catch(Exception ex)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                }
                
            }

            InitializeScores();
        }

        private void SendGeneralMessage(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();

            if (!FieldValidator.AreFieldsEmpty(TextboxGeneralChat.Text))
            {
                var formattedMessage = $"{player.Username}: {TextboxGeneralChat.Text}";
                _proxy.SendMessage(player.Username, formattedMessage);
                ReceiveMessage(TextboxGeneralChat.Text);
                TextboxGeneralChat.Clear();
            }
        }

        public void ReceiveMessage(string message)
        {
            Server.PlayerDTO player = UserSessionManager.getInstance().GetPlayerUserData();

            Application.Current.Dispatcher.Invoke(() =>
            {
                messages.Add(new Message { Sender = player.Username, Messages = message });
            });

        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new VerificationCodePage());
        }

        public void OpenContextMenu(object sender, RoutedEventArgs e)
        {

            MenuOptions.Visibility = MenuOptions.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
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
                        ListBoxFriendsList.ItemsSource = friendsList;
                        ListBoxChatList.ItemsSource = friendsList;
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
                errorMessages.ShowErrorMessage("Error.TimeOutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
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


        

        private void GoToFriendRequestsWindow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FriendRequestsWindow());
        }

        private void SelectFriend(object sender, SelectionChangedEventArgs e)
        {
            var selectedFriend = (Friend)ListBoxFriendsList.SelectedItem;

            if (selectedFriend != null)
            {
                OpenPlayerProfileWindow(selectedFriend.UserName);
            }
        }

        private void SelectFriendChat(object sender, SelectionChangedEventArgs e)
        {
            var selectedFriend = (Friend)ListBoxChatList.SelectedItem;

            if (selectedFriend != null)
            {
                GridChat.Visibility = Visibility.Visible;
                friendChatMessages.Clear();
                LoadFriendChatMessages(selectedFriend.UserName);
                LabelFriendName.Content = selectedFriend.UserName;
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
                friendChatProxy.SendMessageToFriend(player.Username, LabelFriendName.Content.ToString(), textBoxFriendMessage.Text);
                Message friendMessage = new Message();
                friendMessage.Sender = player.Username;
                friendMessage.Messages = textBoxFriendMessage.Text;
                textBoxFriendMessage.Clear();
                MessageReceived(player.Username, LabelFriendName.Content.ToString(), textBoxFriendMessage.Text);

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

        private void MessageReceived(string sender, string receiver, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                friendChatMessages.Add(new Message { Sender = sender, Messages = message, Alignment = HorizontalAlignment.Left });
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


        private void InitializeScores()
        {
            try
            {
                Server.PlayerDTO player = new Server.PlayerDTO();
                player = UserSessionManager.getInstance().GetPlayerUserData();
                PlayerScoreServer.PlayerScoresServiceClient scoreProxy = new PlayerScoreServer.PlayerScoresServiceClient();
                var response = scoreProxy.GetAllScores();
                if (response.IsSuccess)
                {
                    if (response.Scores != null)
                    {
                        if (response.Scores != null && response.Scores.Any())
                        {
                            ObservableCollection<Score> scoresList = new ObservableCollection<Score>(
                                response.Scores .Select(playerScore =>
                                {
                                    return new Score
                                    {
                                        Username = playerScore.PlayerName,
                                        Losses = playerScore.Losses,
                                        Wins = playerScore.Wins,
                                    };
                                })
                            );
                            ListViewPlayerScoresTable.ItemsSource = scoresList;
                        };
                       
                       

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
                errorMessages.ShowErrorMessage("Error.TimeOutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }


        }

        private void SeeCards(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CardWindow());
        }

        private void ChangeLanguageToSpanish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "es-MX";
            Properties.Settings.Default.Save();
            SetLanguage("es-MX");
        }

        private void ChangeLanguageToEnglish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
            SetLanguage("en-US");
        }

        private void OpenContextMenuLanguage(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var contextMenu = button?.ContextMenu;


            if (contextMenu != null)
            {
                contextMenu.PlacementTarget = button;
                contextMenu.IsOpen = true;
            }
        }

        private void SetLanguage(string languageCode)
        {
            var cultureInfo = new System.Globalization.CultureInfo(languageCode);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var currentPage = Application.Current.MainWindow?.Content as Page;
            if (currentPage != null)
            {
                currentPage.Language = System.Windows.Markup.XmlLanguage.GetLanguage(cultureInfo.Name);
            }
            var player = UserSessionManager.getInstance().GetPlayerUserData();
            UserSessionManager.getInstance().LoginPlayer(player, false);
            this.NavigationService.Navigate(new HomePage());



        }
    }

    
}