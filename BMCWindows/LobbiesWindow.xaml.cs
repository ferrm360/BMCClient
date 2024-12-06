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

            _callbackHandler = new LobbyCallbackHandler();
            var callbackContext = new InstanceContext(_callbackHandler);
            _proxy = new LobbyServiceClient(callbackContext);
            publicToggleButton.Checked += ToggleButtonsChecked;
            privateToggleButton.Checked += ToggleButtonsChecked;
            gameSessions = new ObservableCollection<LobbyDTO>();
            DataContext = this;
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
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Lobby.LoadError");
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
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Lobby.NotSelectedLobby");
                return;
            }

            this.NavigationService.Navigate(new LobbyWindow(selectedLobby, _proxy, _callbackHandler));
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

        private void ToggleButtonsChecked(object sender, RoutedEventArgs e)
        {
            FilteredGameSessions.Clear();

            if (sender == publicToggleButton)
            {
                FilteredGameSessions.Clear();
                privateToggleButton.IsChecked = false; 
                foreach (var lobby in gameSessions)
                {
                    if (!lobby.IsPrivate)
                    {
                        FilteredGameSessions.Add(lobby);
                    }
                }
            }
            else if (sender == privateToggleButton)
            {
                FilteredGameSessions.Clear();
                publicToggleButton.IsChecked = false; 
                foreach (var lobby in gameSessions)
                {
                    if (lobby.IsPrivate)
                    {
                        FilteredGameSessions.Add(lobby);
                    }
                }
            }
        }


        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}