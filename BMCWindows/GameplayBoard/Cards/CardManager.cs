using BMCWindows.Utilities;
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
            { "Cat1Life", ("Cat1", 1, new BitmapImage(new Uri(CardImagePaths.IñakiCard, UriKind.Absolute))) },
            { "CatAnother1Life", ("Cat1.1", 1, new BitmapImage(new Uri(CardImagePaths.IñakiCard, UriKind.Absolute))) },
            { "Cat2Lives", ("Cat2", 2, new BitmapImage(new Uri(CardImagePaths.MichiDelToroCard, UriKind.Absolute))) },
            { "CatAnother2Lives", ("Cat2.1", 2, new BitmapImage(new Uri(CardImagePaths.MichiDelToroCard, UriKind.Absolute))) },
            { "Cat3Lives", ("Cat3", 3, new BitmapImage(new Uri(CardImagePaths.Coca3LitrosCard, UriKind.Absolute))) },
            { "Dog1Life", ("Dog1", 1, new BitmapImage(new Uri(CardImagePaths.HuahuaCard, UriKind.Absolute))) },
            { "DogAnother1Life", ("Dog1.1", 1, new BitmapImage(new Uri(CardImagePaths.HuahuaCard, UriKind.Absolute))) },
            { "Dog2Lives", ("Dog2", 2, new BitmapImage(new Uri(CardImagePaths.AnasofCard, UriKind.Absolute))) },
            { "DogAnother2Lives", ("Dog2.1", 2, new BitmapImage(new Uri(CardImagePaths.AnasofCard, UriKind.Absolute))) },
            { "Dog3Lives", ("Dog3", 3, new BitmapImage(new Uri(CardImagePaths.ChilaquilCard, UriKind.Absolute))) }
            };
            return cardData.ContainsKey(cardName) ? cardData[cardName] : ("", 0, null);
        }

    }
}
