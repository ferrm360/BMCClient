using BMCWindows.DTOs;
using BMCWindows.GameOver;
using BMCWindows.GameplayAttack;
using BMCWindows.GameplayAttack.Rules;
using BMCWindows.GameplayBoard;
using BMCWindows.GameplayServer;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayPage
{
    public partial class GameplayAttackWindow : Page
    {
        private LobbyDTO _lobby;
        private int[,] _playerMatrixLife;
        private readonly String[,] _playerMatrixName;
        private Dictionary<string, AttackCard> HostAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> GuestAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> CurrentPlayerDeck { get; set; }
        private Dictionary<string, AttackCard> RemainingHostCards { get; set; }
        private Dictionary<string, AttackCard> RemainingGuestCards { get; set; }
        private string selectedCard = null;
        private string selectedCardName;
        private int selectedCardAttackLevel;
        private BitmapImage selectedCardImage;
        private GameCallbackHandler _callbackHandler;
        private GameServiceClient _proxy;
        private BoardPlayerManager _boardPlayerManager;
        private BoardEnemyManager _boardEnemyManager;
        private Server.PlayerDTO _currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
        private bool _isPlayerTurn;
        private GameRules _gameRules;
        private readonly object _lockObject = new object();

        public GameplayAttackWindow(GameCallbackHandler gameCallbackHandler, GameServiceClient proxy, LobbyDTO lobby, int[,] playerMatrixLife, String[,] playerMatrixName)
        {
            InitializeComponent();
            _lobby = lobby;
            _playerMatrixLife = playerMatrixLife;
            _playerMatrixName = playerMatrixName;
            _proxy = proxy;
            _callbackHandler = gameCallbackHandler;

            _callbackHandler.OnAttackReceivedEvent += (attackPositionDTO) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _ = OnAttackReceivedHandlerAsync(attackPositionDTO);
                });
            };

            _callbackHandler.OnTurnChangedEvent += (isPlayerTurn) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _isPlayerTurn = isPlayerTurn;
                    if (_isPlayerTurn)
                    {
                        string turnText = string.Format(Properties.Resources.Info_Turn, _currentPlayer.Username);
                        TextBlockDynamicTurn.Text = turnText;
                    }
                    else
                    {
                        string errorTurnText = Properties.Resources.Error_WaitTurn;
                        TextBlockDynamicTurn.Text = errorTurnText;
                    }
                });
            };

            _callbackHandler.OnGameOverEvent += (loser) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    IncrementWin();
                    OpenGameOverPage(loser);
                });
            };

            _callbackHandler.OnCellDeadEvent += (CellDeadDTO) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    OnCellDeadReceivedHandler(CellDeadDTO);
                });
            };


            var cardInitializer = new CardInitializer();
            HostAvailableAttackCards = cardInitializer.InitializeHostCards();
            GuestAvailableAttackCards = cardInitializer.InitializeGuestCards();

            CurrentPlayerDeck = new Dictionary<string, AttackCard>();
            RemainingHostCards = new Dictionary<string, AttackCard>(HostAvailableAttackCards);
            RemainingGuestCards = new Dictionary<string, AttackCard>(GuestAvailableAttackCards);



            _boardPlayerManager = new BoardPlayerManager(OnCellClickOwnBoard);
            _boardPlayerManager.InitializePlayerBoard(GridPlayerBoard, _playerMatrixLife);

            _boardEnemyManager = new BoardEnemyManager(OnCellClickEnemyBoard);
            _boardEnemyManager.InitializeEnemyBoard(GridEnemyBoard);

            if (_lobby.Host == _currentPlayer.Username)
            {
                AssignAttackCards(RemainingHostCards, 5);
            }
            else
            {
                AssignAttackCards(RemainingGuestCards, 5);
            }

            ShowAssignedCards();
            FirstTurn();
            _gameRules = new GameRules(_playerMatrixLife, _playerMatrixName);
        }

        private void AssignAttackCards(Dictionary<string, AttackCard> AvailableCards, int numCardsPerPlayer)
        {
            Random random = new Random();
            List<string> attackCardKeys = AvailableCards.Keys.ToList();

            attackCardKeys = attackCardKeys.OrderBy(x => random.Next()).ToList();
            
                for (int i = 0; i < numCardsPerPlayer; i++)
                {
                    string selectedAttackCardKey = attackCardKeys[i];
                    AttackCard selectedAttackCard = AvailableCards[selectedAttackCardKey];

                    if (!CurrentPlayerDeck.ContainsKey(selectedAttackCard.Name))
                    {
                        CurrentPlayerDeck.Add(selectedAttackCard.Name, selectedAttackCard);

                        AvailableCards.Remove(selectedAttackCardKey);
                    }
                }
                foreach (var attackCard in CurrentPlayerDeck)
                {
                    Console.WriteLine(attackCard.Key);
                }
            

            
        }

        private void ShowAssignedCards()
        {
            StackPanelPlayerAttackCards.Children.Clear();

            int cardIndex = 0;
            foreach (var card in CurrentPlayerDeck.Values)
            {
                AddCardToPanel(cardIndex, card);
                cardIndex++;
            }
        }

        private void AddCardToPanel(int cardIndex, AttackCard attackCard)
        {
            var newCard = new Border
            {
                Width = 180,
                Height = 250,
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(Color.FromRgb(128, 184, 122)),
                Style = (Style)FindResource("ClickableCardStyle"),
                Margin = new Thickness(-30 - (5 * cardIndex), 10, 10, 10),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 5,
                    Color = Colors.Gray
                },
                DataContext = attackCard.Name
            };

            newCard.Tag = attackCard.Name;
            newCard.MouseLeftButtonDown += SelectCard;

            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var cardImage = new Image
            {
                Source = attackCard.CardImage,
                Width = 180,
                Height = 250
            };

            var textBlock = new TextBlock
            {
                Text = attackCard.Name,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };

            stackPanel.Children.Add(cardImage);
            stackPanel.Children.Add(textBlock);

            newCard.Child = stackPanel;

            StackPanelPlayerAttackCards.Children.Add(newCard);
        }

        private void RemoveCardByName(string cardName)
        {
            var container = StackPanelPlayerAttackCards;

            foreach (var item in container.Children)
            {
                var border = item as Border;
                if (border != null)
                {
                    var stackPanel = border.Child as StackPanel;
                    if (stackPanel != null)
                    {
                        var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                        if (textBlock != null && textBlock.Text == cardName)
                        {
                            container.Children.Remove(border);
                            return;
                        }
                    }
                }
            }
        }



        public int GetCardAttackLevel(string cardName, string player)
        {
            var deck = CurrentPlayerDeck;

            if (deck.ContainsKey(cardName))
            {
                return deck[cardName].AttackLevel;
            }
            return 0;
        }

        private void SelectCard(object sender, MouseButtonEventArgs e)
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();

            var card = sender as Border;
            if (card != null)
            {
                string cardName = card.Tag as string;

                if (!string.IsNullOrEmpty(cardName))
                {
                    var cardData = GetCardData(cardName, currentPlayer.Username);

                    if (cardData != null)
                    {
                        selectedCardName = cardData.Name;
                        selectedCardAttackLevel = cardData.AttackLevel;
                        selectedCardImage = cardData.CardImage;

                    }
                }
            }
        }


        private AttackCard GetCardData(string cardName, string currentPlayerUsername)
        {
            if (string.IsNullOrEmpty(cardName))
            {
                return null;
            }

            Dictionary<string, AttackCard> SelectedAttackCardDeck = new Dictionary<string, AttackCard>();

            if (_lobby.Host == currentPlayerUsername)
            {
                SelectedAttackCardDeck = HostAvailableAttackCards;
            }
            else if (_lobby.Host != currentPlayerUsername)
            {
                SelectedAttackCardDeck = GuestAvailableAttackCards;
            }
            else
            {
                return null;
            }

            if (SelectedAttackCardDeck.ContainsKey(cardName))
            {
                return SelectedAttackCardDeck[cardName];
            }

            return null;
        }


        private void GetRandomCard(object sender, RoutedEventArgs e)
        {
            lock (_lockObject)
            {
                Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
                Random random = new Random();

                if (_lobby.Host == currentPlayer.Username)
                {
                    AssignAttackCards(RemainingHostCards, 1);
                    var hostAvailableCards = RemainingHostCards.Values.ToList();

                    if (hostAvailableCards.Count > 0)
                    {
                        int randomIndex = random.Next(hostAvailableCards.Count);
                        AddCardToPanel(randomIndex, hostAvailableCards[randomIndex]);
                        RemainingHostCards.Remove(RemainingHostCards.ElementAt(randomIndex).Key);
                    }
                    else
                    {
                        RefillCards(RemainingHostCards, HostAvailableAttackCards);
                    }
                }
                else
                {
                    AssignAttackCards(RemainingGuestCards, 1);
                    var guestAvailableCards = RemainingGuestCards.Values.ToList();

                    if (guestAvailableCards.Count > 0)
                    {
                        int randomIndex = random.Next(guestAvailableCards.Count);
                        AddCardToPanel(randomIndex, guestAvailableCards[randomIndex]);
                        RemainingGuestCards.Remove(RemainingGuestCards.ElementAt(randomIndex).Key);
                    }
                    else
                    {
                        RefillCards(RemainingGuestCards, GuestAvailableAttackCards);
                    }
                }
            }
        }

        private void RefillCards(Dictionary<string, AttackCard> remainingCards, Dictionary<string, AttackCard> availableAttackCards)
        {
            foreach (var card in availableAttackCards)
            {
                if (!remainingCards.ContainsKey(card.Key)) 
                {
                    remainingCards.Add(card.Key, card.Value);
                }
            }

            if (remainingCards.Count > 0)
            {
                Random random = new Random();
                var refreshedCards = remainingCards.Values.ToList();
                int randomIndex = random.Next(refreshedCards.Count);
                AddCardToPanel(randomIndex, refreshedCards[randomIndex]);
                remainingCards.Remove(remainingCards.ElementAt(randomIndex).Key);
            }
        }



        private void OnCellClickOwnBoard(int row, int col)
        {
            string infoMessage = string.Format(Properties.Resources.Gameplay_Attack, row, col);
            MessageBox.Show(infoMessage);
        }

        private void OnCellClickEnemyBoard(int row, int col)
        {
            if (!_isPlayerTurn)
            {
                string errorTurnText = Properties.Resources.Error_WaitTurn;
                TextBlockDynamicTurn.Text = errorTurnText;
                return;
            }

            if (string.IsNullOrEmpty(selectedCardName))
            {
                string errorMessage = Properties.Resources.Error_NoCardSelected;
                MessageBox.Show(errorMessage);
                return;
            }

            Task.Run(() =>
             {
                 GameplayServer.AttackPositionDTO attackPositionDTO = new GameplayServer.AttackPositionDTO
                 {
                     X = row,
                     Y = col
                 };

                 var result = _proxy.Attack(_lobby.LobbyId, _currentPlayer.Username, attackPositionDTO);

                 if (!result.IsSuccess)
                 {
                     MessageBox.Show(result.ErrorKey);
                 }

                 Dispatcher.Invoke(() =>
                 {
                     RemoveCardByName(selectedCardName);
                     selectedCard = null;
                     selectedCardName = null;
                     selectedCardAttackLevel = 0;
                     selectedCardImage = null;

                     
                     string attackMessage = Properties.Resources.AttackMade.ToString();
                     TextBlockDynamicTalk.Text = attackMessage;
                     string errorTurnText = Properties.Resources.Error_WaitTurn;
                     TextBlockDynamicTurn.Text = errorTurnText;
                 });
             });
        }

        private async Task OnAttackReceivedHandlerAsync(AttackPositionDTO attackPosition)
        {
            if (_playerMatrixName[attackPosition.X, attackPosition.Y] == null)
            {
                string missedAttack = Properties.Resources.MissedAttack;
                TextBlockDynamicTalk.Text = missedAttack;

            }
            else
            {
                TextBlockDynamicTalk.Text = "!NO! :c";
                _playerMatrixLife[attackPosition.X, attackPosition.Y] -= 1;

                var deadCell = _gameRules.CheckForDeadCell();
                if (deadCell.IsDead)
                {


                    string deadCellName = _playerMatrixName[attackPosition.X, attackPosition.Y];
                    var deadCardData = CardManager.GetCardData(deadCellName);

                    if (!string.IsNullOrEmpty(deadCardData.Name))
                    {
                        _boardPlayerManager.UpdateCellToDead(GridPlayerBoard, attackPosition.X, attackPosition.Y, deadCardData.CardImage);
                    }


                    _ = Task.Run(() =>
                    {
                        CellDeadDTO cellDeadDTO = new CellDeadDTO
                        {
                            CardName = deadCellName,
                            Looser = _currentPlayer.Username,
                            LobbyId = _lobby.LobbyId,
                            X = attackPosition.X,
                            Y = attackPosition.Y
                        };

                        _proxy.NotifyCellDeadAsync(cellDeadDTO);

                    });

                    _playerMatrixName[deadCell.Row, deadCell.Col] = null;
                }
            }

            if (_gameRules.IsGameOver())
            {
                _ = Task.Run(() =>
                {
                    GameplayServer.OperationResponse responseNotify = _proxy.NotifyGameOver(_lobby.LobbyId, _currentPlayer.Username);

                    if (!responseNotify.IsSuccess)
                    {
                        MessageBox.Show(responseNotify.ErrorKey);
                    }
                });

                IncrementLosses();
                OpenGameOverPage(UserSessionManager.getInstance().GetPlayerUserData().Username);
            }
        }

        private void FirstTurn()
        {
            if (_currentPlayer.Username == _lobby.Host)
            {
                string turnText = Properties.Resources.FirstTurn.ToString();
                TextBlockDynamicTurn.Text = turnText;
                _isPlayerTurn = true;
            }
            else
            {
                string turnText = Properties.Resources.TurnInfo.ToString();
                TextBlockDynamicTurn.Text = turnText;
                _isPlayerTurn = false;
            }
        }

        private void OnCellDeadReceivedHandler(CellDeadDTO cellDeadDTO)
        {
            var deadCardData = CardManager.GetCardData(cellDeadDTO.CardName);

            if (!string.IsNullOrEmpty(deadCardData.Name))
            {
                _boardEnemyManager.UpdateEnemyCellToDead(GridEnemyBoard, cellDeadDTO.X, cellDeadDTO.Y, deadCardData.CardImage);
            }
        }

        private void OpenGameOverPage(string loser)
        {
            var gameOverWindow = new GameOverWindow(_lobby, loser);
            this.NavigationService?.Navigate(gameOverWindow);
        }

        private void CloseProxy()
        {
            ProxyManager.CloseGameServiceProxy(_proxy);
        }

        private void IncrementWin()
        {
            if (!UserSessionManager.getInstance().IsGuestUser())
            {
                using (PlayerScoreServer.PlayerScoresServiceClient proxyScores = new PlayerScoreServer.PlayerScoresServiceClient())
                {
                    var result = proxyScores.IncrementWins(_currentPlayer.Username);

                    if (!result.IsSuccess)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage(result.ErrorKey);
                    }
                }
            }
        }

        private void IncrementLosses()
        {
            if (!UserSessionManager.getInstance().IsGuestUser())
            {
                using (PlayerScoreServer.PlayerScoresServiceClient proxyScores = new PlayerScoreServer.PlayerScoresServiceClient())
                {
                    var result = proxyScores.IncrementLosses(_currentPlayer.Username);

                    if (!result.IsSuccess)
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage(result.ErrorKey);
                    }
                }
            }
        }
    }
}
