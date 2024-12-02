using BMCWindows.DTOs;
using BMCWindows.GameplayBoard.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayAttack
{
    public class CardInitializer
    {
        private const string BaseImagePath = "pack://application:,,,/Images/";

        public Dictionary<string, AttackCard> InitializeHostCards()
        {
            var hostCards = new List<(string Name, int AttackLevel, string ImagePath)>
            {
                (CardNames.Llorona, 2, BaseImagePath + "IñakiCard.png"),
                (CardNames.CharroNegro, 2, BaseImagePath + "IñakiCard.png"),
                (CardNames.Chupacabras, 2, BaseImagePath + "IñakiCard.png"),
                (CardNames.LaManoPeluda, 2, BaseImagePath + "IñakiCard.png"),
                (CardNames.LaMuerte, 2, BaseImagePath + "IñakiCard.png"),
                (CardNames.Olmeca, 3, BaseImagePath + "IñakiCard.png"),
                (CardNames.Tolteca, 3, BaseImagePath + "IñakiCard.png"),
                (CardNames.Azteca, 3, BaseImagePath + "IñakiCard.png"),
                (CardNames.Maya, 3, BaseImagePath + "IñakiCard.png"),
                (CardNames.Mixteca, 3, BaseImagePath + "IñakiCard.png"),
                (CardNames.Huitzilopochtli, 4, BaseImagePath + "IñakiCard.png"),
                (CardNames.Quetzalcoatl, 4, BaseImagePath + "IñakiCard.png"),
                (CardNames.Tonatiuh, 4, BaseImagePath + "IñakiCard.png"),
                (CardNames.Chalchiuhtlicue, 4, BaseImagePath + "IñakiCard.png"),
                (CardNames.XipeTotec, 4, BaseImagePath + "IñakiCard.png"),
                (CardNames.LaHuesuda, 5, BaseImagePath + "IñakiCard.png"),
                (CardNames.ElMisterioso, 5, BaseImagePath + "IñakiCard.png"),
                (CardNames.DemonioAzulJr, 5, BaseImagePath + "IñakiCard.png"),
                (CardNames.ElTirantes, 5, BaseImagePath + "IñakiCard.png"),
                (CardNames.ElPoliedro, 5, BaseImagePath + "IñakiCard.png")
            };

            return CreateCardDictionary(hostCards);
        }

        public Dictionary<string, AttackCard> InitializeGuestCards()
        {
            var guestCards = new List<(string Name, int AttackLevel, string ImagePath)>
            {
                (CardNames.Llorona, 2, BaseImagePath + "AnaSofCard.png"),
                (CardNames.CharroNegro, 2, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Chupacabras, 2, BaseImagePath + "AnaSofCard.png"),
                (CardNames.LaManoPeluda, 2, BaseImagePath + "AnaSofCard.png"),
                (CardNames.LaMuerte, 2, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Olmeca, 3, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Tolteca, 3, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Azteca, 3, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Maya, 3, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Mixteca, 3, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Huitzilopochtli, 4, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Quetzalcoatl, 4, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Tonatiuh, 4, BaseImagePath + "AnaSofCard.png"),
                (CardNames.Chalchiuhtlicue, 4, BaseImagePath + "AnaSofCard.png"),
                (CardNames.XipeTotec, 4, BaseImagePath + "AnaSofCard.png"),
                (CardNames.LaHuesuda, 5, BaseImagePath + "AnaSofCard.png"),
                (CardNames.ElMisterioso, 5, BaseImagePath + "AnaSofCard.png"),
                (CardNames.DemonioAzulJr, 5, BaseImagePath + "AnaSofCard.png"),
                (CardNames.ElTirantes, 5, BaseImagePath + "AnaSofCard.png"),
                (CardNames.ElPoliedro, 5, BaseImagePath + "AnaSofCard.png")
            };

            return CreateCardDictionary(guestCards);
        }

        private Dictionary<string, AttackCard> CreateCardDictionary(List<(string Name, int AttackLevel, string ImagePath)> cardData)
        {
            var cardDictionary = new Dictionary<string, AttackCard>();
            foreach (var card in cardData)
            {
                cardDictionary.Add(card.Name, new AttackCard
                {
                    Name = card.Name,
                    AttackLevel = card.AttackLevel,
                    CardImage = new BitmapImage(new Uri(card.ImagePath))
                });
            }
            return cardDictionary;
        }
    }
}
