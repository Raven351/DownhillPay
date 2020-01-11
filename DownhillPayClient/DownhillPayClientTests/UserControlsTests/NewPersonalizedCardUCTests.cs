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
    }
}
