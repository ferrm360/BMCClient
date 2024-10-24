using BMCWindows.Patterns.Singleton;
using BMCWindows.Properties;
using BMCWindows.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page, ChatServer.IChatServiceCallback
    {

       
        public ObservableCollection<Friend> Friends { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public ChatService chatService = new ChatService();
        public ChatServer.ChatServiceClient proxy;



        public HomePage()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();
            generalMessages.ItemsSource = Messages; // Enlazamos solo una vez aquí
            //generalMessages.ItemsSource = Messages; 
            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();
            InstanceContext context = new InstanceContext(this);
            proxy = new ChatServer.ChatServiceClient(context);
            proxy.RegisterUser(player.Username);
            labelUserName.Content = player.Username;

          /*  FriendServer.FriendshipServiceClient friendsProxy = new FriendServer.FriendshipServiceClient();
            var result = friendsProxy.GetFriendList(player.Username);
            if (result.IsSuccess)
            {
                if (result.Data is List<PlayerDTO> friendPlayers)
                {
                    ObservableCollection<Friend> friendsList = new ObservableCollection<Friend>(
                        friendPlayers.Select(friendPlayer => new Friend
                        {
                            UserName = friendPlayer.Username,

                        })
                        );
                    FriendsList.ItemsSource = friendsList;
                    Chat.ItemsSource = friendsList;

                }
                else
                {
                    MessageBox.Show(result.ErrorKey);
                }

            }*/
        }

       

       

        private void SendGeneralMessage_Click(object sender, RoutedEventArgs e)
        {

            Server.PlayerDTO player = new Server.PlayerDTO();
            player = UserSessionManager.getInstance().getPlayerUserData();

            if (!string.IsNullOrEmpty(textboxGeneralChat.Text))
            {
                proxy.SendMessage(player.Username, textboxGeneralChat.Text);
                ReceiveMessage(textboxGeneralChat.Text);  
                textboxGeneralChat.Clear();
                //LoadRecentMessages();
                // Actualizar los mensajes mostrados en la interfaz
            }
        }

        public void ReceiveMessage(string message)
        {
            Server.PlayerDTO player = UserSessionManager.getInstance().getPlayerUserData();

            // Asegúrate de que la colección está siendo modificada en el hilo principal
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(new Message { Sender = player.Username, Messages = message });
            });

        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Settings());
        }

        public void OpenContextMenu(object sender, RoutedEventArgs e)
        {
            menuOptions.Visibility = menuOptions.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoToSearchWindow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SearchWindow());
        }

        private void GoToProfileWindow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProfileWindow());
        }
    }

    public class Friend
    {
        public string UserName { get; set; }
    }

    
}
