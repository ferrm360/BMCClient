using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BMCWindows.GameplayBoard
{
    public class CardSelector
    {
        public string SelectedCard { get; private set; }
        public string SelectedCardName { get; private set; }
        public int SelectedCardLife { get; private set; }

        public void SelectCard(Border card, Func<string, (string Name, int Life)> getCardData)
        {
            if (card == null) return;

            var stackPanel = card.Child as StackPanel;
            if (stackPanel == null) return;

            var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock == null) return;

            SelectedCard = textBlock.Text;
            var cardData = getCardData(SelectedCard);
            SelectedCardName = cardData.Name;
            SelectedCardLife = cardData.Life;

            Console.WriteLine($"Carta seleccionada: {SelectedCardName}, Vida: {SelectedCardLife}");
        }
    }
}
