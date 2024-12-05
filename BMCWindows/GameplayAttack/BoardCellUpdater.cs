using System;
using System.Linq;
using System.Threading.Tasks; // Necesario para usar Task.Delay
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayAttack
{
    public static class BoardCellUpdater
    {
        public static async Task UpdateCellToDeadAsync(Grid boardGrid, int row, int col, BitmapImage cardImage)
        {
            await Task.Delay(1000);

            foreach (var child in boardGrid.Children)
            {
                if (child is Button button)
                {
                    int buttonRow = Grid.GetRow(button);
                    int buttonCol = Grid.GetColumn(button);

                    if (buttonRow == row && buttonCol == col)
                    {
                        var grid = button.Content as Grid;
                        if (grid != null)
                        {
                            var image = grid.Children.OfType<Image>().FirstOrDefault();
                            if (image != null)
                            {
                                image.Source = cardImage;
                            }

                            var textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
                            if (textBlock != null)
                            {
                                textBlock.Visibility = Visibility.Collapsed;
                            }
                        }

                        button.IsEnabled = false;
                        break;
                    }
                }
            }
        }
    }
}
