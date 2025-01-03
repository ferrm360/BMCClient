﻿using BMCWindows.GuestPlayerServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para GameOptionsWindow.xaml
    /// </summary>
    public partial class GameOptionsWindow : Page
    {
        //private ChatServer.ChatServiceClient _proxy;
        public GameOptionsWindow()
        {
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            InitializeComponent();
            var _proxy = ChatServiceManager.ChatClient;

            if (UserSessionManager.getInstance().IsGuestUser())
            {
                ButtonLogout.Visibility = Visibility.Visible;
                ButtonCancel.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonCancel.Visibility = Visibility.Visible;
                ButtonLogout.Visibility = Visibility.Collapsed;
            }
        }


        private void Cancel(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void GoToCreateLobbyWindow(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new LobbyCreationWindow());
        }

        private void GoToJoinLobbyWindow(object sender, EventArgs e) 
        {
            this.NavigationService.Navigate(new LobbiesWindow());
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            GuestPlayerServer.GuestPlayerServiceClient proxy = new GuestPlayerServer.GuestPlayerServiceClient();
            try
            {
                var result = proxy.LogoutGuestPlayer(UserSessionManager.getInstance().GetPlayerUserData().Username);

                if (result.IsSuccess)
                {
                    UserSessionManager.getInstance().LogoutPlayer();
                    String infoMessage = Properties.Resources.ResourceManager.GetString("Info.GuestLogOut");
                    MessageBox.Show(infoMessage);
                    this.NavigationService.GoBack();

                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(result.ErrorKey);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
