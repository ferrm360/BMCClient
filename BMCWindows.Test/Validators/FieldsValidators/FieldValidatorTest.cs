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
    }
}
