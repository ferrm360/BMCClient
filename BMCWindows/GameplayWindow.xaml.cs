using BMCWindows.DTOs;
using BMCWindows.GameplayPage;
using BMCWindows.GameplayServer;
using BMCWindows.LobbyServer;
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
    /// Lógica de interacción para GameplayWindow.xaml
    /// </summary>
    public partial class GameplayWindow : Page
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
        private GameCallbackHandler _callbackHandler;
        private GameServiceClient _proxy;
        private int _playersReadyCount = 0; // Track players ready



        public GameplayWindow(LobbyDTO lobby, ObservableCollection<String> Players)
        {

            _callbackHandler = new GameCallbackHandler();
            _callbackHandler.OnGameStartedEvent += HandleGameStarted;
            _callbackHandler.OnPlayerReadyEvent += HandlePlayerReady;

            var context = new System.ServiceModel.InstanceContext(_callbackHandler);
            _proxy = new GameplayServer.GameServiceClient(context);

            InitializeComponent();
            _lobby = lobby;
            _Players = Players;
            this.DataContext = this;
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();
            Player1Cards.ItemsSource = LoadPlayer1List();
            if (_lobby.Host == currentPlayer.Username)
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

                string imagePath = "Images/IñakiCard.png";
                BitmapImage imageOneLifeCard = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                imageCardOneLife.Source = imageOneLifeCard;
                textBlockOneLifeCardName.Text = "Cat1Life";
                textBlockOneLifeCardLife.Text = "1";



                textBlockAnotherOneLifeCardName.Text = "CatAnother1Life";
                textBlockAnotherOneLifeCardLife.Text = "1";
                imageAnotherOneLifeCard.Source = imageOneLifeCard;

                textBlockTwoLivesCardName.Text = "Cat2Lives";
                textBlockTwoLivesCardLives.Text = "2";
                string imagePathTwoLives = "Images/michideltoroCard.png";
                BitmapImage imageTwoLives = new BitmapImage(new Uri(imagePathTwoLives, UriKind.Relative));
                imageTwoLivesCard.Source = imageTwoLives;

                textBlockAnotherTwoLivesCardName.Text = "CatAnother2Lives";
                textBlockAnotherTwoLivesCardLives.Text = "2";
                imageAnotherTwoLivesCard.Source = imageTwoLives;

                textBlockThreeLivesCardName.Text = "Cat3Lives";
                textBlockThreeLivesCardLives.Text = "3";
                string imagePathThreeLives = "Images/coca3litrosCard.png";
                BitmapImage imageThreeLives = new BitmapImage(new Uri(imagePathThreeLives, UriKind.Relative));
                imageThreeLivesCard.Source = imageThreeLives;


            }
            else
            {

                string imagePath = "Images/HuahuaCard.png";
                BitmapImage imageOneLifeCard = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                imageCardOneLife.Source = imageOneLifeCard;
                textBlockOneLifeCardName.Text = "Dog1Life";
                textBlockOneLifeCardLife.Text = "1";

                textBlockAnotherOneLifeCardName.Text = "DogAnother1Life";
                textBlockAnotherOneLifeCardLife.Text = "1";
                imageAnotherOneLifeCard.Source = imageOneLifeCard;


                textBlockTwoLivesCardName.Text = "Dog2Lives";
                textBlockTwoLivesCardLives.Text = "2";
                string imagePathTwoLives = "Images/AnasofCard.png";
                BitmapImage imageTwoLives = new BitmapImage(new Uri(imagePathTwoLives, UriKind.Relative));
                imageTwoLivesCard.Source = imageTwoLives;

                textBlockAnotherTwoLivesCardName.Text = "DogAnother2Lives";
                textBlockAnotherTwoLivesCardLives.Text = "2";
                imageAnotherTwoLivesCard.Source = imageTwoLives;

                textBlockThreeLivesCardName.Text = "Dog3Lives";
                textBlockThreeLivesCardLives.Text = "3";
                string imagePathThreeLives = "Images/ChilaquilCard.png";
                BitmapImage imageThreeLives = new BitmapImage(new Uri(imagePathThreeLives, UriKind.Relative));
                imageThreeLivesCard.Source = imageThreeLives;
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
            this.NavigationService.Navigate(new LobbiesWindow());
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

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button cellButton = new Button
                    {
                        Background = Brushes.Transparent,
                        //Content = $"{row},{col}",
                        Margin = new Thickness(1)
                    };



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
                int row = Grid.GetRow(clickedButton);
                int col = Grid.GetColumn(clickedButton);

                if (clickedButton.Content == null && !string.IsNullOrEmpty(selectedCardName))
                {
                    var cardData = GetCardData(selectedCard);
                    if (cardData.Name != null)
                    {
                        StackPanel cardPanel = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Children =
                    {
                        new Image { Source = cardData.CardImage, Width = 60, Height = 90 },
                        new TextBlock { Text = cardData.Name, HorizontalAlignment = HorizontalAlignment.Center }
                    }
                        };

                        clickedButton.Content = cardPanel;

                        Matrix[row, col] = cardData.Life;

                        Console.WriteLine($"Carta {cardData.Name} colocada en ({row}, {col}) con vida {cardData.Life}");
                    }
                }
                else
                {
                    clickedButton.Content = null;
                    Matrix[row, col] = 0;

                    Console.WriteLine($"Carta eliminada de ({row}, {col})");
                }
            }
        }


        private void RemoveCardFromOtherButtons(Card cardToRemove)
        {
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    var currentCardPanel = button.Content as StackPanel;

                    if (currentCardPanel != null)
                    {
                        var textBlock = currentCardPanel.Children.OfType<TextBlock>().FirstOrDefault();
                        if (textBlock != null && textBlock.Text == cardToRemove.Name)
                        {
                            button.Content = null;
                            Console.WriteLine($"Carta {cardToRemove.Name} eliminada de la posición ({Grid.GetRow(button)}, {Grid.GetColumn(button)})");
                        }
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
            if (!string.IsNullOrEmpty(selectedCardName) && selectedCardLife > 0)
            {
                Matrix[row, col] = selectedCardLife;
                Console.WriteLine($"Matriz actualizada: [{row}, {col}] = {selectedCardLife}");
            }
            else
            {
                Console.WriteLine($"Error al actualizar la matriz en ({row}, {col}): Carta no válida");
            }
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
            }
        }


        private (string Name, int Life, BitmapImage CardImage) GetCardData(string cardName)
        {
            var cardData = new Dictionary<string, (string, int, BitmapImage)>
            {
                { "Cat1Life", ("Cat1", 1, new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png"))) },
                { "CatAnother1Life", ("Cat1.1", 1, new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")))},
                { "Cat2Lives", ("Cat2", 2, new BitmapImage(new Uri("pack://application:,,,/Images/michideltoroCard.png"))) },
                {"CatAnother2Lives", ("Cat2.1", 2, new BitmapImage(new Uri("pack://application:,,,/Images/michideltoroCard.png"))) },
                { "Cat3Lives", ("Cat3", 3, new BitmapImage(new Uri("pack://application:,,,/Images/coca3litrosCard.png"))) },
                { "Dog1Life", ("Dog1", 1, new BitmapImage(new Uri("pack://application:,,,/Images/HuahuaCard.png"))) },
                {"DogAnother1Life", ("Dog1.1", 1, new BitmapImage(new Uri("pack://application:,,,/Images/HuahuaCard.png"))) },
                { "Dog2Lives", ("Dog2", 2, new BitmapImage(new Uri("pack://application:,,,/Images/AnasofCard.png"))) },
                {"DogAnother2Lives", ("Dog2.1", 2, new BitmapImage(new Uri("pack://application:,,,/Images/AnasofCard.png"))) },
                { "Dog3Lives", ("Dog3", 3, new BitmapImage(new Uri("pack://application:,,,/Images/ChilaquilCard.png"))) }
            };
            return cardData.ContainsKey(cardName) ? cardData[cardName] : ("", 0, null);
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

        private async void AcceptCardsPosition(object sender, RoutedEventArgs e)
        {
            // Depuración: Imprimir matriz antes de enviar
            string matrixDebug = "";
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    matrixDebug += Matrix[i, j] + " ";
                }
                matrixDebug += Environment.NewLine;
            }
            MessageBox.Show($"Matriz actual:\n{matrixDebug}", "Depuración");

            if (!CheckIfFiveCardsArePlaced())
            {
                MessageBox.Show("Coloque todas sus cartas en el tablero");
                return;
            }

            try
            {
                Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().getPlayerUserData();

                var response = await _proxy.MarkPlayerReadyAsync(_lobby.LobbyId, currentPlayer.Username);

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



        private void FillGridWithImage(object sender, RoutedEventArgs e)
        {
            string imagePath = "Images/CardBack.png";
            BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.Relative));

            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Button button)
                {
                    button.Content = new Image { Source = image, Width = 60, Height = 90 };
                }
            }
            Console.WriteLine("Todos los botones del grid han sido llenados con la imagen.");
        }

        private void HandlePlayerReady(string player)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _playersReadyCount++;
                    MessageBox.Show($"El jugador {player} está listo. ({_playersReadyCount}/{_Players.Count})");
                });
            });
        }

        private void HandleGameStarted()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Todos los jugadores están listos. Iniciando el juego...");
                    NavigateToGameWindow();
                });
            });
        }

        private void NavigateToGameWindow()
        {
            var gameWindow = new GameplayAttackWindow(_lobby, Matrix);
            NavigationService.Navigate(gameWindow);
        }
    }
}