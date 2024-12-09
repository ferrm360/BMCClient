using BMCWindows.DTOs;
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
    /// Lógica de interacción para FriendRequestsWindow.xaml
    /// </summary>
    public partial class FriendRequestsWindow : Page
    {
        private ChatServer.ChatServiceClient _proxy;
        public FriendRequestsWindow()
        {
            InitializeComponent();
            _proxy = ChatServiceManager.ChatClient;
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            LoadFriendshipRequests(player.Username);
        }

        private void LoadFriendshipRequests(string username)
        {
            try
            {
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetFriendRequestList(username);

                if (response.IsSuccess)
                {
                    if (response.FriendRequests != null && response.FriendRequests.Any())
                    {
                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.FriendRequests.Select(friendPlayer => new Friend
                            {
                                UserName = friendPlayer.ReceiverUsername,
                                 RequestId = friendPlayer.RequestID,

                            })
                        );
                        ListBoxRequestsList.ItemsSource = friendsList;
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
                errorMessages.ShowErrorMessage("Error.ServerError");
            }
        }


        private void GoBack(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void AcceptFriend(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedPlayer = button.DataContext as Friend;

            if (selectedPlayer != null)
            {
                FriendServer.FriendshipServiceClient proxy = new FriendServer.FriendshipServiceClient();
                var result = proxy.AcceptFriendRequest(selectedPlayer.RequestId);
                
                if (result.IsSuccess)
                {
                    Server.PlayerDTO player = new Server.PlayerDTO();
                    player = UserSessionManager.getInstance().GetPlayerUserData();
                    string friendMessage = Properties.Resources.Friend_AcceptRequest.ToString();
                    string messageTitle = Properties.Resources.FriendLabelFriends.ToString();
                    MessageBoxResult messageResult = MessageBox.Show(friendMessage, messageTitle,MessageBoxButton.OK,MessageBoxImage.Information);
                    
                    if( messageResult == MessageBoxResult.OK)
                    {
                        LoadFriendshipRequests(player.Username);
                        this.NavigationService.Navigate(new FriendRequestsWindow());
                    }
                    
                }
                else
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(result.ErrorKey);
                }

            }

        }

        private void RejectFriend(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedPlayer = button.DataContext as Friend;

            if (selectedPlayer != null)
            {
                FriendServer.FriendshipServiceClient proxy = new FriendServer.FriendshipServiceClient();
                var result = proxy.RejectFriendResponse(selectedPlayer.RequestId);
                if (result.IsSuccess)
                {
                    Server.PlayerDTO player = new Server.PlayerDTO();
                    player = UserSessionManager.getInstance().GetPlayerUserData();

                    string friendMessage = Properties.Resources.Friend_DeclinedRequest.ToString();
                    string messageTitle = Properties.Resources.FriendLabelFriends.ToString();
                    MessageBoxResult messageResult =  MessageBox.Show(friendMessage, messageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    if( messageResult == MessageBoxResult.OK)
                    {
                        LoadFriendshipRequests(player.Username);
                        this.NavigationService.Navigate(new FriendRequestsWindow());
                    }

                }
                else 
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(result.ErrorKey);
                }

            }
                
    
        }
    }
}
