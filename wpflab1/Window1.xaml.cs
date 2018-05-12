using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public ObservableCollection<EmailMessage> messagesRcvd = null;
        public ObservableCollection<EmailMessage> messagesSnt = null;
        public EmailUser usr = null;
        public Window1()
        {
            
            InitializeComponent();
            DataContext = this;
            LOGINBTN.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool checkCredentials()
        {
            EmailUser loggedUser;
            string login = loginBox.Text;
            string password = pswdBox.Password;
            bool success = EmailData.GetUserData(login, password, out loggedUser);
            if (success)
            {
                usr = loggedUser;
                messagesRcvd = loggedUser.MessagesReceived;
                messagesSnt = loggedUser.MessagesSent;
                return true;
            }
            else
            {
                usr = null;
                messagesRcvd = null;
                messagesSnt = null;
                return false;
            }
                
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkCredentials())
                Close();
            else
                MessageBox.Show("Login failed");
        }

        private void pswdBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (checkCredentials())
                    Close();
                else
                    MessageBox.Show("Login failed");
        }

        private void loginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loginBox.Text.Length != 0 && pswdBox.Password.Length != 0)
                LOGINBTN.IsEnabled = true;
            else
                LOGINBTN.IsEnabled = false;
        }

        private void pswdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text.Length != 0 && pswdBox.Password.Length != 0)
                LOGINBTN.IsEnabled = true;
            else
                LOGINBTN.IsEnabled = false;
        }
    }
}
