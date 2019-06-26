using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace NP_HW_4.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            var server = new Socket(
                AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var serverEP = new IPEndPoint(IPAddress.Any, 12345);
            server.Bind(serverEP);
            EndPoint remoteEP = new IPEndPoint(0, 0);
            var buffer = new byte[1024 * 4];
            var image = new byte[1000000];

            while (true)
            {
                int reciveSize = server.ReceiveFrom(buffer, ref remoteEP);
                if (Encoding.UTF8.GetString(buffer, 0, reciveSize) == "getScreen")
                {
                    Bitmap screen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics graphics = Graphics.FromImage(screen as Image);
                    graphics.CopyFromScreen(0, 0, 0, 0, screen.Size);
                    screen.Save("123.jpeg");

                    Console.WriteLine("screen done");

                    MemoryStream stream = new MemoryStream();
                    screen.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    var size = stream.Length;
                    image = stream.GetBuffer();
                    server.SendTo(image, remoteEP);

                }
            }
        }

    }
}
