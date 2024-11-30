using BMCWindows.AttackServer;
using BMCWindows.DTOs;
using BMCWindows.GameplayAttack;
using BMCWindows.GameplayServer;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        private string _cardBackImagePath = "pack://application:,,,/Images/CardBack.png"; 
        private Dictionary<string, AttackCard> HostAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> GuestAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> CurrentPlayerDeck { get; set; }
        private Dictionary<string, AttackCard> RemainingHostCards { get; set; }
        private Dictionary<string, AttackCard> RemainingGuestCards { get; set; }
        private string selectedCard = null;
        private string selectedCardName;
        private int selectedCardAttackLevel;
        private BitmapImage selectedCardImage;
        private AttackCallbackHandler _callbackHandler;
        private AttackServiceClient _proxy;
        private BoardPlayerManager _boardPlayerManager;
        private BoardEnemyManager _boardEnemyManager;


        public GameplayAttackWindow(LobbyDTO lobby, int[,] playerMatrixLife, String[,] playerMatrixName)
        {
            InitializeComponent();
            _lobby = lobby;
            _playerMatrixLife = playerMatrixLife;
            _playerMatrixName = playerMatrixName;

            _callbackHandler = new AttackCallbackHandler();
            var context = new System.ServiceModel.InstanceContext(_callbackHandler);
            _proxy = new AttackServer.AttackServiceClient(context);

            HostAvailableAttackCards = new Dictionary<string, AttackCard>();    
            GuestAvailableAttackCards = new Dictionary<string, AttackCard>();
            CurrentPlayerDeck = new Dictionary<string, AttackCard>();
            RemainingHostCards = new Dictionary<string, AttackCard>();
            RemainingGuestCards = new Dictionary<string, AttackCard>();    
            Server.PlayerDTO currentPlayer = new Server.PlayerDTO();
            currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();


            var attackCallbackHandler = new AttackCallbackHandler();
            attackCallbackHandler.OnAttackReceivedEvent += OnAttackReceivedHandler;

            _boardPlayerManager = new BoardPlayerManager(OnCellClickOwnBoard);
            _boardPlayerManager.InitializePlayerBoard(PlayerBoardGrid, _playerMatrixLife);

            _boardEnemyManager = new BoardEnemyManager(OnCellClickEnemyBoard);
            _boardEnemyManager.InitializeEnemyBoard(EnemyBoardGrid);

            InitializeAvailableCards();
            if(_lobby.Host == currentPlayer.Username)
            {
                AssignAttackCards(RemainingHostCards,5);
            } 
            else
            {
                AssignAttackCards(RemainingGuestCards, 5);
            }

            ShowAssignedCards();
        }

        private void AssignAttackCards(Dictionary<string, AttackCard> AvailableCards,  int numCardsPerPlayer)
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

        private void InitializeAvailableCards()
        {
            HostAvailableAttackCards.Add("Llorona", new AttackCard { Name = "Llorona", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Charro negro", new AttackCard { Name = "Charro negro", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Chupacabras", new AttackCard { Name = "Chupacabras", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("La mano peluda", new AttackCard { Name = "La mano peluda", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("La muerte", new AttackCard { Name = "La muerte", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Olmeca", new AttackCard { Name = "Olmeca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Tolteca", new AttackCard { Name = "Tolteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Azteca", new AttackCard { Name = "Azteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Maya", new AttackCard { Name = "Maya", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Moixteca", new AttackCard { Name = "Mixteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Huitzilopochtli ", new AttackCard { Name = "Huitzilopochtli", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Quetzalcoatl", new AttackCard { Name = "Quetzalcoatl", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Tonatiuh", new AttackCard { Name = "Tonatiuh", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Chalchiuhtlicue", new AttackCard { Name = "Chalchiuhtlicue", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Xipe-Totec ", new AttackCard { Name = "Xipe-Totec", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("La huesuda", new AttackCard { Name = "La huesuda", AttackLevel = 5   , CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("El misterioso", new AttackCard { Name = "El misterioso", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("Demonio azul Jr.", new AttackCard { Name = "Demonio azul Jr.", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("El tirantes", new AttackCard { Name = "El tirantes", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });
            HostAvailableAttackCards.Add("El poliedro", new AttackCard { Name = "El poliedro", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png")) });

            foreach (var attackCard in HostAvailableAttackCards)
            {
                RemainingHostCards.Add(attackCard.Key, attackCard.Value);
            }

            GuestAvailableAttackCards.Add("Llorona", new AttackCard { Name = "Llorona", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Charron negro", new AttackCard { Name = "Charron negro", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Chupacabras", new AttackCard { Name = "Chupacabras", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("La mano peluda", new AttackCard { Name = "La mano peluda", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("La muerte", new AttackCard { Name = "La muerte", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Olmeca", new AttackCard { Name = "Olmeca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Tolteca", new AttackCard { Name = "Tolteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Azteca", new AttackCard { Name = "Azteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Maya", new AttackCard { Name = "Maya", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Moixteca", new AttackCard { Name = "Mixteca", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Huitzilopochtli ", new AttackCard { Name = "Huitzilopochtli", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Quetzalcoatl", new AttackCard { Name = "Quetzalcoatl", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Tonatiuh", new AttackCard { Name = "Tonatiuh", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Chalchiuhtlicue", new AttackCard { Name = "Chalchiuhtlicue", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Xipe-Totec ", new AttackCard { Name = "Xipe-Totec", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("La huesuda", new AttackCard { Name = "La huesuda", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El misterioso", new AttackCard { Name = "El misterioso", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Demonio azul Jr.", new AttackCard { Name = "Demonio Azul Jr.", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El tirantes", new AttackCard { Name = "El tirantes", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El poliedro", new AttackCard { Name = "El poliedro", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });

            foreach (var attackCard in GuestAvailableAttackCards)
            {
                RemainingGuestCards.Add(attackCard.Key, attackCard.Value);
            }
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


        private AttackCard GetCardData(string  cardName, string currentPlayerUsername)
        {
            if (string.IsNullOrEmpty(cardName))
            {
                Console.WriteLine("Carta vacía");
            } 

            Dictionary<string, AttackCard> SelectedAttackCardDeck = new Dictionary<string, AttackCard>();

            if(_lobby.Host == currentPlayerUsername)
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
            if(_lobby.Host == currentPlayer.Username)
            {
                AssignAttackCards(RemainingHostCards, 1);
                var hostAvailableCards = HostAvailableAttackCards.Values.ToList();
                int numCards = hostAvailableCards.Count;
                AddCardToPanel(numCards - 1, hostAvailableCards[numCards-1]);
            } 
            else
            {
                AssignAttackCards(RemainingGuestCards, 1);
                var guestAvailableCards = GuestAvailableAttackCards.Values.ToList();
                int numGuestAttackCards = guestAvailableCards.Count;
                AddCardToPanel(numGuestAttackCards-1, guestAvailableCards[numGuestAttackCards-1]);
            }
        }

        private void OnCellClickOwnBoard(int row, int col)
        {
            MessageBox.Show($"Has clickeado en la celda: ({row}, {col}) de tu propio tablero");
        }

        private void OnCellClickEnemyBoard(int row, int col)
        {
            MessageBox.Show($"Has clickeado en la celda: ({row}, {col}) del tablero enemigo, atacando");
        }

        private void OnAttackReceivedHandler(AttackPositionDTO attackPosition)
        {
            MessageBox.Show($"Ataque recibido en la posición: {attackPosition.X}, {attackPosition.Y}");

            _playerMatrixLife[attackPosition.X, attackPosition.Y] -= 1;

            _boardPlayerManager.InitializePlayerBoard(PlayerBoardGrid, _playerMatrixLife);
        }
    }
}
