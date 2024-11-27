using BMCWindows.LobbyServer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BMCWindows.GameplayPage
{
    public partial class GameplayAttackWindow : Page
    {
        private LobbyDTO _lobby;
        private int[,] _playerMatrix;
        private string _cardBackImagePath = "pack://application:,,,/Images/CardBack.png"; // Pack URI

        public GameplayAttackWindow(LobbyDTO lobby, int[,] playerMatrix)
        {
            InitializeComponent();
            _lobby = lobby;
            _playerMatrix = playerMatrix;

            // Inicializar los tableros
            InitializePlayerBoard();
            InitializeEnemyBoard();
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
    }
}
