using BMCWindows.EmailServer;
using BMCWindows.Patterns.Singleton;
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
                DynamicMessageTextBlock.Text = "Enviando código de verificación...";
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
                                Dispatcher.Invoke(() =>
                                    MessageBox.Show(result.ErrorKey, "Error al Enviar", MessageBoxButton.OK, MessageBoxImage.Error));
                            }
                        }
                        catch (Exception ex)
                        {
                            Dispatcher.Invoke(() =>
                                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                        }
                    }
                });

                DynamicMessageTextBlock.Text = "Código enviado con éxito.";
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el código: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                DynamicMessageTextBlock.Text = "¡Código correcto!";
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                Task.Delay(1000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        NavigationService?.Navigate(new Uri("NextPage.xaml", UriKind.Relative));
                    });
                });
            }
            else if (VerificationCodeTextBox.Text.Length == _currentCode.Length)
            {
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
                DynamicMessageTextBlock.Text = "Muchos intentos de reenvío. Intenta nuevamente en 1 minuto.";
                DynamicMessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            _resendAttempts++;
            _currentCode = GenerateAlphanumericCode();

            await Task.Run(() => SendVerificationCode());
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que deseas cancelar la operación?",
                                         "Cancelar Verificación",
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
