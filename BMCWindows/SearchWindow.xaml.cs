﻿using BMCWindows.DTOs;
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
    /// Lógica de interacción para SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Page
    {
        private ChatServer.ChatServiceClient _proxy;
        public SearchWindow()
        {
            InitializeComponent();
            _proxy = ChatServiceManager.ChatClient;
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            LoadPlayerList(player.Username);
        }

        private void LoadPlayerList(string username)
        {
            try
            {
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetPlayersList(username);

                if (response.IsSuccess)
                {
                    if (response.Friends != null && response.Friends.Any())
                    {
                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.Friends.Select(friendPlayer => new Friend
                            {
                                UserName = friendPlayer.Username,
                                
                            })
                        );
                        ListBoxPlayersList.ItemsSource = friendsList;                     
                    }
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(response.ErrorKey);
                }
            }
            catch (CommunicationException commEx)
            {
                MessageBox.Show(commEx.Message);
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException timeoutEx)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
        }

        private void SearchPlayer(object  sender, EventArgs e)
        {
            string searchPlayerUsername = TextboxSearch.Text;
            try
            {
                Server.PlayerDTO player = new Server.PlayerDTO();
                player = UserSessionManager.getInstance().GetPlayerUserData();
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetPlayersListByUsername(searchPlayerUsername, player.Username);

                if (response.IsSuccess)
                {
                    if (response.Player != null && response.Player.Any())
                    {
                        ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();

                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.Player.Select(friendPlayer =>
                            {
                                var friendProfilePicture = profileProxy.GetProfileImage(friendPlayer.Username);
                                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(friendProfilePicture.ImageData);

                                return new Friend
                                {
                                    UserName = friendPlayer.Username,
                                    FriendPicture = image,
                                };



                            })
                        );
                        ListBoxPlayersList.ItemsSource = friendsList;
                    }
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(response.ErrorKey);
                }
            }
            catch (CommunicationException commEx)
            {
                MessageBox.Show(commEx.Message);
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CommunicationError");
            }
            catch (TimeoutException timeoutEx)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.TimeoutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
        }

        private void GoBack(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void SelectPlayer(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxPlayersList.SelectedItem is Friend selectedPlayer)
            {
                PopupPlayer.DataContext = selectedPlayer;
                PopupPlayer.Placement = PlacementMode.Mouse;
                PopupPlayer.IsOpen = true;
            }
        }

        private void SendRequest(object sender, EventArgs e)
        {
            
            if (PopupPlayer.DataContext is Friend selectedPlayer)
            {
                string receiverUsername = selectedPlayer.UserName;
                FriendServer.FriendshipServiceClient proxy = new FriendServer.FriendshipServiceClient();
                Server.PlayerDTO player = new Server.PlayerDTO();
                player = UserSessionManager.getInstance().GetPlayerUserData();
                try
                {
                    var result = proxy.SendFriendRequest(player.Username, receiverUsername);

                    if (result.IsSuccess)
                    {
                        string message = Properties.Resources.Info_RequestSentSuccesfully.ToString();
                        MessageBox.Show(message);

                    }
                    else
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage("Error.GeneralException");
                    }
                }
                catch (CommunicationException commEx)
                {
                    MessageBox.Show(commEx.Message);
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.CommunicationError");
                }
                catch (TimeoutException timeoutEx)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.TimeoutError");
                }
                catch (Exception ex)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.GeneralException");
                }



            }
            else
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.NotSelectedPlayer");
            }
        }
    }
}
