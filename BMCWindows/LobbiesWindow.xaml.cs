using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Lógica de interacción para LobbiesWindows.xaml
    /// </summary>
    public partial class LobbiesWindow : Page
    {
        public ObservableCollection<LobbyDTO> gameSessions { get; set; }
        public ObservableCollection<LobbyDTO> FilteredGameSessions { get; set; } = new ObservableCollection<LobbyDTO>();
        private LobbyDTO selectedLobby;
        private LobbyCallbackHandler _callbackHandler;
        private LobbyServiceClient _proxy;

        public LobbiesWindow()
        {
            InitializeComponent();

            // Inicializar el Callback Handler y Proxy
            _callbackHandler = new LobbyCallbackHandler();
            var callbackContext = new InstanceContext(_callbackHandler);
            _proxy = new LobbyServiceClient(callbackContext);

            gameSessions = new ObservableCollection<LobbyDTO>();
            ApplyToggleStyle(publicToggleButton);
            ApplyToggleStyle(privateToggleButton);

            LoadLobbiesList();
        }

        private void LoadLobbiesList()
        {
            if (gameSessions == null)
            {
                gameSessions = new ObservableCollection<LobbyDTO>();
            }



            // Crear el contexto de instancia con el callback vacío
            var callbackContext = new InstanceContext(new EmptyLobbyCallback());
            using (var proxy = new LobbyServer.LobbyServiceClient(callbackContext))
            {
                try
                {
                    var lobbies = proxy.GetAllLobbies();

                    gameSessions.Clear();

                    foreach (var lobby in lobbies)
                    {
                        gameSessions.Add(lobby);
                    }

                    FilterLobbies();

                    listBoxLobbies.ItemsSource = gameSessions;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los lobbies: {ex.Message}");
                }
            }
        }



        private void FilterLobbies()
        {
            FilteredGameSessions.Clear();
            foreach (var lobby in gameSessions)
            {
                if (privateToggleButton.IsChecked == true && lobby.IsPrivate)
                {
                    FilteredGameSessions.Add(lobby);
                }
                else if (publicToggleButton.IsChecked == true && !lobby.IsPrivate)
                {
                    FilteredGameSessions.Add(lobby);
                }
            }
        }

        private void LobbyListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem;
        }

        private void JoinLobby(object sender, RoutedEventArgs e)
        {
            if (selectedLobby == null)
            {
                MessageBox.Show("Por favor, selecciona un lobby.");
                return;
            }

            // Pasar el proxy y el callbackHandler al navegar a LobbyWindow
            this.NavigationService.Navigate(new LobbyWindow(selectedLobby, _proxy, _callbackHandler));
        }


        /* private void JoinGame(LobbyDTO lobby, string password)
         {
             Server.PlayerDTO player = new Server.PlayerDTO();
             player = UserSessionManager.getInstance().getPlayerUserData();
             if (lobby == null)
             {
                 MessageBox.Show("Por favor, selecciona un lobby.");
                 return;
             }

             using (LobbyServer.LobbyServiceClient proxy = new LobbyServer.LobbyServiceClient())
             {
                 var requestDTO = new JoinLobbyRequestDTO
                 {
                     LobbyId = lobby.LobbyId,
                     Username = player.Username, 
                     Password = password
                 };

                 var response = proxy.JoinLobby(requestDTO);

                 if (response.IsSuccess)
                 {
                     _lobby = lobby;
                     EventMediator.Instance.NotifyPlayerJoined(player.Username);
                     var playersList = new List<string>(_lobby.Players); 
                     playersList.Add(player.Username); 
                     _lobby.Players = playersList.ToArray();
                     LoadLobbiesList();
                     var lobbyWindow = new LobbyWindow(_lobby);


                 }
                 else
                 {
                     MessageBox.Show(response.ErrorKey);
                 }
             }
         }



         private void OpenJoinWindow(object sender, RoutedEventArgs e)
         {
             if (listBoxLobbies.SelectedItem == null)
             {
                 MessageBox.Show("Por favor, selecciona un lobby.");
                 return;
             }

             var selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem;

             if (selectedLobby.IsPrivate)
             {
                 passwordPopup.IsOpen = true;
             }
             else
             {
                 JoinGame(selectedLobby, null);
             }
         }

         private void AcceptPassword(object sender, RoutedEventArgs e)
         {
             var selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem;
             string password = passwordBox.Password; 

              JoinGame(selectedLobby, password);

             passwordPopup.IsOpen = false;
         }

         */
        private void ApplyToggleStyle(ToggleButton button)
        {
            var toggleToColorConverter = new BMCWindows.Utilities.ToggleToColorConverter();
            var toggleToForegroundConverter = new BMCWindows.Utilities.ToggleToForegroundConverter();

            var backgroundBinding = new Binding("IsChecked")
            {
                Source = button,
                Converter = toggleToColorConverter
            };
            button.SetBinding(ToggleButton.BackgroundProperty, backgroundBinding);

            var foregroundBinding = new Binding("IsChecked")
            {
                Source = button,
                Converter = toggleToForegroundConverter
            };
            button.SetBinding(ToggleButton.ForegroundProperty, foregroundBinding);
        }



    }


    
}