using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlogsPrajeesh.BlogSpot.WPFControls;

namespace CODLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            selectedGame.SelectedIndex = Properties.Settings.Default.SelectedGame;
        }

        public void selectedGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/CODLauncher;component/Images/" + ((ComboBoxItem)selectedGame.SelectedItem).Tag.ToString() + ".jpg");
            logo.EndInit();

            image1.Source = logo;

            Properties.Settings.Default.SelectedGame = selectedGame.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void ircConnect_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Username = ircNick.Text;
            Properties.Settings.Default.Save();
            var ircChat = new Window1(ircNick.Text, ((ComboBoxItem)selectedGame.SelectedItem).Tag.ToString());
            ircChat.Show();
        }

        private bool foundIP;
        private string gameIP;
        private string gamePass;
        private int countMenu;
        private ContextMenu menu;

        private void quickJoin_Click(object sender, RoutedEventArgs e)
        {
            menu = new ContextMenu();
            menu.IsOpen = true;
            
            foundIP = false;
            countMenu = 0;
            string clipBoard = Clipboard.GetText();
            string[] splitted = clipBoard.Split(' ');

            foreach (string word in splitted)
            {
                Regex re = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}[:][0-9]{1,5}$");
                Match m = re.Match(word);
                if (m.Success)
                {
                    textBlock1.Text = m.Groups[0].ToString();
                    gameIP = m.Groups[0].ToString();
                    foundIP = true;
                }
            }
            

            if (foundIP == false)
            {
                WPFMessageBox.Show("Could not find IP address in clipboard.");
            }
            else
            {
                List<string> passwords = SplitTextByWord(clipBoard, gameIP);
                string afterIP = passwords[1].ToString();
                string[] passwords2 = afterIP.Split(' ');

                foreach (string password in passwords2){
                    if (countMenu > 0)
                    {
                        MenuItem item = new MenuItem();
                        item.Header = password;
                        item.Click += new RoutedEventHandler(item_Click);
                        menu.Items.Add(item);
                    }
                    countMenu++;
                }
                menu.Items.Add(new Separator());
                MenuItem cancel = new MenuItem();
                cancel.Header = "Cancel";
                menu.Items.Add(cancel);
            }
        }

        private void item_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            WPFMessageBox.Show(mi.Header.ToString());
            gamePass = mi.Header.ToString();

            if (WPFMessageBox.Show("Connect to this server?", "Do you want to connect to " + gameIP + " with the password: " + gamePass + "?", WPFMessageBoxButtons.YesNo) == WPFMessageBoxResult.Yes)
            {
                launchGame();
            }
        }

        private void launchGame() 
        {
            
        }

        public static List<string> SplitTextByWord(string text, string splitTerm)
        {
            List<string> splitItems = new List<string>();
            if (string.IsNullOrEmpty(text)) return splitItems;
            if (string.IsNullOrEmpty(splitTerm))
            {
                splitItems.Add(text);
                return splitItems;
            }
            int nextPos = 0;
            int curPos = 0;
            while (nextPos > -1)
            {
                nextPos = text.IndexOf(splitTerm, curPos);
                if (nextPos != -1)
                {
                    splitItems.Add(text.Substring(curPos, nextPos - curPos));
                    curPos = nextPos + splitTerm.Length;
                }
            }
            splitItems.Add(text.Substring(curPos, text.Length - curPos));

            return splitItems;
        }

    }
}
