using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayBoard
{
    public class AttackCardHostManager
    {
        private Dictionary<string, (string Name, int AttackLevel, BitmapImage cardImage)> _attackCardData;

        public AttackCardHostManager() 
        {
            _attackCardData = new Dictionary<string, (string Name, int AttackLevel, BitmapImage cardImage)>
            {
                {"Llorona", ("Llorona", 1 , new BitmapImage(new Uri("pack://application:,,,/Images/IñakiCard.png"))) }
            };
        }

        public (string Name, int Life, BitmapImage CardImage) GetCardData(string cardName)
        {
            return _attackCardData.ContainsKey(cardName) ? _attackCardData[cardName] : ("", 0, null);
        }

        public void SelectAttackCard(Border card, Action<string, int, BitmapImage> onSelect)
        {
            if (card?.Child is StackPanel stackPanel)
            {
                var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    var attackCardData = GetCardData(textBlock.Text);
                    onSelect?.Invoke(attackCardData.Name, attackCardData.Life, attackCardData.CardImage);
                }
            }
        }
    }
}
