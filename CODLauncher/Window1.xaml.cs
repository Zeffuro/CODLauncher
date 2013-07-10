using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CODLauncher
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1(string nick, string game)
        {
            InitializeComponent();
            Uri url = new Uri("http://webchat.quakenet.org/?channels=" + game + ".wars&nick=" + nick);
            ircChat.Navigate(url);
        }
    }
}
