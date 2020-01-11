using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DownhillPayClient;
using DownhillPayClient.UserControls;
using System.Windows.Controls;

namespace DownhillPayClientTests
{
    [TestClass]
    public class NewPersonalizedCardUCTests
    {
        readonly PrivateObject NewPersonalizedCardUC = new PrivateObject(new NewPersonalizedCardFormUserControl());

        private void SetTextboxText(string fieldToSet, string textToSet)
        {
            TextBox textBox = new TextBox
            {
                Text = textToSet
            };
            NewPersonalizedCardUC.SetField(fieldToSet, textBox);
        }

        private void IsFirstNameCorrectTest(string textboxText, bool assertValue)
        {
            SetTextboxText("firstNameTextBox", textboxText);
            Assert.AreEqual(assertValue, NewPersonalizedCardUC.Invoke("IsFirstNameCorrect"));
        }

        private void IsDayCorrectTest(string textboxText, bool assertValue)
        {
            SetTextboxText("dayTextBox", textboxText);
            Assert.AreEqual(assertValue, NewPersonalizedCardUC.Invoke("IsDayCorrect"));
        }

        private void IsMonthCorrectTest(string textboxText, bool assertValue)
        {
            SetTextboxText("monthTextBox", textboxText);
            Assert.AreEqual(assertValue, NewPersonalizedCardUC.Invoke("IsMonthCorrect"));
        }

        private void IsYearCorrectTest(string textboxText, bool assertValue)
        {
            SetTextboxText("yearTextBox", textboxText);
            Assert.AreEqual(assertValue, NewPersonalizedCardUC.Invoke("IsYearCorrect"));
        }

        #region first name textbox tests
        [TestMethod]
        public void IsFirstNameCorrectTestEmpty()
        {
            IsFirstNameCorrectTest("", false);
        }

        [TestMethod]
        public void IsFirstNameCorrectTestOneChar()
        {
            IsFirstNameCorrectTest("A", false);
        }

        [TestMethod]
        public void IsFirstNameCorrectTestTwoChars()
        {
            IsFirstNameCorrectTest("As", true);
        }
        #endregion

        #region day textbox tests
        [TestMethod]
        public void IsDayCorrectTestEmpty()
        {
            IsDayCorrectTest("", false);
        }

        [TestMethod]
        public void IsDayCorrectTestZero()
        {
            IsDayCorrectTest("0", false);
        }        
        
        [TestMethod]
        public void IsDayCorrectTestNegative()
        {
            IsDayCorrectTest("-1", false);
        }        
        
        [TestMethod]
        public void IsDayCorrectTest32()
        {
            IsDayCorrectTest("32", false);
        }  
        
        [TestMethod]
        public void IsDayCorrectTest1()
        {
            IsDayCorrectTest("1", true);
        }
        
        [TestMethod]
        public void IsDayCorrectTest31()
        {
            IsDayCorrectTest("31", true);
        }

        [TestMethod]
        public void IsDayCorrectNull()
        {
            IsDayCorrectTest(null, false);
        }
        #endregion

        #region month textbox tests
        [TestMethod]
        public void IsMonthCorrectTestNull()
        {
            IsMonthCorrectTest(null, false);
        }

        [TestMethod]
        public void IsMonthCorrectTest0()
        {
            IsMonthCorrectTest("0", false);
        }

        [TestMethod]
        public void IsMonthCorrectTest13()
        {
            IsMonthCorrectTest("13", false);
        }

        [TestMethod]
        public void IsMonthCorrectTest1()
        {
            IsMonthCorrectTest("1", true);
        }

        [TestMethod]
        public void IsMonthCorrectTest12()
        {
            IsMonthCorrectTest("12", true);
        }
        #endregion

        #region year textbox tests
        [TestMethod]
        public void IsYearCorrectTestNull()
        {
            IsYearCorrectTest(null, false);
        }

        [TestMethod]
        public void IsYearCorrectTestEmptyString()
        {
            IsYearCorrectTest("", false);
        }

        [TestMethod]
        public void IsYearCorrectTest1899()
        {
            IsYearCorrectTest("1899", false);
        }

        [TestMethod]
        public void IsYearCorrectTestMinus5()
        {
            IsYearCorrectTest(Convert.ToString(DateTime.Now.Year - 5), true);
        }

        [TestMethod]
        public void IsYearCorrectFuture()
        {
            IsYearCorrectTest(Convert.ToString(DateTime.Now.Year + 1), false);
        }
        #endregion
    }
}
