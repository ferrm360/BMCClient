using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Page
    {
        private LobbyDTO _lobby;
        public ObservableCollection<string> FilteredPlayers { get; set; } = new ObservableCollection<string>();


        public LobbyWindow(LobbyDTO lobby)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            InitializeComponent();
            listViewJoinedPlayer.ItemsSource = FilteredPlayers;
            _lobby = lobby;
            DataContext = this;
            labelLobbyName.Content = _lobby.Name;

            EventMediator.Instance.PlayerJoined += OnPlayerJoined;
            EventMediator.Instance.PlayerLeft += OnPlayerLeft;

            textBlockCurrentPlayerUsername.Text = player.Username;
            LoadPlayers();
            StartPolling();
        }


        private async void StartPolling()
        {
            while (true)
            {
                await Task.Delay(5000); // Espera 5 segundos
                LoadPlayers(); // Llama a un método que obtenga la lista de jugadores actualizada
               var playersList =  _lobby.Players.ToList();
                foreach (var player in playersList) 
                {
                    Console.WriteLine(player.ToString());
                }
            }
        }



        private void UpdatePlayerList(List<string> updatedPlayers)
        {
            // Limpiar la colección antes de agregar los nuevos jugadores
            FilteredPlayers.Clear();
            Server.PlayerDTO currentPlayer = new Server.PlayerDTO();
            currentPlayer = UserSessionManager.getInstance().getPlayerUserData();

            // Agregar los jugadores actualizados a la colección
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
            FilteredPlayers.Clear();
            Server.PlayerDTO currentPlayer = new Server.PlayerDTO();
            currentPlayer = UserSessionManager.getInstance().getPlayerUserData();

            foreach (var player in _lobby.Players)
            {
                if(player != currentPlayer.Username)
                {
                    FilteredPlayers.Add(player);

                }


            }


        }

        private void OnPlayerJoined(string playerUsername)
        {
            // Agrega el nuevo jugador a la lista de jugadores
            MessageBox.Show($"Jugador recibido en OnPlayerJoined: {playerUsername}");
            if (!_lobby.Players.Contains(playerUsername))
            {
                var playersList = new List<string>(_lobby.Players); // Crea una lista temporal
                playersList.Add(playerUsername); // Agrega el jugador
                _lobby.Players = playersList.ToArray();  // Actualiza la lista de jugadores mostrada en la interfaz
                LoadPlayers();
            }

            
        }

        private void OnPlayerLeft(string playerUsername)
        {
            // Maneja la salida de un jugador
            if (_lobby.Players.Contains(playerUsername))
            {
                LoadPlayers(); // Actualiza la lista de jugadores mostrada en la interfaz
            }
        }

    }
}
