using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BMCWindows.ChatLobbyServer;
using BMCWindows.DTOs;
using BMCWindows.EmailServer;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using BMCWindows.Validators;

namespace BMCWindows
{
    public partial class LobbyWindow : Page
    {
        private LobbyDTO _lobby;
        public ObservableCollection<string> filteredPlayers { get; set; }
        public ObservableCollection<string> filteredPlayersGameSession { get; set; }
        public ObservableCollection<Message> messages { get; set; }
        private LobbyServiceClient _lobbyProxy;
        private IChatLobbyService _chatService;
        private ChatCallBackHandler _chatCallbackHandler;

        public LobbyWindow(LobbyDTO lobby, LobbyServiceClient proxy, LobbyCallbackHandler callbackHandler)
        {
            InitializeComponent();

            _lobby = lobby;
            _lobbyProxy = proxy;
            DataContext = this;
            messages = new ObservableCollection<Message>();
            filteredPlayers = new ObservableCollection<string>();
            filteredPlayersGameSession = new ObservableCollection<string>();

            ListBoxGeneralMessages.ItemsSource = messages;
            ListViewJoinedPlayer.ItemsSource = filteredPlayers;

            _chatCallbackHandler = new ChatCallBackHandler();
            _chatCallbackHandler.MessageReceived += OnMessageReceived;

            InitializeChatService();

            callbackHandler.PlayerJoined += (playerName, lobbyId) =>
            {
                if (_lobby.LobbyId == lobbyId)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnPlayerJoined(playerName);
                    });
                }
            };

            callbackHandler.PlayerLeft += (playerName, lobbyId) =>
            {
                if (_lobby.LobbyId == lobbyId)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnPlayerLeft(playerName);
                    });
                }
            };

            callbackHandler.PlayerJoinedMessage += message =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    messages.Add(new Message { Sender = "System", Messages = message });
                });
            };

            callbackHandler.PlayerLeftMessage += message =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!messages.Any(m => m.Messages == message))
                    {
                        messages.Add(new Message { Sender = "System", Messages = message });
                    }
                    
                });
            };

            callbackHandler.PlayerKicked += () =>
            {
                
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnPlayerKicked();
                    });
                
            };


            callbackHandler.StartGame += lobbyId =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_lobby.LobbyId == lobbyId)
                    {
                        this.NavigationService.Navigate(new GameplayWindow(_lobby, filteredPlayers));
                    }
                });
            };


            var player = UserSessionManager.getInstance().GetPlayerUserData();
            TextBlockCurrentPlayerUsername.Text = player.Username;
            LabelLobbyName.Content = _lobby.Name;
            LoadPlayers();

            RegisterUserInChat(player.Username);

            if (_lobby.Host == player.Username)
            {
                filteredPlayersGameSession.Add(player.Username);
            }
            else
            {
                OpenJoinWindow();
            }
        }

        private void InitializeChatService()
        {
            try
            {
                var instanceContext = new InstanceContext(_chatCallbackHandler);
                var chatFactory = new DuplexChannelFactory<IChatLobbyService>(instanceContext, "NetTcpBinding_IChatLobbyService");
                _chatService = chatFactory.CreateChannel();
            }
            catch (EndpointNotFoundException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
            catch (CommunicationException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
        }

        private void RegisterUserInChat(string username)
        {
            try
            {
                _chatService.RegisterUser(username, _lobby.LobbyId);
            }
            catch (EndpointNotFoundException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
            catch (CommunicationException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
        }


        private void LoadPlayers()
        {
            var currentPlayer = UserSessionManager.getInstance().GetPlayerUserData().Username;

            Application.Current.Dispatcher.Invoke(() =>
            {
                filteredPlayers.Clear();
                foreach (var player in _lobby.Players)
                {
                    if (player != currentPlayer)
                    {
                        filteredPlayers.Add(player);
                        filteredPlayersGameSession.Add(player);
                    }
                }
            });
        }

        private void OnPlayerJoined(string playerUsername)
        {
            if (!filteredPlayersGameSession.Contains(playerUsername))
            {
                filteredPlayersGameSession.Add(playerUsername);
            }

            if (!filteredPlayers.Contains(playerUsername))
            {
                filteredPlayers.Add(playerUsername);
            }
        }

        private void OnPlayerLeft(string playerUsername)
        {
            if (filteredPlayers.Contains(playerUsername))
            {
                filteredPlayers.Remove(playerUsername);
                filteredPlayersGameSession.Remove(playerUsername);
            }
        }

        private void OnPlayerKicked() {
            NavigationService.GoBack();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            var player = UserSessionManager.getInstance().GetPlayerUserData();
            string lobbyId = _lobby.LobbyId;

            try
            {
                _lobbyProxy.LeaveLobby(lobbyId, player.Username);
                NavigationService.GoBack();
            }
            catch (EndpointNotFoundException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
            catch (CommunicationException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
        }

        private void SendGeneralMessage(object sender, RoutedEventArgs e)
        {
            var player = UserSessionManager.getInstance().GetPlayerUserData();
            if (!FieldValidator.AreFieldsEmpty(TextboxGeneralChat.Text))
            {
                var formattedMessage = $"{player.Username}: {TextboxGeneralChat.Text}";
                try
                {
                    _chatService.SendMessage(_lobby.LobbyId, player.Username, formattedMessage);
                    TextboxGeneralChat.Clear();
                }
                catch (EndpointNotFoundException)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    bool shouldNavigateToLogin = errorMessages.ShowServerErrorAndNavigateToLogin("Error.EndpointException", "Error.ServerErrorTitle");

                    if (shouldNavigateToLogin)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.NavigationService.Navigate(new LogIn());
                        });
                    }
                }
                catch (CommunicationException ex)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    bool shouldNavigateToLogin = errorMessages.ShowServerErrorAndNavigateToLogin("Error.EndpointException", "Error.ServerErrorTitle");

                    if (shouldNavigateToLogin)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.NavigationService.Navigate(new LogIn());
                        });
                    }
                }
                catch (TimeoutException)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    bool shouldNavigateToLogin = errorMessages.ShowServerErrorAndNavigateToLogin("Error.EndpointException", "Error.ServerErrorTitle");

                    if (shouldNavigateToLogin)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.NavigationService.Navigate(new LogIn());
                        });
                    }
                }
            }
        }

        private void OpenJoinWindow()
        {
            if (_lobby.IsPrivate)
            {
                PasswordPopup.IsOpen = true;
            }
            else
            {
                JoinGame(_lobby, null);
            }
        }

        private void AcceptPassword(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox.Password;
            JoinGame(_lobby, password);
            PasswordPopup.IsOpen = false;
        }

        private void JoinGame(LobbyDTO lobby, string password)
        {
            var player = UserSessionManager.getInstance().GetPlayerUserData();
            if (lobby == null)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Lobby.NotSelectedLobby");
                return;
            }

            try
            {
                var requestDTO = new JoinLobbyRequestDTO
                {
                    LobbyId = lobby.LobbyId,
                    Username = player.Username,
                    Password = password
                };

                var response = _lobbyProxy.JoinLobby(requestDTO);

                if (response.IsSuccess)
                {
                    _lobby = lobby;
                    var playersList = _lobby.Players.ToList(); 
                    playersList.Add(player.Username); 
                    _lobby.Players = playersList.ToArray();
                }
                else
                {
                    MessageBox.Show(response.ErrorKey);
                }
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            var player = UserSessionManager.getInstance().GetPlayerUserData();

            try
            {
                var response = _lobbyProxy.StartGame(_lobby.LobbyId, player.Username);

                if (!response.IsSuccess)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(response.ErrorKey);
                }
                else
                {
                    var context = new InstanceContext(new GameCallbackHandler());
                    var gameServiceClient = new GameplayServer.GameServiceClient(context);

                    var players = filteredPlayersGameSession.ToList().ToArray();

                    var response2 = gameServiceClient.InitializeGame(_lobby.LobbyId, players);

                    if (_lobby.Host == player.Username)
                    {
                        this.NavigationService.Navigate(new GameplayWindow(_lobby, filteredPlayersGameSession));
                    }
                }
            }
            catch (EndpointNotFoundException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
            catch (CommunicationException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                bool shouldNavigateToLogin = errorMessages.ShowServerErrorAndNavigateToLogin("Error.EndpointException", "Error.ServerErrorTitle");

                if (shouldNavigateToLogin)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.NavigationService.Navigate(new LogIn());
                    });
                }
            }
            catch (TimeoutException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TiemoutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }

        private void OnMessageReceived(string username, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                messages.Add(new Message { Sender = username, Messages = message });
            });
        }

        private void OnStartGameNotification(string lobbyId)
        {
            if (_lobby.LobbyId == lobbyId)
            {
                var context = new InstanceContext(new GameCallbackHandler());
                var gameServiceClient = new GameplayServer.GameServiceClient(context);

                var players = filteredPlayersGameSession.ToList().ToArray();

                var response = gameServiceClient.InitializeGame(_lobby.LobbyId, players);

                if (!response.IsSuccess)
                {
                    MessageBox.Show($"Error al inicializar el juego: {response.ErrorKey}");
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.NavigationService.Navigate(new GameplayWindow(_lobby, filteredPlayersGameSession));
                });
            }
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
                    }
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.ServerError");
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
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }

        private void LoadFriends(Object sender, RoutedEventArgs e)
        {
            {
                var player = UserSessionManager.getInstance().GetPlayerUserData();

                ListBoxFriendsList.Visibility = ListBoxFriendsList.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

                if (ListBoxFriendsList.Visibility == Visibility.Visible)
                {
                    LoadFriendList(player.Username);
                }
            }
        }

        private void SelectFriend(object sender, RoutedEventArgs e)
        {
            if (ListBoxFriendsList.SelectedItem != null)
            {

                var selectedItem = (Friend)ListBoxFriendsList.SelectedItem;

                string messageTitle = Properties.Resources.ConfirmInvitationTitle.ToString();
                string inviteMessage = string.Format(Properties.Resources.ConfirmInvitation, selectedItem.UserName);
                var result = MessageBox.Show(inviteMessage, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    EmailDTO emailDTO = new EmailDTO
                    {
                        LobbyName = _lobby.Name,
                        LobbyHost = _lobby.Host,
                        Username = selectedItem.UserName,
                        EmailType = "lobbyinvite"
                    };

                    if (_lobby.IsPrivate)
                    {
                        emailDTO.LobbyPassword = _lobby.Password;
                    }

                    InvitePlayerToLobby(emailDTO);
                }

                ListBoxFriendsList.SelectedItem = null;
            }
        }

        private void ShowServerErrorAndNavigateToLogin(string message, string title)
        {
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (this.NavigationService != null)
                    {
                        UserSessionManager.getInstance().LogoutPlayer();

                        this.NavigationService.Navigate(new LogIn());
                    }
                });
            }
        }


        private void InvitePlayerToLobby(EmailDTO emailDTO)
        {
            try
            {
                using (EmailServer.EmailServiceClient proxyEmail = new EmailServer.EmailServiceClient())
                {
                    var result = proxyEmail.SendEmail(emailDTO);

                    if (result.IsSuccess)
                    {
                        string message = Properties.Resources.Info_InvitationSuccessful.ToString();
                        MessageBox.Show(message);
                    }
                    else
                    {
                        string message = Properties.Resources.Info_InvitationEmailFailed.ToString();
                        MessageBox.Show(message);
                    }
                }
            }
            catch (EndpointNotFoundException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
            catch (CommunicationException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TiemoutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewJoinedPlayer.SelectedItem != null)
            {
                if (UserSessionManager.getInstance().GetPlayerUserData().Username != _lobby.Host)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.NotLobbyHost");
                    return;
                }
                var playerName = ListViewJoinedPlayer.SelectedItem.ToString();

                string confirmationMessage = string.Format(Properties.Resources.ConfirmationBanPlayer, playerName);
                string messageTitle = Properties.Resources.MessageTitleBanPlayer.ToString();
                var result = MessageBox.Show(confirmationMessage,
                                              messageTitle,
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var kickResult = _lobbyProxy.KickPlayer(_lobby.LobbyId, _lobby.Host, playerName);
                        if (kickResult.IsSuccess)
                        {
                            _lobby = kickResult.Lobby;
                        }
                        else
                        {
                            ErrorMessages errorMessages = new ErrorMessages();
                            errorMessages.ShowErrorMessage("Error.ExpelPlayer");
                        }
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage("Error.ServerError");
                    }
                    catch (CommunicationException ex)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage("Error.CommunicationError");
                    }
                    catch (TimeoutException ex)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage("Error.TiemoutError");
                    }
                    catch (Exception ex)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage("Error.ServerError");
                    }
                }
            }
        }


    }
}
