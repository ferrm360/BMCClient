using BMCWindows.Patterns.Singleton;
using BMCWindows.Validators;
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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        private string realPassword = string.Empty;

        public LogIn()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            Server.AccountServiceClient proxy = new Server.AccountServiceClient();
            String user = textBoxUser.Text;
            String password = realPassword;

            if (!FieldValidator.AreFieldsEmpty(user, password))
            {
                var result = proxy.Login(user, password);
                if (result.Success)
                {
                    Server.PlayerDTO player = new Server.PlayerDTO();
                    player.Username = user;
                    player.Password = password;
                    UserSessionManager.getInstance().loginPlayer(player);
                    this.NavigationService.Navigate(new HomePage());

                }
                else 
                {
                    MessageBox.Show(result.ErrorKey);
                }
            }
            else 
            {
                MessageBox.Show("Hay campos vacíos");
            }

            

        }

        private void HidePassword(object sender, System.Windows.Input.TextCompositionEventArgs e) 
        {
            TextBox textBox = sender as TextBox;
            
            realPassword += e.Text;
            textBox.Text = new string( '*', realPassword.Length);
            textBox.SelectionStart = textBox.Text.Length;
            e.Handled = true;

        }
    }
}
