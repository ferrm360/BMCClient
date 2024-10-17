using BMCWindows.Patterns.Singleton;
using BMCWindows.Server;
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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Page
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            Server.AccountServiceClient proxy = new Server.AccountServiceClient();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player.Username = textBoxUser.Text;
            player.Email = textBoxEmail.Text;
            player.Password = textBoxPassword.Text;
            if(!FieldValidator.AreFieldsEmpty(textBoxUser.Text, textBoxEmail.Text, textBoxPassword.Text, textBoxConfirmPassword.Text) /*&& FieldValidator.ValidatePassword(textBoxPassword.Text) && textBoxPassword.Text == textBoxConfirmPassword.Text */)
            {
                var result = proxy.Register(player);
                if (result.Success) {
                    UserSessionManager.getInstance().loginPlayer(player);
                    this.NavigationService.Navigate(new HomePage());
                } else
                {
                    MessageBox.Show(result.ErrorKey);
                }
            }
            else
            {
                MessageBox.Show("Hay campos vacíos o incorrectos");

            }

            

            
            
        }

        private void HidePassword(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text += "*";
            e.Handled = true;

        }


        private void Cancel(object sender, RoutedEventArgs e) 
        {
            this.NavigationService.GoBack();
        }

       
    }
}
