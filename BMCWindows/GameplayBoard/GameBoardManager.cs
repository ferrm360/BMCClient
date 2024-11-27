using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BMCWindows.GameplayBoard
{
    public class GameBoardManager
    {
        private Grid _boardGrid;
        private int[,] _matrix;
        private Action<int, int, string, int> _onCardPlaced;

        public GameBoardManager(Grid boardGrid, int rows, int cols, Action<int, int, string, int> onCardPlaced)
        {
            _boardGrid = boardGrid;
            _matrix = new int[rows, cols];
            _onCardPlaced = onCardPlaced;
        }

        public void Initialize(RoutedEventHandler cellClickHandler)
        {
            _boardGrid.Children.Clear();

            for (int row = 0; row < _matrix.GetLength(0); row++)
            {
                for (int col = 0; col < _matrix.GetLength(1); col++)
                {
                    var cellButton = new Button
                    {
                        Background = Brushes.Transparent,
                        Margin = new Thickness(1)
                    };
                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);
                    cellButton.Click += cellClickHandler;

                    _boardGrid.Children.Add(cellButton);
                }
            }
        }

        public void PlaceCard(int row, int col, string cardName, int cardLife, Button clickedButton)
        {
            _matrix[row, col] = cardLife;

            StackPanel cardPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    new TextBlock { Text = cardName, HorizontalAlignment = HorizontalAlignment.Center },
                    new TextBlock { Text = $"Vida: {cardLife}", HorizontalAlignment = HorizontalAlignment.Center }
                }
            };

            clickedButton.Content = cardPanel;
            _onCardPlaced?.Invoke(row, col, cardName, cardLife);
        }

        public int CountPlacedCards()
        {
            int count = 0;
            foreach (var value in _matrix)
            {
                if (value > 0) count++;
            }
            return count;
        }

        public int[,] GetMatrix() => _matrix;

        public void HandleButtonClick(Button button, string selectedCardName, int selectedCardLife)
        {
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (button.Content == null && !string.IsNullOrEmpty(selectedCardName))
            {
                PlaceCard(row, col, selectedCardName, selectedCardLife, button);
            }
            else
            {
                button.Content = null;
                Console.WriteLine($"Carta eliminada en la posición ({row}, {col})");
            }
        }

    }
}
