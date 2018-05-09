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
        bool isLoggedIn;
        public ObservableCollection<EmailMessage> messages = null;
        public Window1()
        {
            isLoggedIn = false;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(isLoggedIn)
            {
                loginBox.Text = "Login";

            }
            else
            {
                EmailUser loggedUser;
                string login = loginBox.Text;
                string password = pswdBox.Password;
                bool success = EmailData.GetUserData(login, password, out loggedUser);
                if (success)
                {
                    messages = loggedUser.MessagesReceived;
                    isLoggedIn = true;
                    loginBox.Text = "Logout";
                    Close();
                }
                else
                    MessageBox.Show("Login failed");
            }
        }
    }
}
