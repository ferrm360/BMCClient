using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;

namespace BMCWindows
{
    public partial class LobbyWindow : Page
    {
        private LobbyDTO _lobby;
        public ObservableCollection<string> FilteredPlayers { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        private LobbyServiceClient lobbyProxy;

        public LobbyWindow(LobbyDTO lobby, LobbyServiceClient proxy, LobbyCallbackHandler callbackHandler)
        {
            InitializeComponent();

            _lobby = lobby;
            lobbyProxy = proxy; // Usa el proxy existente
            DataContext = this;
            Messages = new ObservableCollection<Message>();
            FilteredPlayers = new ObservableCollection<string>();

            generalMessages.ItemsSource = Messages;
            listViewJoinedPlayer.ItemsSource = FilteredPlayers;

            // Vincular el callbackHandler a los eventos de UI
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
                    Messages.Add(new Message { Sender = "System", Messages = message });
                });
            };

            callbackHandler.PlayerLeftMessage += message =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add(new Message { Sender = "System", Messages = message });
                });
            };

            // Inicializar la UI y cargar los jugadores actuales en la lobby
            var player = UserSessionManager.getInstance().getPlayerUserData();
            textBlockCurrentPlayerUsername.Text = player.Username;
            labelLobbyName.Content = _lobby.Name;
            LoadPlayers();

            // Añadir al host en la lista de jugadores, si es el jugador actual
            if (_lobby.Host == player.Username)
            {
                FilteredPlayers.Add(player.Username);
            }
            else
            {
                OpenJoinWindow();
            }
        }

        private void LoadPlayers()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FilteredPlayers.Clear();
                foreach (var player in _lobby.Players)
                {
                    FilteredPlayers.Add(player);
                }
            });
        }

        private void OnPlayerJoined(string playerUsername)
        {
            if (!FilteredPlayers.Contains(playerUsername))
            {
                FilteredPlayers.Add(playerUsername);
                Messages.Add(new Message { Sender = "System", Messages = $"{playerUsername} se ha unido a la lobby." });
            }
        }

        private void OnPlayerLeft(string playerUsername)
        {
            if (FilteredPlayers.Contains(playerUsername))
            {
                FilteredPlayers.Remove(playerUsername);
                Messages.Add(new Message { Sender = "System", Messages = $"{playerUsername} ha salido de la lobby." });
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            var player = UserSessionManager.getInstance().getPlayerUserData();
            string lobbyId = _lobby.LobbyId;

            try
            {
                lobbyProxy.LeaveLobby(lobbyId, player.Username);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al salir de la lobby: {ex.Message}");
            }
        }

        private void SendGeneralMessage(object sender, RoutedEventArgs e)
        {
            var player = UserSessionManager.getInstance().getPlayerUserData();
            if (!string.IsNullOrEmpty(textboxGeneralChat.Text))
            {
                Messages.Add(new Message { Sender = player.Username, Messages = textboxGeneralChat.Text });
                textboxGeneralChat.Clear();
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

        private void JoinGame(LobbyDTO lobby, string password)
        {
            var player = UserSessionManager.getInstance().getPlayerUserData();
            if (lobby == null)
            {
                MessageBox.Show("Por favor, selecciona un lobby.");
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

                var response = lobbyProxy.JoinLobby(requestDTO);

                if (response.IsSuccess)
                {
                    _lobby = lobby;
                    OnPlayerJoined(player.Username);
                }
                else
                {
                    MessageBox.Show(response.ErrorKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al unirse a la lobby: {ex.Message}");
            }
        }
    }
}
