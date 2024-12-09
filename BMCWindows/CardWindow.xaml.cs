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
        private List<AttackCard> _dogCardData {  get; set; }
        private List<AttackCard> _catCardData { get; set; }
        private List<AttackCard> _cardData { get; set; }
        private ChatServer.ChatServiceClient _proxy;

        public CardWindow()
        {
            InitializeComponent();
            _proxy = ChatServiceManager.ChatClient;
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().GetPlayerUserData();
            _dogCardData = new List<AttackCard>();
            _catCardData = new List<AttackCard>();
            _cardData = new List<AttackCard>();
            LoadCards();



        }

        private void LoadCards()
        {
            var dogList = GetDogCardList();
            var catList = GetCatCardList();    
            _cardData = dogList.Concat(catList).ToList();
            ItemsControlCardsControl.ItemsSource = _cardData;
        }

        private List<AttackCard> GetDogCardList()
        {
            var cardInitializer = new CardInitializer();
            var dogCards = cardInitializer.InitializeGuestCards();

            foreach (var card in dogCards)
            {
                _dogCardData.Add(new AttackCard
                {
                    CardImage = card.Value.CardImage,
                    Name = card.Value.Name,
                    AttackLevel = card.Value.AttackLevel,
                });
            }
            return _dogCardData;

        }

        private List<AttackCard> GetCatCardList()
        {
            var cardInitializer = new CardInitializer();
            var catCards = cardInitializer.InitializeHostCards();

            foreach (var card in catCards)
            {
                _catCardData.Add(new AttackCard
                {
                    CardImage = card.Value.CardImage,
                    Name = card.Value.Name,
                    AttackLevel = card.Value.AttackLevel,
                });
            }
            return _catCardData;

        }

        private void Cancel(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
