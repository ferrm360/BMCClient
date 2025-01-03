﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using BMCWindows.Utilities;

namespace BMCWindows.GameplayAttack
{
    public class BoardPlayerManager
    {
        private readonly Action<int, int> _onCellClickAction;

        public BoardPlayerManager(Action<int, int> onCellClickAction)
        {
            _onCellClickAction = onCellClickAction;
        }

        public void InitializePlayerBoard(Grid playerBoardGrid, int[,] _playerMatrixLife)
        {
            playerBoardGrid.Children.Clear();
            playerBoardGrid.RowDefinitions.Clear();
            playerBoardGrid.ColumnDefinitions.Clear();
                
            for (int row = 0; row < 4; row++)
            {
                playerBoardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int col = 0; col < 3; col++)
            {
                playerBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
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
                        Margin = new Thickness(2),
                        Content = new Grid
                        {
                            Children =
                            {
                                new Image
                                {
                                    Source = new BitmapImage(new Uri(CardImagePaths.CardBack, UriKind.Absolute)),
                                    Stretch = Stretch.Fill
                                }
                            }
                        }
                    };

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    cellButton.Click += PlayerBoardButton_Click;
                    playerBoardGrid.Children.Add(cellButton);
                }
            }
        }

        public async void UpdateCellToDead(Grid playerBoardGrid, int row, int col, BitmapImage cardImage)
        {
            await BoardCellUpdater.UpdateCellToDeadAsync(playerBoardGrid, row, col, cardImage);
        }



        private void PlayerBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            _onCellClickAction?.Invoke(row, col);
        }
    }
}
