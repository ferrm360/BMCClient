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
    /// Lógica de interacción para GameOptionsWindow.xaml
    /// </summary>
    public partial class GameOptionsWindow : Page
    {
        public GameOptionsWindow()
        {
            InitializeComponent();
        }


        private void Cancel(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void GoToCreateLobbyWindow(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new LobbyCreationWindow());
        }

        private void GoToJoinLobbyWindow(object sender, EventArgs e) 
        {
            this.NavigationService.Navigate(new LobbiesWindow());
        }
    }
}
