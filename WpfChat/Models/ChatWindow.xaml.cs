using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace WpfChat.Models
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow(ref TcpClient client, ref NetworkStream Stream)
        {
            InitializeComponent();
            this.DataContext = new ChatModel(ref client, ref Stream);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
