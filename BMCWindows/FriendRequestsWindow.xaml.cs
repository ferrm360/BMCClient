using BMCWindows.Patterns.Singleton;
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
        public FriendRequestsWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
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
                        RequestsList.ItemsSource = friendsList;
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response?.ErrorKey ?? "Unknown error"}");
                }
            }
            catch (CommunicationException commEx)
            {
                MessageBox.Show($"Communication error: {commEx.Message}");
            }
            catch (TimeoutException timeoutEx)
            {
                MessageBox.Show($"Timeout error: {timeoutEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}");
            }
        }


        private void GoBack(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
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
                    MessageBox.Show("Solicitud rechazada exitosamente");
                }
                else
                {
                    MessageBox.Show(result.ErrorKey);
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
                    MessageBox.Show("Solicitud rechazada exitosamente");
                }
                else 
                {
                    MessageBox.Show(result.ErrorKey);
                }

            }
                
    
        }
    }
}
