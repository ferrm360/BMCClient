using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMCWindows.Utilities
{
    public class ErrorMessages
    {
        public void ShowErrorMessage(string errorKey)
        {
            string errorMessage = Properties.Resources.ResourceManager.GetString(errorKey);

            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                
                string generalError = Properties.Resources.ResourceManager.GetString("Error.GeneralException");
                MessageBox.Show(generalError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
