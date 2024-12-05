using BMCWindows.DTOs;
using BMCWindows.GameplayAttack;
using BMCWindows.Patterns.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BMCWindows
{
    /// <summary>
    /// Lógica de interacción para CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Page
    {
        private List<AttackCard> dogCardData {  get; set; }
        private List<AttackCard> catCardData { get; set; }
        private List<AttackCard> cardData { get; set; }

        public CardWindow()
        {
            InitializeComponent();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            dogCardData = new List<AttackCard>();
            catCardData = new List<AttackCard>();
            cardData = new List<AttackCard>();
            LoadCards();



        }

        private void LoadCards()
        {
            var dogList = GetDogCardList();
            var catList = GetCatCardList();    
            cardData = dogList.Concat(catList).ToList();
            cardsControl.ItemsSource = cardData;
        }

        private List<AttackCard> GetDogCardList()
        {
            var cardInitializer = new CardInitializer();
            var dogCards = cardInitializer.InitializeGuestCards();

            foreach (var card in dogCards)
            {
                dogCardData.Add(new AttackCard
                {
                    CardImage = card.Value.CardImage,
                    Name = card.Value.Name,
                    AttackLevel = card.Value.AttackLevel,
                });
            }
            return dogCardData;

        }

        private List<AttackCard> GetCatCardList()
        {
            var cardInitializer = new CardInitializer();
            var catCards = cardInitializer.InitializeHostCards();

            foreach (var card in catCards)
            {
                catCardData.Add(new AttackCard
                {
                    CardImage = card.Value.CardImage,
                    Name = card.Value.Name,
                    AttackLevel = card.Value.AttackLevel,
                });
            }
            return catCardData;

        }

        private void Cancel(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
