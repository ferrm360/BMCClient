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
        private const string BaseImagePath = "pack://application:,,,/Images/AttackCards/";

        public Dictionary<string, AttackCard> InitializeHostCards()
        {
            var hostCards = new List<(string Name, int AttackLevel, string ImagePath)>
            {
                (CardNames.Llorona, 2, BaseImagePath + "LloronaCat.png"),
                (CardNames.CharroNegro, 2, BaseImagePath + "CharroCat.png"),
                (CardNames.Chupacabras, 2, BaseImagePath + "ChupaCabrasCat.png"),
                (CardNames.LaManoPeluda, 2, BaseImagePath + "ManoPeludaCat.png"),
                (CardNames.LaMuerte, 2, BaseImagePath + "MuerteCat.png"),
                (CardNames.Olmeca, 3, BaseImagePath + "OlmecaCat.png"),
                (CardNames.Tolteca, 3, BaseImagePath + "ToltecaCat.png"),
                (CardNames.Azteca, 3, BaseImagePath + "AztecaCat.png"),
                (CardNames.Maya, 3, BaseImagePath + "MayaCat.png"),
                (CardNames.Mixteca, 3, BaseImagePath + "MixtecaCat.png"),
                (CardNames.Huitzilopochtli, 4, BaseImagePath + "HuitzilopochtliCat.png"),
                (CardNames.Quetzalcoatl, 4, BaseImagePath + "QuetzalcoatlCat.png"),
                (CardNames.Tonatiuh, 4, BaseImagePath + "TonatiuhCat.png"),
                (CardNames.Chalchiuhtlicue, 4, BaseImagePath + "ChalchiuhticueCat.png"),
                (CardNames.XipeTotec, 4, BaseImagePath + "XipeTotecCat.png"),
                (CardNames.LaHuesuda, 5, BaseImagePath + "HuesudaCat.png"),
                (CardNames.ElMisterioso, 5, BaseImagePath + "MisteriosoCat.png"),
                (CardNames.DemonioAzulJr, 5, BaseImagePath + "DemonioAzulCat.png"),
                (CardNames.ElTirantes, 5, BaseImagePath + "ElTirantesCat.png"),
                (CardNames.ElPoliedro, 5, BaseImagePath + "ElPoliedroCat.png")
            };

            return CreateCardDictionary(hostCards);
        }

        public Dictionary<string, AttackCard> InitializeGuestCards()
        {
            var guestCards = new List<(string Name, int AttackLevel, string ImagePath)>
            {
                (CardNames.Llorona, 2, BaseImagePath + "LloronaDog.png"),
                (CardNames.CharroNegro, 2, BaseImagePath + "CharroDog.png"),
                (CardNames.Chupacabras, 2, BaseImagePath + "ChupaCabrasDog.png"),
                (CardNames.LaManoPeluda, 2, BaseImagePath + "ManoPeludaDog.png"),
                (CardNames.LaMuerte, 2, BaseImagePath + "MuerteDog.png"),
                (CardNames.Olmeca, 3, BaseImagePath + "OlmecaDog.png"),
                (CardNames.Tolteca, 3, BaseImagePath + "ToltecaDog.png"),
                (CardNames.Azteca, 3, BaseImagePath + "AztecaDog.png"),
                (CardNames.Maya, 3, BaseImagePath + "MayaDog.png"),
                (CardNames.Mixteca, 3, BaseImagePath + "MixtecaDog.png"),
                (CardNames.Huitzilopochtli, 4, BaseImagePath + "HuitzilopochtliDog.png"),
                (CardNames.Quetzalcoatl, 4, BaseImagePath + "QuetzalcoatlDog.png"),
                (CardNames.Tonatiuh, 4, BaseImagePath + "TonatiuhDog.png"),
                (CardNames.Chalchiuhtlicue, 4, BaseImagePath + "ChalchiuhticueDog.png"),
                (CardNames.XipeTotec, 4, BaseImagePath + "XipeTotecDog.png"),
                (CardNames.LaHuesuda, 5, BaseImagePath + "HuesudaDog.png"),
                (CardNames.ElMisterioso, 5, BaseImagePath + "MisteriosoDog.png"),
                (CardNames.DemonioAzulJr, 5, BaseImagePath + "DemonioAzulDog.png"),
                (CardNames.ElTirantes, 5, BaseImagePath + "ElTirantesDog.png"),
                (CardNames.ElPoliedro, 5, BaseImagePath + "ElPoliedroDog.png")
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
