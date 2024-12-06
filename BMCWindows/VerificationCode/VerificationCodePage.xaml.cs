using BMCWindows.EmailServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BMCWindows.VerificationCode
{
    public partial class VerificationCodePage : Page
    {
        private string _currentCode;
        private bool _isProcessing = false;
        private int _resendAttempts = 0;
        private DateTime _firstAttemptTime;
        private const int MaxResendAttempts = 3;
        private const int TimeLimitInSeconds = 60;

        public VerificationCodePage()
        {
            InitializeComponent();

            _currentCode = GenerateAlphanumericCode();
            _firstAttemptTime = DateTime.Now;
            SendVerificationCode();
        }

        private string GenerateAlphanumericCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void SendVerificationCode()
        {
            if (_isProcessing)
                return;

            try
            {
                _isProcessing = true;

                LoadingProgressBar.Visibility = Visibility.Visible;
                string infoText = Properties.Resources.Info_SendingCode.ToString();
                DynamicMessageTextBlock.Text = infoText;
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Orange);

                await Task.Run(() =>
                {
                    EmailDTO emailDTO = new EmailDTO
                    {
                        Recipient = UserSessionManager.getInstance().GetPlayerUserData().Email,
                        VerificationCode = _currentCode,
                        EmailType = "changepassword",
                        Username = UserSessionManager.getInstance().GetPlayerUserData().Username
                    };

                    using (var proxy = new EmailServiceClient())
                    {
                        try
                        {
                            var result = proxy.SendEmail(emailDTO);
                            if (!result.IsSuccess)
                            {
                               
                                    ErrorMessages errorMessages = new ErrorMessages();
                                    errorMessages.ShowErrorMessage(result.ErrorKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMessages errorMessages = new ErrorMessages();
                            errorMessages.ShowErrorMessage("Error.ServerError");
                        }
                    }
                });

                string codeSent = Properties.Resources.Info_CodeSentSuccesfully.ToString(); 
                DynamicMessageTextBlock.Text = codeSent;
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                ErrorMessages errorMessages = new ErrorMessages();
                errorMessages.ShowErrorMessage("Error.SendCodeFailed");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
                _isProcessing = false;
            }
        }

        private void VerificationCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VerificationCodeTextBox.Text.Trim().Equals(_currentCode, StringComparison.OrdinalIgnoreCase))
            {
                string infoText = Properties.Resources.Info_ValidationCodeCorrect.ToString();
                DynamicMessageTextBlock.Text = infoText;
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                Task.Delay(1000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.NavigationService.Navigate(new SettingsWindow());
                    });
                });
            }
            else if (VerificationCodeTextBox.Text.Length == _currentCode.Length)
            {
                string errorText = Properties.Resources.Error_InvalidValidationCode.ToString();
                DynamicMessageTextBlock.Text = "Código de verificación incorrecto.";
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private async void OnResendCodeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_isProcessing)
                return;

            var currentTime = DateTime.Now;
            if ((currentTime - _firstAttemptTime).TotalSeconds > TimeLimitInSeconds)
            {
                _resendAttempts = 0;
                _firstAttemptTime = currentTime;
            }

            if (_resendAttempts >= MaxResendAttempts)
            {
                string errorText = Properties.Resources.Error_TooManyTries.ToString();
                DynamicMessageTextBlock.Text = errorText;
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            _resendAttempts++;
            _currentCode = GenerateAlphanumericCode();

            await Task.Run(() => SendVerificationCode());
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            string confirmationText = Properties.Resources.Confirmation_ConfirmCancelation.ToString();
            string messageTitle = Properties.Resources.CancelVerificationTitle.ToString();
            var result = MessageBox.Show(confirmationText,
                                         messageTitle,
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (NavigationService != null)
                {
                    NavigationService.GoBack();
                }
                else
                {
                }
            }
        }

    }
}
