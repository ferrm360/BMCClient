using BMCWindows.EmailServer;
using BMCWindows.Patterns.Singleton;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressBarLoading.Visibility = Visibility.Visible;
                    TextBlockDynamicMessage.Text = Properties.Resources.Info_SendingCode.ToString();
                    TextBlockDynamicMessage.Foreground = new SolidColorBrush(Colors.Orange);
                });

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
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    ErrorMessages errorMessages = new ErrorMessages();
                                    errorMessages.ShowErrorMessage(result.ErrorKey);
                                });
                            }
                        }
                        catch (CommunicationException commEx)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ErrorMessages errorMessages = new ErrorMessages();
                                errorMessages.ShowErrorMessage("Error.CommunicationError");
                            });
                        }
                        catch (TimeoutException timeoutEx)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ErrorMessages errorMessages = new ErrorMessages();
                                errorMessages.ShowErrorMessage("Error.TimeOutError");
                            });
                        }
                        catch (Exception ex)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ErrorMessages errorMessages = new ErrorMessages();
                                errorMessages.ShowErrorMessage("Error.GeneralException");
                            });
                        }
                    }
                });

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TextBlockDynamicMessage.Text = Properties.Resources.Info_CodeSentSuccesfully.ToString();
                    TextBlockDynamicMessage.Foreground = new SolidColorBrush(Colors.Green);
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ErrorMessages errorMessages = new ErrorMessages();
                    errorMessages.ShowErrorMessage("Error.SendCodeFailed");
                });
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressBarLoading.Visibility = Visibility.Collapsed;
                });

                _isProcessing = false;
            }
        }


        private void VerificationCodeTextBox(object sender, TextChangedEventArgs e)
        {
            if (TextBoxVerificationCode.Text.Trim().Equals(_currentCode, StringComparison.OrdinalIgnoreCase))
            {
                string infoText = Properties.Resources.Info_ValidationCodeCorrect.ToString();
                TextBlockDynamicMessage.Text = infoText;
                TextBlockDynamicMessage.Foreground = new SolidColorBrush(Colors.Green);

                Task.Delay(1000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.NavigationService.Navigate(new SettingsWindow());
                    });
                });
            }
            else if (TextBoxVerificationCode.Text.Length == _currentCode.Length)
            {
                string errorText = Properties.Resources.ResourceManager.GetString("Error.InvalidValidationCode");
                TextBlockDynamicMessage.Text = errorText;
                TextBlockDynamicMessage.Foreground = new SolidColorBrush(Colors.Red);
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
                TextBlockDynamicMessage.Text = errorText;
                TextBlockDynamicMessage.Foreground = new SolidColorBrush(Colors.Red);
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
