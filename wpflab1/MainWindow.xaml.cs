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
        private ObservableCollection<EmailMessage> _messagesRcvd;
        private ObservableCollection<EmailMessage> _messagesSnt;

        public ObservableCollection<EmailMessage> allMessagesRcvd { get; set; }
        public ObservableCollection<EmailMessage> allMessagesSnt { get; set; }
        public ObservableCollection<EmailMessage> messagesRcvd { get { return _messagesRcvd; } set { _messagesRcvd = value; RaisePropertyChanged("messagesRcvd"); } }
        public ObservableCollection<EmailMessage> messagesSnt { get { return _messagesSnt; } set { _messagesSnt = value; RaisePropertyChanged("messagesSnt"); } }
        public EmailMessage selectedMessage { get { return _selectedMessage; } set { _selectedMessage = value; RaisePropertyChanged("selectedMessage"); } }
        public EmailUser usr { get { return _usr; } set { _usr = value; RaisePropertyChanged("usr"); } }

        public MainWindow()
        {
            _selectedMessage = new EmailMessage();

            allMessagesRcvd = new ObservableCollection<EmailMessage>();
            allMessagesRcvd = new ObservableCollection<EmailMessage>();
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
            CollectionView sentView = (CollectionView)CollectionViewSource.GetDefaultView(sntListBox.ItemsSource);
            CollectionView rcvdView = (CollectionView)CollectionViewSource.GetDefaultView(recvdListBox.ItemsSource);
            sentView.Filter = UserFilterSnt;
            rcvdView.Filter = UserFilterRecvd;
        }

        private bool UserFilterRecvd(object item)
        {
            if (String.IsNullOrEmpty(searchBar.Text))
                return true;
            else
            {
                string[] keywords = searchBar.Text.Split();
                EmailMessage message = item as EmailMessage;
                foreach(string s in keywords)
                    if (message.Date.Contains(s) || message.Title.Contains(s) || message.From.Contains(s))
                        return true;
                return false;
            }
        }

        private bool UserFilterSnt(object item)
        {
            if (String.IsNullOrEmpty(searchBar.Text))
                return true;
            else
            {
                string[] keywords = searchBar.Text.Split();
                EmailMessage message = item as EmailMessage;
                foreach (string s in keywords)
                    if (message.Date.Contains(s) || message.Title.Contains(s) || message.To.Contains(s))
                        return true;
                return false;
            }
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
                    {
                        allMessagesRcvd.Add(wnd.messagesRcvd[i]);
                        messagesRcvd.Add(wnd.messagesRcvd[i]);
                    }
                        
                    for (int i = 0; i < wnd.messagesSnt.Count; i++)
                    {
                        allMessagesRcvd.Add(wnd.messagesSnt[i]);
                        messagesRcvd.Add(wnd.messagesSnt[i]);
                    }

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(messagesCol.SelectedIndex == 0)
                CollectionViewSource.GetDefaultView(recvdListBox.ItemsSource).Refresh();
            else
                CollectionViewSource.GetDefaultView(sntListBox.ItemsSource).Refresh();
        }
    }
}
