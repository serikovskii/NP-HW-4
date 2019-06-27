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
            BitmapImage imageReceive = new BitmapImage();

            server.SendTo(Encoding.ASCII.GetBytes("getScreen"), serverEP);
            byte[] buffer = new byte[70 * 1024];
            FileStream stream = new FileStream("image.jpeg", FileMode.Create);
            var byetFileBlock = new byte[1550];
            long sizeFile = 0;
            int countFileBlock = 0;
            int size = server.ReceiveFrom(byetFileBlock, ref serverEP);
            countFileBlock = BitConverter.ToInt32(byetFileBlock, 0);
            while (countFileBlock > 0)
            {
                size = server.ReceiveFrom(buffer, ref serverEP);

                
                stream.Write(buffer, 0, size);
                sizeFile += size;
                countFileBlock--;

            }
            MessageBox.Show("prishel");
            stream.Close();

            //BitmapImage screen = new BitmapImage();
            //screen.BeginInit();
            //screen.UriSource = new Uri("image.jpeg");
            //screen.EndInit();

            //image.Source = screen;

        }
    }
}
