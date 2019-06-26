using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NP_HW_4.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetScreenButton(object sender, RoutedEventArgs e)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            server.SendTo(Encoding.ASCII.GetBytes("getScreen"), serverEP);
            byte[] buffer = new byte[8192 * 1024];
            while (true)
            {
                server.ReceiveFrom(buffer, ref serverEP);
                if (buffer.Length > 0)
                {
                    MemoryStream stream = new MemoryStream(buffer);
                    BitmapImage imageReceive = new BitmapImage();
                    imageReceive.StreamSource = stream;
                    image.Source = imageReceive;
                    break;
                }
            }
        }
    }
}
