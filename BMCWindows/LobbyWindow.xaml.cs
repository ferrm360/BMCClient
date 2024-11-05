using BMCWindows.ChatServer;
using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
    /// Lógica de interacción para LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Page, IChatServiceCallback 
    {
        private LobbyDTO _lobby;
        public ObservableCollection<string> FilteredPlayers { get; set; } 
        public ObservableCollection<Message> Messages { get; set; }
        public ChatService chatService = new ChatService();
        public ChatServer.ChatServiceClient proxy;
        LobbyServer.LobbyServiceClient lobbyProxy;

        public LobbyWindow(LobbyDTO lobby)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            InitializeComponent();
            
            _lobby = lobby;
            DataContext = this;
            labelLobbyName.Content = _lobby.Name;
            Messages = new ObservableCollection<Message>();
            generalMessages.ItemsSource = Messages;
            InstanceContext context = new InstanceContext(this);
            proxy = new ChatServer.ChatServiceClient(context);
            lobbyProxy = new LobbyServer.LobbyServiceClient();
            FilteredPlayers = new ObservableCollection<string>();
            listViewJoinedPlayer.ItemsSource = FilteredPlayers;
            EventMediator.Instance.PlayerJoined += OnPlayerJoined;
            EventMediator.Instance.PlayerLeft += OnPlayerLeft;

            textBlockCurrentPlayerUsername.Text = player.Username;
            LoadPlayers();
            if(_lobby.Host == player.Username)
            {
                FilteredPlayers.Add(player.Username);
            }

            if(_lobby.Host != player.Username)
            {
                OpenJoinWindow();
            }
        }





        private void UpdatePlayerList(List<string> updatedPlayers)
        {
            FilteredPlayers.Clear();
            Server.PlayerDTO currentPlayer = new Server.PlayerDTO();
            currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            foreach (var player in updatedPlayers)
            {
                if(player != currentPlayer.Username)
                {
                    FilteredPlayers.Add(player);

                }
            }
        }

        private void LoadPlayers()
        {
            Console.WriteLine("LoadPlayers");
            Application.Current.Dispatcher.Invoke(() =>
            {
                FilteredPlayers.Clear(); // Limpia la colección antes de agregar

                Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();

                foreach (var player in _lobby.Players)
                {
                    Console.WriteLine(player);
                            Console.WriteLine("Entra");

                            FilteredPlayers.Add(player); // Agrega los jugadores excepto el actual

                            foreach (var player2 in FilteredPlayers)
                            {
                                Console.WriteLine("Jugadores en Filtered players:" + player2);

                            }

                        

                    
                    
                }
            }); 
            

        }


        private void OnPlayerJoined(string playerUsername)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Jugador recibido en OnPlayerJoined: {playerUsername}");

                // Agrega el nuevo jugador a la lista interna del lobby
                if (!_lobby.Players.Contains(playerUsername))
                {
                    var playersList = new List<string>(_lobby.Players);
                    playersList.Add(playerUsername);
                    _lobby.Players = playersList.ToArray();
                }

                // Actualiza la ObservableCollection para reflejar el cambio en la UI
                if (!FilteredPlayers.Contains(playerUsername))
                {
                    FilteredPlayers.Add(playerUsername); // Asegúrate de que también se actualice en la UI
                }
            });

            LoadPlayers();
        }


        private void OnPlayerLeft(string playerUsername)
        {
            Server.PlayerDTO player = UserSessionManager.getInstance().getPlayerUserData();
            if (_lobby.Players.Contains(playerUsername))
            {
                LoadPlayers();
                
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
           
            string lobbyId = _lobby.LobbyId;
            lobbyProxy.LeaveLobby(lobbyId, player.Username);
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


        private void JoinGame(LobbyDTO lobby, string password)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            if (lobby == null)
            {
                MessageBox.Show("Por favor, selecciona un lobby.");
                return;
            }

            using (lobbyProxy)
            {
                var requestDTO = new JoinLobbyRequestDTO
                {
                    LobbyId = lobby.LobbyId,
                    Username = player.Username,
                    Password = password
                };

                var response = lobbyProxy.JoinLobby(requestDTO);

                if (response.IsSuccess)
                {
                    _lobby = lobby;
                    LoadPlayers();
                    EventMediator.Instance.NotifyPlayerJoined(player.Username);
                    


                }
                else
                {
                    MessageBox.Show(response.ErrorKey);
                }
            }
        }

        private void OpenJoinWindow()
        {
            

            if (_lobby.IsPrivate)
            {
                passwordPopup.IsOpen = true;
            }
            else
            {
                JoinGame(_lobby, null);
            }
        }

        private void AcceptPassword(object sender, RoutedEventArgs e)
        {
            
            string password = passwordBox.Password;

            JoinGame(_lobby, password);

            passwordPopup.IsOpen = false;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            if(_lobby.Players.Length == 2)
            {
                this.NavigationService.Navigate(new GameplayWindow(_lobby));
            }
        }

    }
}
