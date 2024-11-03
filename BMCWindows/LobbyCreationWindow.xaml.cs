using BMCWindows.DTOs;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    /// Lógica de interacción para LobbyCreationWindow.xaml
    /// </summary>
    public partial class LobbyCreationWindow : Page
    {
        public LobbyCreationWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            labelUser.Content = player.Username;
            ApplyToggleStyle(publicToggleButton);
            ApplyToggleStyle(privateToggleButton);
            publicToggleButton.Checked += ToggleButtonsChecked;
            privateToggleButton.Checked += ToggleButtonsChecked;
            labelLobbyPassword.Visibility = Visibility.Hidden;
            imageTextBoxLobbyPassword.Visibility = Visibility.Hidden;
            textBoxLobbyPassword.Visibility = Visibility.Hidden;


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
            if (sender == publicToggleButton)
            {
                privateToggleButton.IsChecked = false;
                textBoxLobbyPassword.Visibility = Visibility.Hidden;
                imageTextBoxLobbyPassword.Visibility = Visibility.Hidden;
                labelLobbyPassword.Visibility = Visibility.Hidden;
            }
            else if (sender == privateToggleButton)
            {
                publicToggleButton.IsChecked = false;
                textBoxLobbyPassword.Visibility = Visibility.Visible;
                imageTextBoxLobbyPassword.Visibility = Visibility.Visible;
                labelLobbyPassword.Visibility = Visibility.Visible;

            }

        }


        private void CreateLobby(object sender, RoutedEventArgs e)
        {
            LobbyServer.LobbyServiceClient proxy = new LobbyServer.LobbyServiceClient();
            LobbyServer.CreateLobbyRequestDTO requestDTO = new LobbyServer.CreateLobbyRequestDTO();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            string lobbyName = textBoxLobbyName.Text;
            string lobbyPassword = textBoxLobbyPassword.Text;


            requestDTO.Username = player.Username;
            requestDTO.Name = lobbyName;
            requestDTO.Host = player.Username;
            if (privateToggleButton.IsChecked == true)
            {
               requestDTO.IsPrivate = true;
               requestDTO.Password = lobbyPassword;

            }
            else
            {
                requestDTO.IsPrivate = false;
                requestDTO.Password = null;
            }
            if (privateToggleButton.IsChecked == false && !FieldValidator.AreFieldsEmpty(lobbyName) || privateToggleButton.IsChecked == true && !FieldValidator.AreFieldsEmpty(lobbyName, lobbyPassword))
            {
                var result = proxy.CreateLobby(requestDTO);
                
                if (result.IsSuccess) 
                {
                    var lobbyDto = result.Lobby;
                    Console.WriteLine(" " + proxy.GetAllLobbies().Length);
                    this.NavigationService.Navigate(new LobbyWindow(lobbyDto));
                } else
                {
                    MessageBox.Show(result.ErrorKey);
                }

            }
            else
            {
                MessageBox.Show("No se puede crear el lobby hay campos vacíos, verífiquelos");
            }


            


        }

        private void CancelLobbyCreation(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


    }



}
