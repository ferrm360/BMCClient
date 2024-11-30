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
    public static class CardManager
    {
        public static (string Name, int Life, BitmapImage CardImage) GetCardData(string cardName)
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
