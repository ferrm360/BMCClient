using BMCWindows.DTOs;
using BMCWindows.GameplayServer;
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
    public partial class GameplayWindow : Page, GameplayServer.IGameServiceCallback
    {
        private LobbyDTO _lobby;
        private ObservableCollection<String> _Players { get; set; } = new ObservableCollection<String>();
        private ObservableCollection<Player> Player1 { get; set; } = new ObservableCollection<Player>();
        private ObservableCollection<Player> Player2 { get; set; } = new ObservableCollection<Player>();
        private ObservableCollection<Card> CatCards { get; set; }
        private ObservableCollection<Card> DogCards { get; set; }
        private string selectedCard = null;
        private int[,] Matrix { get; set; }
        private ObservableCollection<Card> Player1GameCards { get; set; }
        private ObservableCollection<Card> Player2GameCards { get; set; }
        private string selectedCardName;
        private int selectedCardLife;


        public GameplayWindow(LobbyDTO lobby, ObservableCollection<String> Players)
        {
            InitializeComponent();
            _lobby = lobby;
            _Players = Players;
            this.DataContext = this;
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
            textBlockGameName.Text = lobby.Name;

            Matrix = new int[5, 3];
       
            Player1GameCards = new ObservableCollection<Card>();
            Player2GameCards = new ObservableCollection<Card>();

            InitializeBoard();
            //InitializeCards(currentPlayer.Username);

            if (lobby.Host == currentPlayer.Username)
            {
                textBlockOneLifeCardName.Text = "Cat1Life";
                textBlockOneLifeCardLife.Text = "1";

                textBlockAnotherOneLifeCardName.Text = "CatAnother1Life";
                textBlockAnotherOneLifeCardLife.Text = "1";

                textBlockTwoLivesCardName.Text = "Cat2Lives";
                textBlockTwoLivesCardLives.Text = "2";

                textBlockAnotherTwoLivesCardName.Text = "CatAnother2Lives";
                textBlockAnotherTwoLivesCardLives.Text = "2";

                textBlockThreeLivesCardName.Text = "Cat3Lives";
                textBlockThreeLivesCardLives.Text = "3";


            }
            else
            {
                textBlockOneLifeCardName.Text = "Dog1Life";
                textBlockOneLifeCardLife.Text = "1";

                textBlockAnotherOneLifeCardName.Text = "DogAnother1Life";
                textBlockAnotherOneLifeCardLife.Text = "1";

                textBlockTwoLivesCardName.Text = "Dog2Lives";
                textBlockTwoLivesCardLives.Text = "2";

                textBlockAnotherTwoLivesCardName.Text = "DogAnother2Lives";
                textBlockAnotherTwoLivesCardLives.Text = "2";

                textBlockThreeLivesCardName.Text = "Dog3Lives";
                textBlockThreeLivesCardLives.Text = "3";

            }

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

        private void InitializeBoard()
        {
            BoardGrid.Children.Clear();

            int buttonCount = 0;

            for (int row = 0; row < 5; row++)  
            {
                for (int col = 0; col < 3; col++)  
                {
                    Button cellButton = new Button
                    {
                        Background = Brushes.Transparent,
                        Content = $"{row},{col}",
                        Margin = new Thickness(1)
                    };

                    cellButton.Click += TestButton_Click;

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    cellButton.Click += AssignOrRemoveCard; 
                    BoardGrid.Children.Add(cellButton); 
                    buttonCount++;

                    
                    

                    
                }
            }
        }

        

        


        private void AssignCard(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton != null)
            {
                int row = Grid.GetRow(clickedButton);
                int col = Grid.GetColumn(clickedButton);

                Console.WriteLine($"Botón clickeado en Fila {row}, Columna {col}");

                UpdateCardInMatrix(row, col);

                clickedButton.Content = $"{selectedCardName} - Vida: {selectedCardLife}";
            } 
        }

        private void AssignOrRemoveCard(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            if (clickedButton != null)
            {
                var assignedCard = clickedButton.Content as Card;

                if (assignedCard != null)
                {
                    Console.WriteLine($"Carta {assignedCard.Name} eliminada de la posición ({Grid.GetRow(clickedButton)}, {Grid.GetColumn(clickedButton)})");

                    clickedButton.Content = null;
                }
                else
                {
                    if (selectedCard != null)
                    {
                        var cardData = GetCardData(selectedCard); 
                        var newCard = new Card { Name = cardData.Name, Life = cardData.Life };

                        RemoveCardFromOtherButtons(newCard);

                        clickedButton.Content = newCard;

                        Console.WriteLine($"Carta {newCard.Name} colocada en la posición ({Grid.GetRow(clickedButton)}, {Grid.GetColumn(clickedButton)})");
                    }
                }
            }
        }

        private void RemoveCardFromOtherButtons(Card cardToRemove)
        {
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    var currentCard = button.Content as Card;

                    if (currentCard != null && currentCard.Name == cardToRemove.Name)
                    {
                        button.Content = null;
                        Console.WriteLine($"Carta {cardToRemove.Name} eliminada de la posición ({Grid.GetRow(button)}, {Grid.GetColumn(button)})");
                    }
                }
            }
        }

            private void ManageCards(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            if (clickedButton != null)
            {
                var assignedCard = clickedButton.Content as Card;

                if (assignedCard != null)
                {
                    Console.WriteLine($"Carta {assignedCard.Name} eliminada de la posición ({Grid.GetRow(clickedButton)}, {Grid.GetColumn(clickedButton)})");

                    clickedButton.Content = null;
                }
                else
                {
                    if (selectedCard != null)
                    {
                        var cardData = GetCardData(selectedCard); 
                        var newCard = new Card { Name = cardData.Name, Life = cardData.Life };

                        clickedButton.Content = newCard;

                        Console.WriteLine($"Carta {newCard.Name} colocada en la posición ({Grid.GetRow(clickedButton)}, {Grid.GetColumn(clickedButton)})");
                    }
                }
            }
        }

        private void UpdateCardInMatrix(int row, int col)
        {
            Matrix[row, col] = selectedCardLife;  

            Console.WriteLine($"Carta {selectedCardName} colocada en la posición ({row}, {col}) con vida {selectedCardLife}");
        }



        private void SelectCard(object sender, MouseButtonEventArgs e)
        {
            var card = sender as Border;
            if (card != null)
            {
                var stackPanel = card.Child as StackPanel;
                if (stackPanel != null)
                {
                    var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null)
                    {
                        selectedCard = textBlock.Text;
                        var cardData = GetCardData(selectedCard); 
                        selectedCardName = cardData.Name; 
                        selectedCardLife = cardData.Life;  

                        Console.WriteLine($"Carta seleccionada: {selectedCardName}, Vida: {selectedCardLife}");
                    }
                }

                if (card.Tag?.ToString() == "Clicked")
                {
                    card.Tag = null;
                }
                else
                {
                    card.Tag = "Clicked";
                }
            }
        }

        private (string Name, int Life) GetCardData(string cardName)
        {
            var cardData = new Dictionary<string, (string, int)>

            {
                { "Cat1Life", ("Cat1", 1) },
                { "CatAnother1Life", ("Cat1", 1)},
                { "Cat2Lives", ("Cat2", 2) },
                {"CatAnother2Lives", ("Cat2", 2) },
                { "Cat3Lives", ("Cat3", 3) },
                { "Dog1Life", ("Dog1", 1) },
                {"DogAnother1Life", ("Dog1", 1) },
                { "Dog2Lives", ("Dog2", 2) },
                {"DogAnother2Lives", ("Dog2", 2) },
                { "Dog3Lives", ("Dog3", 3) } 
            };

            return cardData.ContainsKey(cardName) ? cardData[cardName] : ("", 0);
        }

        private void RemoveCardFromGrid(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton.Content != null)
            {
                clickedButton.Content = null;
            }
        }

        private void SendCardsPosition(string lobbyId, string player, List<int> flatMatrix)
        {
            int rows = 5; 
            int cols = 3; 
            int[][] matrix = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                matrix[i] = flatMatrix.Skip(i * cols).Take(cols).ToArray();
            }

            foreach (var row in matrix)
            {
                Console.WriteLine(string.Join(", ", row));
            }
        }

        private int CountCardsInGrid()
        {
            int cardCount = 0;

            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    if (button.Content != null)
                    {
                        cardCount++;
                    }
                }
            }

            return cardCount;
        }

        private bool CheckIfFiveCardsArePlaced()
        {
            int placedCards = CountCardsInGrid();

            if (placedCards == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AcceptCardsPosition(object sender, RoutedEventArgs e)
        {
            List<int> flatMatrix = new List<int>();
            if (!CheckIfFiveCardsArePlaced())             
            {
                MessageBox.Show("Coloque todas sus cartas en el tablero");
                return;
            }

            try
            {
                var board = ConvertMatrixToGameBoardDTO(Matrix);

                Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();

                InstanceContext context = new InstanceContext(this);
                GameplayServer.GameServiceClient proxy = new GameplayServer.GameServiceClient(context);

                var response = proxy.SubmitInitialMatrix(_lobby.LobbyId, currentPlayer.Username, board);

                if (!response.IsSuccess)
                {
                    MessageBox.Show($"Error al enviar el tablero: {response.ErrorKey}");
                }
                else
                {
                    MessageBox.Show("Tablero enviado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el tablero: {ex.Message}");
            }


        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string content = clickedButton.Content.ToString();
                MessageBox.Show($"Botón clickeado: {content}");
            }
        }

        private GameplayServer.GameBoardDTO ConvertMatrixToGameBoardDTO(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            List<int> data = new List<int>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data.Add(matrix[i, j]);
                }
            }
            return new GameplayServer.GameBoardDTO
            {
                Rows = rows,
                Columns = cols,
                Data = data.ToArray()
            };
        }




        public void OnGameStarted()
        {
            throw new NotImplementedException();
        }
    }
}


