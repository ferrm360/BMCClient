using BMCWindows.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Test.Validators.FieldsValidators
{
    public class FieldValidatorTest
    {
        [TestMethod]
        public void ValidateFieldsTestSuccess()
        {
            string bio = "Hola, esta es mi biografía";
            bool result = FieldValidator.AreFieldsEmpty(bio);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFieldsTestFailure() 
        {
            string user = " ";
            bool result = FieldValidator.AreFieldsEmpty(user);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePasswordSucces()
        {
            string password = "Contr@señ4";
            bool result = FieldValidator.ValidatePassword(password);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePasswordFailure()
        {
            string password = "fg";
            bool result = FieldValidator.ValidatePassword(password);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateEmailSucces()
        {
            string email = "correo.ma@gmail.com";
            bool result = FieldValidator.ValidateEmail(email);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateEmailFailed()
        {
            string email = "correocom";
            bool result = FieldValidator.ValidateEmail(email);
            Assert.IsFalse(result);
        }
    }
}
