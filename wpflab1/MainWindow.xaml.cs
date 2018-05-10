using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfTask1;

namespace wpflab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        bool isLoggedIn;
        private EmailMessage _selectedMessage;
        private EmailUser _usr;
        public ObservableCollection<EmailMessage> messagesRcvd { get; set; }
        public ObservableCollection<EmailMessage> messagesSnt { get; set; }
        public EmailMessage selectedMessage { get { return _selectedMessage; } set { _selectedMessage = value; RaisePropertyChanged("selectedMessage"); } }
        public EmailUser usr { get { return _usr; } set { _usr = value; RaisePropertyChanged("usr"); } }

        public MainWindow()
        {
            _selectedMessage = new EmailMessage();
            messagesRcvd = new ObservableCollection<EmailMessage>();
            messagesSnt = new ObservableCollection<EmailMessage>();
            usr = new EmailUser();
            isLoggedIn = false;
            InitializeComponent();
            leftColumn.MinWidth = 0;
            leftColumn.Width = new GridLength(0, GridUnitType.Star);
            rightColumn.Width = new GridLength(1, GridUnitType.Star);
            gridColumn.Width = new GridLength(0, GridUnitType.Pixel);
            addMailButton.IsEnabled = false;
            DataContext = this;
            recvdListBox.ItemsSource = messagesRcvd;
            sntListBox.ItemsSource = messagesSnt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(isLoggedIn)
            {
                leftColumn.MinWidth = 0;
                leftColumn.Width = new GridLength(0, GridUnitType.Star);
                rightColumn.Width = new GridLength(1, GridUnitType.Star);
                gridColumn.Width = new GridLength(0, GridUnitType.Pixel);
                messagesSnt.Clear();
                messagesRcvd.Clear();
                usr = new EmailUser();

                addMailButton.IsEnabled = false;
                loginBtnText.Text = "Login";
                isLoggedIn = false;
            }
            else
            {

                Opacity = 0.5;
                Window1 wnd = new Window1();
                wnd.ShowInTaskbar = false;
                wnd.Owner = this;
                wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                wnd.ShowDialog();
                if (wnd.messagesRcvd != null)
                {
                    
                    gridColumn.Width = new GridLength(2, GridUnitType.Pixel);
                    leftColumn.Width = new GridLength(4, GridUnitType.Star);
                    rightColumn.Width = new GridLength(7, GridUnitType.Star);
                    leftColumn.MinWidth = 210;

                    addMailButton.IsEnabled = true;
                    loginBtnText.Text = "Logout";
                    for(int i = 0; i<wnd.messagesRcvd.Count; i++)
                        messagesRcvd.Add(wnd.messagesRcvd[i]);

                    for (int i = 0; i < wnd.messagesSnt.Count; i++)
                        messagesRcvd.Add(wnd.messagesSnt[i]);

                    usr = wnd.usr;
                    isLoggedIn = true;
                }
                Opacity = 1;
            }
        }

        private void addMailButton_Click(object sender, RoutedEventArgs e)
        {
            Opacity = 0.5;
            NewMailWindow wnd = new NewMailWindow();
            wnd.ShowInTaskbar = false;
            wnd.Owner = this;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.Width = 0.8 * Width;
            wnd.Height = 0.8 * Height;
            wnd.ShowDialog();
            if(wnd.sentMessage != null)
                messagesSnt.Add(wnd.sentMessage);

            Opacity = 1;
        }

        private void recvdListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                messagesRcvd.Remove(selectedMessage);
                selectedMessage = null;
            }
        }

        private void sntListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                messagesSnt.Remove(selectedMessage);
                selectedMessage = null;
            }
        }
    }
}
