using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayAttack
{
    public static class BoardCellUpdater
    {
        public static void UpdateCellToDead(Grid boardGrid, int row, int col, BitmapImage cardImage)
        {
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
                                MessageBox.Show("Imagen actualizada");
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
