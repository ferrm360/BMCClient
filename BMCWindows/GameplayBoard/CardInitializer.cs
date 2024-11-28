using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayBoard
{
    public class CardInitializer
    {
        public void InitializeHostCards(Image imageOneLife, TextBlock nameOneLife, TextBlock lifeOneLife,
                                       Image imageAnotherOneLife, TextBlock nameAnotherOneLife, TextBlock lifeAnotherOneLife,
                                       Image imageTwoLives, TextBlock nameTwoLives, TextBlock lifeTwoLives,
                                       Image imageAnotherTwoLives, TextBlock nameAnotherTwoLives, TextBlock lifeAnotherTwoLives,
                                       Image imageThreeLives, TextBlock nameThreeLives, TextBlock lifeThreeLives)
        {
            InitializeCard(imageOneLife, nameOneLife, lifeOneLife, "Cat1Life", "1", "Images/IñakiCard.png");
            InitializeCard(imageAnotherOneLife, nameAnotherOneLife, lifeAnotherOneLife, "CatAnother1Life", "1", "Images/IñakiCard.png");
            InitializeCard(imageTwoLives, nameTwoLives, lifeTwoLives, "Cat2Lives", "2", "Images/michideltoroCard.png");
            InitializeCard(imageAnotherTwoLives, nameAnotherTwoLives, lifeAnotherTwoLives, "CatAnother2Lives", "2", "Images/michideltoroCard.png");
            InitializeCard(imageThreeLives, nameThreeLives, lifeThreeLives, "Cat3Lives", "3", "Images/coca3litrosCard.png");
        }

        public void InitializeGuestCards(Image imageOneLife, TextBlock nameOneLife, TextBlock lifeOneLife,
                                         Image imageAnotherOneLife, TextBlock nameAnotherOneLife, TextBlock lifeAnotherOneLife,
                                         Image imageTwoLives, TextBlock nameTwoLives, TextBlock lifeTwoLives,
                                         Image imageAnotherTwoLives, TextBlock nameAnotherTwoLives, TextBlock lifeAnotherTwoLives,
                                         Image imageThreeLives, TextBlock nameThreeLives, TextBlock lifeThreeLives)
        {
            InitializeCard(imageOneLife, nameOneLife, lifeOneLife, "Dog1Life", "1", "Images/HuahuaCard.png");
            InitializeCard(imageAnotherOneLife, nameAnotherOneLife, lifeAnotherOneLife, "DogAnother1Life", "1", "Images/HuahuaCard.png");
            InitializeCard(imageTwoLives, nameTwoLives, lifeTwoLives, "Dog2Lives", "2", "Images/AnasofCard.png");
            InitializeCard(imageAnotherTwoLives, nameAnotherTwoLives, lifeAnotherTwoLives, "DogAnother2Lives", "2", "Images/AnasofCard.png");
            InitializeCard(imageThreeLives, nameThreeLives, lifeThreeLives, "Dog3Lives", "3", "Images/ChilaquilCard.png");
        }

        private void InitializeCard(Image image, TextBlock name, TextBlock life, string cardName, string cardLife, string imagePath)
        {
            BitmapImage cardImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Source = cardImage;
            name.Text = cardName;
            life.Text = cardLife;
        }
    }
}
