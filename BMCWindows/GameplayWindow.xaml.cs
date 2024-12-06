using BMCWindows.DTOs;
using BMCWindows.GameplayBoard;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BMCWindows
{
    /// <summary>
    /// Lógica de interacción para GameplayWindow.xaml
    /// </summary>
    public partial class GameplayWindow : Page
    {
        private LobbyDTO _lobby;
        private ObservableCollection<String> Players { get; set; } = new ObservableCollection<String>();
        private string selectedCard = null;
        private int[,] MatrixCardLife { get; set; }
        private String[,] MatrixCardName { get; set; }
        private ObservableCollection<Card> Player1GameCards { get; set; }
        private ObservableCollection<Card> Player2GameCards { get; set; }
        private string selectedCardName;
        private int selectedCardLife;
        private GameCallbackHandler _callbackHandler;
        private GameServiceClient _proxy;
        private int _playersReadyCount = 0;
        private bool _isReady = false;


        public GameplayWindow(LobbyDTO lobby, ObservableCollection<String> Players)
        {
            _callbackHandler = new GameCallbackHandler();
            _callbackHandler.OnGameStartedEvent += HandleGameStarted;
            _callbackHandler.OnPlayerReadyEvent += HandlePlayerReady;

            var context = new System.ServiceModel.InstanceContext(_callbackHandler);
            _proxy = new GameplayServer.GameServiceClient(context);

            InitializeComponent();
            _lobby = lobby;
            this.Players = Players;
            this.DataContext = this;
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
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

            MatrixCardLife = new int[5, 3];
            MatrixCardName = new String[5, 3];

            Player1GameCards = new ObservableCollection<Card>();
            Player2GameCards = new ObservableCollection<Card>();

            InitializeBoard();

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
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                foreach (var player in Players)
                {
                    if (currentPlayer.Username != player)
                    {
                        var playerProfilePicture = profileProxy.GetProfileImage(player);
                        if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                        {
                            BitmapImage image = ImageConvertor.ConvertByteArrayToImage(playerProfilePicture.ImageData);
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
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                if (currentPlayer.Username == _lobby.Host)
                {
                    var playerProfilePicture = profileProxy.GetProfileImage(currentPlayer.Username);
                    if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                    {
                        BitmapImage image = ImageConvertor.ConvertByteArrayToImage(playerProfilePicture.ImageData);
                        playerList.Add(new Player { Username = _lobby.Host, ProfilePicture = image });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
            return playerList;
        }

        private ObservableCollection<Player> LoadCurrentPlayerList()
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
            ProfileServer.ProfileServiceClient profileProxy = new ProfileServer.ProfileServiceClient();
            ObservableCollection<Player> playerList = new ObservableCollection<Player>();

            try
            {
                var playerProfilePicture = profileProxy.GetProfileImage(currentPlayer.Username);
                if (playerProfilePicture.ImageData != null && playerProfilePicture.ImageData.Length > 0)
                {
                    BitmapImage image = ImageConvertor.ConvertByteArrayToImage(playerProfilePicture.ImageData);
                    playerList.Add(new Player { Username = currentPlayer.Username, ProfilePicture = image });
                }
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GeneralException");
            }
            return playerList;
        }

        private void LeaveGame(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new LobbiesWindow());
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
                    var cardData = CardManager.GetCardData(selectedCard);
                    var newCard = new Card { Name = cardData.Name, Life = cardData.Life, CardImage = cardData.CardImage };
                    if (cardData.Name != null)
                    {
                        RemoveCardFromOtherButtons(newCard);

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
                        MatrixCardLife[row, col] = cardData.Life;
                        MatrixCardName[row, col] = selectedCard;

                    }
                }
                else
                {
                    clickedButton.Content = null;
                    MatrixCardLife[row, col] = 0;
                    MatrixCardName[row, col] = null;

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

                    clickedButton.Content = null;
                }
                else
                {
                    if (selectedCard != null)
                    {
                        var cardData = CardManager.GetCardData(selectedCard);
                        var newCard = new Card { Name = cardData.Name, Life = cardData.Life };

                        clickedButton.Content = newCard;

                    }
                }
            }
        }

        private void UpdateCardInMatrix(int row, int col)
        {
            if (!string.IsNullOrEmpty(selectedCardName) && selectedCardLife > 0)
            {
                MatrixCardLife[row, col] = selectedCardLife;
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
                        var cardData = CardManager.GetCardData(selectedCard);
                        selectedCardName = cardData.Name;
                        selectedCardLife = cardData.Life;

                    }
                }
            }
        }

        private void RemoveCardFromGrid(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton.Content != null)
            {
                clickedButton.Content = null;
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
            if (_isReady) {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.PlayerNotReady");
                return;
            }

            if (!CheckIfFiveCardsArePlaced())
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.GameboardCardsNotPlaced");
                return;
                
            }

            try
            {
                Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();

                var response = await _proxy.MarkPlayerReadyAsync(_lobby.LobbyId, currentPlayer.Username);

                if (!response.IsSuccess)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage(response.ErrorKey);
                }
                else
                {
                    _isReady = true;
                    string infoMessage = Properties.Resources.ResourceManager.GetString("Info.CardsPostionSent");
                    MessageBox.Show(infoMessage);
                }
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.CantSummitMatrix");
            }
        }

        private void HandlePlayerReady(string player)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _playersReadyCount++;
                });
            });
        }

        private void HandleGameStarted()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    string infoMessage = Properties.Resources.ResourceManager.GetString("Info.GameStarted");
                    MessageBox.Show(infoMessage);
                    NavigateToGameWindow();
                });
            });
        }

        private void NavigateToGameWindow()
        {
            var gameWindow = new GameplayAttackWindow(_callbackHandler, _proxy ,_lobby, MatrixCardLife, MatrixCardName);
            NavigationService.Navigate(gameWindow);
        }
    }
}