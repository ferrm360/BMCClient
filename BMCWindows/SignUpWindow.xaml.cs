using BMCWindows.Patterns.Singleton;
using BMCWindows.Server;
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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Page
    {

        private int _maxPasswordLength = 255;
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            Server.AccountServiceClient proxy = new Server.AccountServiceClient();
            Server.PlayerDTO player = new Server.PlayerDTO();
            player.Username = TextBoxUser.Text;
            player.Email = TextBoxEmail.Text;
            player.Password = PasswordBoxPassword.Password;
            if(!FieldValidator.AreFieldsEmpty(TextBoxUser.Text, TextBoxEmail.Text, PasswordBoxPassword.Password, PasswordBoxConfirmPassword.Password) && FieldValidator.ValidatePassword(PasswordBoxPassword.Password) && PasswordBoxPassword.Password == PasswordBoxConfirmPassword.Password && FieldValidator.ValidateEmail(TextBoxEmail.Text))
            {
                try
                {
                    var result = proxy.Register(player);
                    if (result.IsSuccess)
                    {
                        UserSessionManager.getInstance().LoginPlayer(player);
                        this.NavigationService.Navigate(new HomePage());
                    }
                    else
                    {
                        ErrorMessages errorMessages = new ErrorMessages();
                        errorMessages.ShowErrorMessage(result.ErrorKey);
                    }
                }
                catch (EndpointNotFoundException)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.ServerError");
                }
                catch (CommunicationException)
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.CommunicationError");
                }
                catch (TimeoutException) 
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.TimeoutError");
                }
            }
            else
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("MessageBoxEmptyFields");

            }  
        }

        private void Cancel(object sender, RoutedEventArgs e) 
        {
            this.NavigationService.GoBack();
        }

        private void CheckLimit(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int maxLength = int.Parse(textBox.Tag.ToString());

            if (textBox.Text.Length >= maxLength)
            {
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.IsReadOnly = false;
            }
        }

        private void CheckPasswordLimit(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = passwordBox.Password;

            if (password.Length >= _maxPasswordLength)
            {
                passwordBox.IsEnabled = false;
            }
            else
            {
                passwordBox.IsEnabled = true;
            }
        }


    }
}
