using BMCWindows.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMCWindows.Test
{
    [TestClass]
    public class ValidatorsTest
    {
        
        
        [TestMethod]
        public void validatePasswordTestFails()
        {
            string password = "12345";
            bool result = FieldValidator.ValidatePassword(password);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void validatePasswordTestSuccess() 
        {
            string password = "Contr@señ4";
            bool result = FieldValidator.ValidatePassword(password);
            Assert.IsTrue(result);
        }

    }
}
