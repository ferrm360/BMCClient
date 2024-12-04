using BMCWindows.DTOs;
using BMCWindows.FriendServer;
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
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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
        public event EventHandler AttackProcessed;


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
                    Console.WriteLine($"Turno actualizado: {_currentPlayer.Username} es {_isPlayerTurn}");
                    _isPlayerTurn = isPlayerTurn;
                    if (_isPlayerTurn)
                    {
                        DynamicTurnTextBlock.Text = $"¡Es tu turno {_currentPlayer.Username}!";
                    }
                    else
                    {
                        DynamicTurnTextBlock.Text = "Espera tu turno.";
                    }
                });
            };

            _callbackHandler.OnGameOverEvent += () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Has ganado {_currentPlayer.Username}!" );
                });
            };


            var cardInitializer = new CardInitializer();
            HostAvailableAttackCards = cardInitializer.InitializeHostCards();
            GuestAvailableAttackCards = cardInitializer.InitializeGuestCards();

            CurrentPlayerDeck = new Dictionary<string, AttackCard>();
            RemainingHostCards = new Dictionary<string, AttackCard>(HostAvailableAttackCards);
            RemainingGuestCards = new Dictionary<string, AttackCard>(GuestAvailableAttackCards);



            _boardPlayerManager = new BoardPlayerManager(OnCellClickOwnBoard);
            _boardPlayerManager.InitializePlayerBoard(PlayerBoardGrid, _playerMatrixLife);

            _boardEnemyManager = new BoardEnemyManager(OnCellClickEnemyBoard);
            _boardEnemyManager.InitializeEnemyBoard(EnemyBoardGrid);

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
            stackPanelPlayerAttackCards.Children.Clear();

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
                Background = new SolidColorBrush(Color.FromRgb(255, 250, 250)),
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
                Width = 100,
                Height = 130
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

            stackPanelPlayerAttackCards.Children.Add(newCard);
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

                        Console.WriteLine($"Carta seleccionada: {selectedCardName}, Ataque: {selectedCardAttackLevel}");
                    }
                    else
                    {
                        Console.WriteLine("Carta no encontrada.");
                    }
                }
                else
                {
                    Console.WriteLine("No se pudo obtener el nombre de la carta.");
                }
            }
        }


        private AttackCard GetCardData(string cardName, string currentPlayerUsername)
        {
            if (string.IsNullOrEmpty(cardName))
            {
                Console.WriteLine("Carta vacía");
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
                Console.WriteLine("No se encontraron las cartas");
            }

            if (SelectedAttackCardDeck.ContainsKey(cardName))
            {
                return SelectedAttackCardDeck[cardName];
            }

            return null;
        }


        private void GetRandomCard(object sender, RoutedEventArgs e)
        {
            Server.PlayerDTO currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();
            if (_lobby.Host == currentPlayer.Username)
            {
                AssignAttackCards(RemainingHostCards, 1);
                var hostAvailableCards = HostAvailableAttackCards.Values.ToList();
                int numCards = hostAvailableCards.Count;
                AddCardToPanel(numCards - 1, hostAvailableCards[numCards - 1]);
            }
            else
            {
                AssignAttackCards(RemainingGuestCards, 1);
                var guestAvailableCards = GuestAvailableAttackCards.Values.ToList();
                int numGuestAttackCards = guestAvailableCards.Count;
                AddCardToPanel(numGuestAttackCards - 1, guestAvailableCards[numGuestAttackCards - 1]);
            }
        }

        private void OnCellClickOwnBoard(int row, int col)
        {
            MessageBox.Show($"Has clickeado en la celda: ({row}, {col}) de tu propio tablero");
        }

        private void OnCellClickEnemyBoard(int row, int col)
        {
            if (!_isPlayerTurn)
            {
                DynamicTurnTextBlock.Text = "Aun no es tu turno.";
                return;
            }

            GameplayServer.AttackPositionDTO attackPositionDTO = new GameplayServer.AttackPositionDTO();
            attackPositionDTO.X = row;
            attackPositionDTO.Y = col;

            var result = _proxy.Attack(_lobby.LobbyId, _currentPlayer.Username, attackPositionDTO);
            DynamicTalkTextBlock.Text = ($"¡Has atacando!");
            DynamicTurnTextBlock.Text = ($"¡Espera tu turno!");
        }

        private async Task OnAttackReceivedHandlerAsync(AttackPositionDTO attackPosition)
        {
            if (_playerMatrixName[attackPosition.X, attackPosition.Y] == null)
            {
                DynamicTalkTextBlock.Text = "¡Ups! eso estuvo cerca :D";

            }
            else
            {
                DynamicTalkTextBlock.Text = "!NO! :c";
                _playerMatrixLife[attackPosition.X, attackPosition.Y] -= 1;

                var deadCell = _gameRules.CheckForDeadCell();
                if (deadCell.IsDead)
                {
                    string deadCellName = _playerMatrixName[attackPosition.X, attackPosition.Y];
                    var deadCardData = CardManager.GetCardData(deadCellName);

                    if (!string.IsNullOrEmpty(deadCardData.Name))
                    {
                        _boardPlayerManager.UpdateCellToDead(PlayerBoardGrid, attackPosition.X, attackPosition.Y, deadCardData.CardImage);
                    }


                    _playerMatrixName[deadCell.Row, deadCell.Col] = null;
                }
            }

            _boardPlayerManager.InitializePlayerBoard(PlayerBoardGrid, _playerMatrixLife);

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

                MessageBox.Show($"Has perdido {_currentPlayer.Username}");
            }
        }

        private async void DebugGameOver_Click(object sender, RoutedEventArgs e)
        {
            var response = await _proxy.NotifyGameOverAsync(_lobby.LobbyId, _currentPlayer.Username);

            if (response.IsSuccess)
            {
                MessageBox.Show("NotifyGameOverAsync se llamó correctamente.");
            }
            else
            {
                MessageBox.Show($"Error en NotifyGameOverAsync: {response.ErrorKey}");
            }
        }

        private void FirstTurn()
        {
            if (_currentPlayer.Username == _lobby.Host)
            {
                DynamicTurnTextBlock.Text = "Tienes el primer turno";
                _isPlayerTurn = true;
            }
            else
            {
                DynamicTurnTextBlock.Text = "No tienes el primer turno, espera tu turno";
                _isPlayerTurn = false;
            }
        }
    }
}
