using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DownhillPayClient.MessageBoxLayout
{
    /// <summary>
    /// Interaction logic for MessageBoxLayoutInfo.xaml
    /// </summary>
    public partial class MessageBoxLayoutInfo : Window
    {
        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                messageTextBlock.Text = value;
            }
        }

        public MessageBoxLayoutInfo()
        {
            InitializeComponent();
        }

        public MessageBoxLayoutInfo(string message) : this()
        {
            messageTextBlock.Text = message;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
