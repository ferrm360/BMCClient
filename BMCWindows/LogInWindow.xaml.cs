using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using BMCWindows.Validators;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        private string _realPassword = string.Empty;
        private int _maxPasswordLength = 255;

        public LogIn()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartPage());
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            Server.AccountServiceClient proxy = new Server.AccountServiceClient();
            String user = TextBoxUser.Text;
            String password = PasswordBoxPassword.Password.ToString();
            

            if (!FieldValidator.AreFieldsEmpty(user, password))
            {
                try
                {
                    var result = proxy.Login(user, password);
                    if (result.IsSuccess)
                    {
                        Server.PlayerDTO player = new Server.PlayerDTO();
                        player.Username = user;
                        player.Password = password;
                        player.Email = result.Email;
                        UserSessionManager.getInstance().LoginPlayer(player, false);
                        this.NavigationService.Navigate(new HomePage());

                    }
                    else
                    {
                        
                        ErrorMessages errorMessages = new ErrorMessages();  
                        errorMessages.ShowErrorMessage(result.ErrorKey);
                        
                    }
                }
                catch(EndpointNotFoundException)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.ServerError");
                }
            }
            else 
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("MessageBoxEmptyFields");
            }
        }

        private void HidePassword(object sender, System.Windows.Input.TextCompositionEventArgs e) 
        {
            TextBox textBox = sender as TextBox;
            
            _realPassword += e.Text;
            textBox.Text = new string( '*', _realPassword.Length);
            textBox.SelectionStart = textBox.Text.Length;
            e.Handled = true;
        }

        private void CheckLimit(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int maxLength = int.Parse(textBox.Tag.ToString());  

            if (textBox.Text.Length >= maxLength)
            {
                if (textBox.Text.Length > maxLength)
                {
                    textBox.Text = textBox.Text.Substring(0, maxLength);
                    textBox.SelectionStart = textBox.Text.Length;
                }

                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.IsReadOnly = false;
            }
        }


        private void CheckPasswordLimit(object sender,  RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PasswordBoxPassword.Password;

            if (password.Length >= _maxPasswordLength)
            {
                PasswordBoxPassword.IsEnabled = false;
                if (password.Length > _maxPasswordLength)
                {
                    passwordBox.Password = password.Substring(0, _maxPasswordLength);
                }
            }
            else
            {
                PasswordBoxPassword.IsEnabled = true;
            }
        }
    }
}
