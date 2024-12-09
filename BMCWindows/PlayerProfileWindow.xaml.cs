using BMCWindows.DTOs;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Lógica de interacción para PlayerProfileWindow.xaml
    /// </summary>
    public partial class PlayerProfileWindow : Page
    {
        private string _username;
        private ChatServer.ChatServiceClient _proxy;
        public PlayerProfileWindow(string username)
        {
            InitializeComponent();
            _username = username;
            _proxy = ChatServiceManager.ChatClient;
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            LabelUser.Content = _username;
            LoadFriendList(_username);
            ProfileServer.ProfileServiceClient proxyProfile = new ProfileServer.ProfileServiceClient();
            string bio = proxyProfile.GetBioByUsername(_username);
            TextBlockBio.Text = bio;
            var imageUrl = proxyProfile.GetProfileImage(_username);
            if (imageUrl.ImageData == null || imageUrl.ImageData.Length == 0)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.EmptyImage");
            }
            else
            {
                BitmapImage image = ImageConvertor.ConvertByteArrayToImage(imageUrl.ImageData);
                if (image == null)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.ImageNotFound");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProfileImageBrush.ImageSource = image;
                    });

                }
            }

            var profile = proxyProfile.GetProfileByUsername(_username);
            InitializeScores();

        }

        private void LoadFriendList(string username)
        {
            try
            {
                FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
                var response = friendsProxy.GetFriendList(username);

                if (response.IsSuccess)
                {
                    if (response.Friends != null && response.Friends.Any())
                    {
                        ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();

                        ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                            response.Friends.Select(friendPlayer =>
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
                        ListBoxFriendsList.ItemsSource = friendsList;
                        
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
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
        }

        private void InitializeScores()
        {
            try
            {
                
                PlayerScoreServer.PlayerScoresServiceClient scoreProxy = new PlayerScoreServer.PlayerScoresServiceClient();
                var response = scoreProxy.GetScoresByUsername(_username);
                if (response.IsSuccess)
                {
                    if (response.Scores != null)
                    {
                        TextBlockPlayerWins.Text = response.Scores.Wins.ToString();
                        TextBlockPlayerLosses.Text = response.Scores.Losses.ToString();

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
                errorMessages.ShowErrorMessage("Error.TimeOutError");
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }


        }



        

        private void DeleteFriend(object sender, RoutedEventArgs e)
        {
            FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
            string confimationMessage = string.Format(Properties.Resources.Friend_ConfirmationDeleteFriends, _username);
            string messageTitle = Properties.Resources.Friend_DeleteFriendConfirmationTitle;
            MessageBoxResult messageBoxResult =  MessageBox.Show(confimationMessage, messageTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if(messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    var response = friendsProxy.DeleteFriend(currentPlayer.Username, _username);
                    if (response.IsSuccess == true)
                    {
                        string messageDeletedFriendTitle = Properties.Resources.Friend_DeletedFriend;
                        string messageDeletedFriendMessage = Properties.Resources.Friend_DeletionSuccess;
                        MessageBoxResult messageResult = MessageBox.Show(messageDeletedFriendMessage, messageDeletedFriendTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                        if (messageBoxResult == MessageBoxResult.OK)
                        {
                            ButtonDeleteFriend.Visibility = Visibility.Hidden;
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
                    errorMessages.ShowErrorMessage("Error.TimeOutError");
                }
                catch (Exception ex)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.GeneralException");
                }

            }

        }

        private void GoBack(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
