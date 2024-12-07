﻿using BMCWindows.DTOs;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using BMCWindows.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para LobbyCreationWindow.xaml
    /// </summary>
    public partial class LobbyCreationWindow : Page
    {
        public LobbyCreationWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
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
            var callbackHandler = new LobbyCallbackHandler();
            var callbackContext = new InstanceContext(callbackHandler);
            var proxy = new LobbyServer.LobbyServiceClient(callbackContext);

            var requestDTO = new LobbyServer.CreateLobbyRequestDTO
            {
                Username = UserSessionManager.getInstance().GetPlayerUserData().Username,
                Name = textBoxLobbyName.Text,
                Host = UserSessionManager.getInstance().GetPlayerUserData().Username,
                IsPrivate = privateToggleButton.IsChecked == true,
                Password = privateToggleButton.IsChecked == true ? textBoxLobbyPassword.Text : null
            };

            if ((privateToggleButton.IsChecked == false && !FieldValidator.AreFieldsEmpty(requestDTO.Name)) ||
                (privateToggleButton.IsChecked == true && !FieldValidator.AreFieldsEmpty(requestDTO.Name, requestDTO.Password)))
            {
                try
                {
                    var result = proxy.CreateLobby(requestDTO);

                    if (result.IsSuccess)
                    {
                        var lobbyDto = result.Lobby;

                        if (requestDTO.IsPrivate) { 
                            lobbyDto.Password = requestDTO.Password;
                        }


                        callbackHandler.PlayerJoined += (playerName, lobbyId) =>
                        {
                        };
                        callbackHandler.PlayerLeft += (playerName, lobbyId) =>
                        {
                        };

                        this.NavigationService.Navigate(new LobbyWindow(lobbyDto, proxy, callbackHandler));
                    }
                    else
                    {
                        MessageBox.Show(result.ErrorKey);
                    }
                }
                catch (TimeoutException ex)
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
            else
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("MessageBoxEmptyFields");
            }
        }

        private void CancelLobbyCreation(object sender, RoutedEventArgs e)
        {
           
            this.NavigationService.Navigate(new GameOptionsWindow());
        }

        private void CheckLimit(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int maxLength = int.Parse(textBox.Tag.ToString());

            if (textBox.Text.Length >= maxLength)
            {
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.IsReadOnly = false;
            }
        }
    }
}
