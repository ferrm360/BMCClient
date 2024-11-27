using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayBoard
{
    public class CardManager
    {
        private Dictionary<string, (string Name, int Life, BitmapImage Image)> _cardData;

        public CardManager() 
        {
             _cardData = new Dictionary<string, (string, int, BitmapImage)>
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

        }

        public (string Name, int Life, BitmapImage CardImage) GetCardData(string cardName)
        {
            return _cardData.ContainsKey(cardName) ? _cardData[cardName] : ("", 0, null);
        }

        public void SelectCard(Border card, Action<string, int, BitmapImage> onSelect)
        {
            if (card?.Child is StackPanel stackPanel)
            {
                var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    var cardData = GetCardData(textBlock.Text);
                    onSelect?.Invoke(cardData.Name, cardData.Life, cardData.CardImage);
                }
            }
        }

        public void RemoveCardFromButtons(Grid boardGrid, string cardName)
        {
            foreach (UIElement element in boardGrid.Children)
            {
                if (element is Button button && button.Content is StackPanel stackPanel)
                {
                    var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null && textBlock.Text == cardName)
                    {
                        button.Content = null;
                        Console.WriteLine($"Carta {cardName} eliminada de la posición ({Grid.GetRow(button)}, {Grid.GetColumn(button)})");
                    }
                }
            }
        }

    }
}
