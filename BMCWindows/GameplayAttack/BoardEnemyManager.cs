using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using BMCWindows.DTOs;
using BMCWindows.Patterns.Singleton;
using BMCWindows.GameplayServer;

namespace BMCWindows.GameplayAttack
{
    public class BoardEnemyManager
    {
        private readonly string _cardBackImagePath = "pack://application:,,,/Images/CardBack.png";

        private readonly Action<int, int> _onCellClickAction;

        public BoardEnemyManager(Action<int, int> onCellClickAction)
        {
            _onCellClickAction = onCellClickAction;
        }

        public void InitializeEnemyBoard(Grid enemyBoardGrid)
        {
            enemyBoardGrid.Children.Clear();
            enemyBoardGrid.RowDefinitions.Clear();
            enemyBoardGrid.ColumnDefinitions.Clear();

            for (int row = 0; row < 4; row++)
            {
                enemyBoardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int col = 0; col < 3; col++)
            {
                enemyBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button cellButton = new Button
                    {
                        Width = 130,
                        Height = 70,
                        Background = Brushes.Transparent,
                        Margin = new Thickness(5),
                        Content = new Image
                        {
                            Source = new BitmapImage(new Uri(_cardBackImagePath, UriKind.Absolute)),
                            Stretch = Stretch.Fill
                        }
                    };

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    cellButton.Click += EnemyBoardButton_Click;
                    enemyBoardGrid.Children.Add(cellButton);
                }
            }
        }

        private void EnemyBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            AttackPositionDTO attackPositionDTO = new AttackPositionDTO();
            attackPositionDTO.X = row;
            attackPositionDTO.Y = col;

            _onCellClickAction?.Invoke(row, col);
        }

    }
}
