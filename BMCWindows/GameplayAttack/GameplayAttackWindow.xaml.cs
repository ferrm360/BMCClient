using BMCWindows.DTOs;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private int[,] _playerMatrix;
        private string _cardBackImagePath = "pack://application:,,,/Images/CardBack.png"; // Pack URI
        private Dictionary<string, AttackCard> HostAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> GuestAvailableAttackCards { get; set; }
        private Dictionary<string, AttackCard> CurrentPlayerDeck { get; set; }
        private Dictionary<string, AttackCard> RemainingHostCards { get; set; }
        private Dictionary<string, AttackCard> RemainingGuestCards { get; set; }
        private string selectedCard = null;
        private string selectedCardName;
        private int selectedCardLife;

        public GameplayAttackWindow(LobbyDTO lobby, int[,] playerMatrix)
        {
            InitializeComponent();
            _lobby = lobby;
            _playerMatrix = playerMatrix;

            HostAvailableAttackCards = new Dictionary<string, AttackCard>();    
            GuestAvailableAttackCards = new Dictionary<string, AttackCard>();
            CurrentPlayerDeck = new Dictionary<string, AttackCard>();
            RemainingHostCards = new Dictionary<string, AttackCard>();
            RemainingGuestCards = new Dictionary<string, AttackCard>();    
            Server.PlayerDTO currentPlayer = new Server.PlayerDTO();
            currentPlayer = UserSessionManager.getInstance().GetPlayerUserData();

            // Inicializar los tableros
            InitializePlayerBoard();
            InitializeEnemyBoard();
            InitializeAvailableCards();
            if(_lobby.Host == currentPlayer.Username)
            {
                AssignAttackCards(HostAvailableAttackCards);
            } else
            {
                AssignAttackCards(GuestAvailableAttackCards);
            }

            ShowAssignedCards();
        }

        private void InitializePlayerBoard()
        {
            PlayerBoardGrid.Children.Clear();
            PlayerBoardGrid.RowDefinitions.Clear();
            PlayerBoardGrid.ColumnDefinitions.Clear();

            // Definir las filas y columnas del tablero
            for (int row = 0; row < 4; row++)
            {
                PlayerBoardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int col = 0; col < 3; col++)
            {
                PlayerBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crear los botones
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button cellButton = new Button
                    {
                        Background = Brushes.Transparent,
                        Margin = new Thickness(5),
                        Content = new Grid
                        {
                            Children =
                            {
                                new Image
                                {
                                    Source = new BitmapImage(new Uri(_cardBackImagePath, UriKind.Absolute)),
                                    Stretch = Stretch.Fill // Asegura que la imagen abarque toda la celda
                                },
                                new TextBlock
                                {
                                    Text = _playerMatrix[row, col].ToString(), // Mostrar valor de la matriz
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 12,
                                    FontWeight = FontWeights.Bold,
                                    Foreground = Brushes.White
                                }
                            }
                        }
                    };

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    cellButton.Click += PlayerBoardButton_Click;
                    PlayerBoardGrid.Children.Add(cellButton);
                }
            }
        }

        private void InitializeEnemyBoard()
        {
            EnemyBoardGrid.Children.Clear();
            EnemyBoardGrid.RowDefinitions.Clear();
            EnemyBoardGrid.ColumnDefinitions.Clear();

            // Definir las filas y columnas del tablero
            for (int row = 0; row < 4; row++)
            {
                EnemyBoardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int col = 0; col < 3; col++)
            {
                EnemyBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crear los botones
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button cellButton = new Button
                    {
                        Background = Brushes.Transparent,
                        Margin = new Thickness(5),
                        Content = new Image
                        {
                            Source = new BitmapImage(new Uri(_cardBackImagePath, UriKind.Absolute)),
                            Stretch = Stretch.Fill // Asegura que la imagen abarque toda la celda
                        }
                    };

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    cellButton.Click += EnemyBoardButton_Click;
                    EnemyBoardGrid.Children.Add(cellButton);
                }
            }
        }

        private void PlayerBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            MessageBox.Show($"Seleccionaste la posición del tablero propio: ({row}, {col})");
        }

        private void EnemyBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            MessageBox.Show($"Atacando posición del tablero enemigo: ({row}, {col})");
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ejecutando ataque...");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AssignAttackCards(Dictionary<string, AttackCard> AvailableCards)
        {
            Random random = new Random();
            List<string> attackCardKeys = AvailableCards.Keys.ToList();

            // Barajar las claves aleatoriamente para obtener un orden aleatorio
            attackCardKeys = attackCardKeys.OrderBy(x => random.Next()).ToList();

            // Asignar las primeras 5 cartas al jugador
            for (int i = 0; i < 5; i++)
            {
                string selectedAttackCardKey = attackCardKeys[i];
                AttackCard selectedAttackCard = AvailableCards[selectedAttackCardKey];

                // Agregar la carta al CurrentPlayerDeck
                if (!CurrentPlayerDeck.ContainsKey(selectedAttackCard.Name))
                {
                    CurrentPlayerDeck.Add(selectedAttackCard.Name, selectedAttackCard);
                }

            }

            foreach (var attackCard in CurrentPlayerDeck) 
            {
                Console.WriteLine(attackCard.Key);
            }
        }

        private void ShowAssignedCards()
        {
            // Limpiar las cartas mostradas previamente
            stackPanelPlayerAttackCards.Children.Clear();

            // Recorrer el diccionario de cartas del jugador
            int cardIndex = 0;
            foreach (var card in CurrentPlayerDeck.Values)  // Suponiendo que tienes un diccionario de cartas
            {
                // Agregar cada carta al panel, pasando el índice para personalizar la posición
                AddCardToPanel(cardIndex, card);
                cardIndex++;  // Aumentar el índice para la siguiente carta
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
                DataContext = attackCard.Name  // Asigna el nombre de la carta como contexto de datos
            };

            newCard.MouseLeftButtonDown += SelectCard;  // Evento de clic para la carta

            // Crear el StackPanel para mostrar la carta
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Crear la imagen de la carta
            var cardImage = new Image
            {
                Source = attackCard.CardImage,  // Usando la imagen de la carta
                Width = 60,
                Height = 90
            };

            // Crear el texto con el nombre de la carta
            var textBlock = new TextBlock
            {
                Text = attackCard.Name,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };

            // Agregar la imagen y el nombre de la carta al StackPanel
            stackPanel.Children.Add(cardImage);
            stackPanel.Children.Add(textBlock);

            // Asignar el StackPanel al Border
            newCard.Child = stackPanel;

            // Agregar la carta al contenedor
            stackPanelPlayerAttackCards.Children.Add(newCard);
        }











        // verificar el host
        public int GetCardAttackLevel(string cardName, string player)
        {
            // Determinar qué deck usar
            var deck = CurrentPlayerDeck;

            // Verificar si la carta está en el deck
            if (deck.ContainsKey(cardName))
            {
                return deck[cardName].AttackLevel; // Acceso rápido al nivel de ataque
            }
            return 0; // Si no se encuentra la carta, retornar 0 o algún valor por defecto
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
            GuestAvailableAttackCards.Add("La muerte", new AttackCard { Name = "Llorona", AttackLevel = 2, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Olmeca", new AttackCard { Name = "Llorona", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Tolteca", new AttackCard { Name = "Llorona", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Azteca", new AttackCard { Name = "Llorona", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Maya", new AttackCard { Name = "Llorona", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Moixteca", new AttackCard { Name = "Llorona", AttackLevel = 3, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Huitzilopochtli ", new AttackCard { Name = "Llorona", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Quetzalcoatl", new AttackCard { Name = "Llorona", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Tonatiuh", new AttackCard { Name = "Llorona", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Chalchiuhtlicue", new AttackCard { Name = "Llorona", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Xipe-Totec ", new AttackCard { Name = "Llorona", AttackLevel = 4, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("La huesuda", new AttackCard { Name = "Llorona", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El misterioso", new AttackCard { Name = "Llorona", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("Demonio azul Jr.", new AttackCard { Name = "Llorona", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El tirantes", new AttackCard { Name = "Llorona", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });
            GuestAvailableAttackCards.Add("El poliedro", new AttackCard { Name = "Llorona", AttackLevel = 5, CardImage = new BitmapImage(new Uri("pack://application:,,,/Images/AnaSofCard.png")) });

            foreach (var attackCard in GuestAvailableAttackCards)
            {
                RemainingGuestCards.Add(attackCard.Key, attackCard.Value);
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
    }
}
