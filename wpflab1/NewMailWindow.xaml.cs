using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using wpfTask1;

namespace wpflab1
{
    /// <summary>
    /// Interaction logic for NewMailWindow.xaml
    /// </summary>
    public partial class NewMailWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _to;
        private string _title;
        private string _body;
        public EmailMessage sentMessage;

        string Recipient { get { return _to; } set { _to = value; RaisePropertyChanged("Recipient"); } }
        string MailTitle { get { return _title; } set { _title = value; RaisePropertyChanged("Title"); } }
        string Body { get { return _body; } set { _body = value; RaisePropertyChanged("Body"); } }

        public NewMailWindow()
        {
            _to = "";
            _title = "";
            _body = "";
            sentMessage = null;

            InitializeComponent();
            DataContext = this;
            sendButton.IsEnabled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            sentMessage = null;
            Close();
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            sentMessage = new EmailMessage();
            sentMessage.Body = bodyTextBox.Text;
            sentMessage.Title = titleTextBox.Text;
            sentMessage.To = toTextBox.Text;
            sentMessage.Date = DateTime.Now.ToShortDateString();
            Close();
        }
    }
}
