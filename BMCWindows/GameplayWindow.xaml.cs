using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
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
    /// Lógica de interacción para GameplayWindow.xaml
    /// </summary>
    public partial class GameplayWindow : Page
    {
        private LobbyDTO _lobby;
        public ObservableCollection<Player> Player1 { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<Player> Player2 { get; set; } = new ObservableCollection<Player>();

        public GameplayWindow(LobbyDTO lobby)
        {
            InitializeComponent();
            _lobby = lobby;
            Player1Cards.ItemsSource = LoadPlayer1List();
            Player2Cards.ItemsSource = LoadCurrentPlayerList();
            textBlockGameName.Text = _lobby.Name;
        }

        private ObservableCollection<Player> LoadPlayer1List()
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                foreach (var player in _lobby.Players)
                {
                    if (currentPlayer.Username != player)
                    {
                        var playerProfilePicture = profileProxy.GetProfileImage(player);
                        if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                        {
                            BitmapImage image = ConvertByteArrayToImage(playerProfilePicture.ImageData);
                            playerList.Add(new Player { Username = player, ProfilePicture = image });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading player 1 list: {ex.Message}");
            }

            return playerList;
        }

        private ObservableCollection<Player> LoadCurrentPlayerList()
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                var playerProfilePicture = profileProxy.GetProfileImage(currentPlayer.Username);
                if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                {
                    BitmapImage image = ConvertByteArrayToImage(playerProfilePicture.ImageData);
                    playerList.Add(new Player { Username = currentPlayer.Username, ProfilePicture = image });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading current player: {ex.Message}");
            }

            return playerList;
        }

        private BitmapImage ConvertByteArrayToImage(byte[] imageData)
        {
            using (var stream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 
                return bitmapImage;
            }
        }

        private void LeaveGame(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();

        }
    }

}
