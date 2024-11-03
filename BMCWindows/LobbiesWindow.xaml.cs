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
        public ObservableCollection<LobbyDTO> gameSessions {  get; set; }
        public ObservableCollection<LobbyDTO> FilteredGameSessions { get; set; } = new ObservableCollection<LobbyDTO>();
        private LobbyDTO selectedLobby;
        private LobbyDTO _lobby = new LobbyDTO();


        public LobbiesWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            gameSessions = new ObservableCollection<LobbyDTO>();
            ApplyToggleStyle(publicToggleButton);
            ApplyToggleStyle(privateToggleButton);
            LoadLobbiesList();


        }

        private void LoadLobbiesList()
        {
            // Asegúrate de que gameSessions esté inicializada
            if (gameSessions == null)
            {
                gameSessions = new ObservableCollection<LobbyDTO>(); // Inicializa la lista si es null
            }

            // Crea el cliente del servicio
            using (var proxy = new LobbyServer.LobbyServiceClient())
            {
                try
                {
                    // Obtiene la lista de lobbies del servidor
                    var lobbies = proxy.GetAllLobbies();

                    gameSessions.Clear(); // Limpia la lista antes de llenarla

                    // Agrega cada lobby a la lista
                    foreach (var lobby in lobbies)
                    {
                        gameSessions.Add(lobby);
                    }

                    // Filtra los lobbies si es necesario
                    FilterLobbies();

                    // Asigna la fuente de elementos al ListBox
                    listBoxLobbies.ItemsSource = gameSessions; // Asegúrate de que este sea el nombre correcto de tu ListBox
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción que pueda ocurrir al llamar al servicio
                    MessageBox.Show($"Error al cargar los lobbies: {ex.Message}");
                }
            }
        }


        private void FilterLobbies()
        {
            FilteredGameSessions.Clear();
            foreach (var lobby in gameSessions)
            {
                // Agregar lobbies según el tipo seleccionado
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


        private void JoinGame(LobbyDTO lobby, string password)
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            // Verifica que haya un lobby seleccionado
            if (lobby == null)
            {
                MessageBox.Show("Por favor, selecciona un lobby.");
                return;
            }

            // Continúa con la lógica para unirse al lobby
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
                    var playersList = new List<string>(_lobby.Players); // Crea una lista temporal
                    playersList.Add(player.Username); // Agrega el jugador
                    _lobby.Players = playersList.ToArray();
                    LoadLobbiesList();
                    this.NavigationService.Navigate(new LobbyWindow(lobby));
                    
                }
                else
                {
                    MessageBox.Show(response.ErrorKey);
                }
            }
        }

        private void LobbyListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem; 
        }

        private void OpenJoinWindow(object sender, RoutedEventArgs e)
        {
            // Verifica que haya un lobby seleccionado
            if (listBoxLobbies.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un lobby.");
                return;
            }

            // Obtiene el lobby seleccionado
            var selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem;

            // Si el lobby es privado, muestra el popup para ingresar la contraseña
            if (selectedLobby.IsPrivate)
            {
                passwordPopup.IsOpen = true; // Muestra el popup
            }
            else
            {
                // Si no es privado, únete directamente
                JoinGame(selectedLobby, null);
            }
        }

        private void AcceptPassword(object sender, RoutedEventArgs e)
        {
            var selectedLobby = (LobbyDTO)listBoxLobbies.SelectedItem;
            string password = passwordBox.Password; // Obtiene la contraseña ingresada

            // Llama al método para unirte al lobby con la contraseña
             JoinGame(selectedLobby, password);

            // Cierra el popup después de unirse
            passwordPopup.IsOpen = false;
        }

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
