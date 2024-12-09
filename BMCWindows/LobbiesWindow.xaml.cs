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
        public ObservableCollection<LobbyDTO> filteredGameSessions { get; set; } = new ObservableCollection<LobbyDTO>();
        private LobbyDTO _selectedLobby;
        private LobbyCallbackHandler _callbackHandler;
        private LobbyServiceClient _proxy;

        public LobbiesWindow()
        {
            InitializeComponent();

            _callbackHandler = new LobbyCallbackHandler();
            var callbackContext = new InstanceContext(_callbackHandler);
            _proxy = new LobbyServiceClient(callbackContext);
            gameSessions = new ObservableCollection<LobbyDTO>();
            DataContext = this;

           LoadLobbiesList();
        }

        private void LoadLobbiesList()
        {
            if (gameSessions == null)
            {
                gameSessions = new ObservableCollection<LobbyDTO>();
            }



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

                    

                    ListViewLobbies.ItemsSource = gameSessions;
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
                errorMessages.ShowErrorMessage("Lobby.LoadError");
            }
            }
        }

        

        private void LobbyListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedLobby = (LobbyDTO)ListViewLobbies.SelectedItem;
        }

        private void JoinLobby(object sender, RoutedEventArgs e)
        {
            if (_selectedLobby == null)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Lobby.NotSelectedLobby");
                return;
            }

            this.NavigationService.Navigate(new LobbyWindow(_selectedLobby, _proxy, _callbackHandler));
        }

        
       


        private void Cancel(object sender, RoutedEventArgs e)
        {
            
            this.NavigationService.Navigate(new GameOptionsWindow());
        }
    }
}