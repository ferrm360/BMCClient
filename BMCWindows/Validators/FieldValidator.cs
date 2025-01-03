﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BMCWindows.Validators
{
    public class FieldValidator
    {

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                return false; 
            }

            Regex upperCasePattern = new Regex("[A-Z]");
            Regex lowerCasePattern = new Regex("[a-z]");
            Regex digitPattern = new Regex("[0-9]");
            Regex specialCharacterPattern = new Regex("[^a-zA-Z0-9]");

            bool hasUpperCase = upperCasePattern.IsMatch(password);
            bool hasLowerCase = lowerCasePattern.IsMatch(password);
            bool hasDigit = digitPattern.IsMatch(password);
            bool hasSpecialCharacter = specialCharacterPattern.IsMatch(password);

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialCharacter;
        }

        public static bool ValidateEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }

        
    

        public static bool AreFieldsEmpty(params string[] fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    return true; 
                }
            }
            return false; 
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

    }
}
