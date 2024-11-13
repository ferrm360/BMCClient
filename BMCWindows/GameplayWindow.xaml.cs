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
        private ObservableCollection<String> _Players { get; set; } = new ObservableCollection<String>();
        private ObservableCollection<Player> Player1 { get; set; } = new ObservableCollection<Player>();
        private ObservableCollection<Player> Player2 { get; set; } = new ObservableCollection<Player>();
        private int[,] Matrix { get; set; }

        public GameplayWindow(LobbyDTO lobby, ObservableCollection<String> Players)
        {
            InitializeComponent();
            _lobby = lobby;
            _Players = Players;
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            Player1Cards.ItemsSource = LoadPlayer1List();
            if(_lobby.Host == currentPlayer.Username)
            {
                Player2Cards.ItemsSource = LoadHost();
            }
            else
            {
                Player2Cards.ItemsSource = LoadCurrentPlayerList();
            }
            textBlockGameName.Text = _lobby.Name;

            Matrix = new int[5, 3]
       {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 },
            { 9, 10, 11 },
            { 11, 12, 13 }
       };

        }

        private ObservableCollection<Player> LoadPlayer1List()
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                foreach (var player in _Players)
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

        private ObservableCollection<Player> LoadHost()
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                
                
                    if (currentPlayer.Username == _lobby.Host)
                    {
                        var playerProfilePicture = profileProxy.GetProfileImage(currentPlayer.Username);
                        if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                        {
                            BitmapImage image = ConvertByteArrayToImage(playerProfilePicture.ImageData);
                            playerList.Add(new Player { Username = _lobby.Host, ProfilePicture = image });
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

        public ObservableCollection<int> FlatMatrix
        {
            get
            {
                var flatList = new ObservableCollection<int>();
                for (int i = 0; i < Matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < Matrix.GetLength(1); j++)
                    {
                        flatList.Add(Matrix[i, j]);
                    }
                }
                return flatList;
            }
        }

        private void SetCard(object sender, MouseButtonEventArgs e)
        {
            var cell = sender as Border;
            if (cell != null)
            {
                var textBlock = cell.Child as TextBlock;
                if (textBlock != null)
                {
                    var value = textBlock.Text;
                    MessageBox.Show($"Seleccionaste la celda con el valor: {value}");
                }
                var row = Grid.GetRow(cell);  
                var column = Grid.GetColumn(cell);  

                MessageBox.Show($"Posición de la celda seleccionada: Fila {row}, Columna {column}");
            }
        }


    }
}


