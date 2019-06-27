using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

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
            var image = new byte[1000000];
            int blockSize = 1500;
            var buffer = new byte[blockSize];

            while (true)
            {
                int reciveSize = server.ReceiveFrom(buffer, ref remoteEP);
                if (Encoding.UTF8.GetString(buffer, 0, reciveSize) == "getScreen")
                {
                    Bitmap screen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics graphics = Graphics.FromImage(screen as Image);
                    graphics.CopyFromScreen(0, 0, 0, 0, screen.Size);
                    screen.Save("123.bmp");

                    

                    //MemoryStream stream = new MemoryStream();
                    //screen.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    //var size = stream.Length;
                    //image = stream.GetBuffer();
                    //server.SendTo(image, remoteEP);

                    using (FileStream stream = new FileStream("123.bmp", FileMode.Open))
                    {
                        long sizeImage = stream.Length;
                        long countRead = sizeImage / blockSize;
                        long remaindSize = sizeImage % blockSize;

                        server.SendTo(BitConverter.GetBytes(countRead+1), remoteEP);

                        for (int i = 0; i < countRead; i++)
                        {
                            stream.Read(buffer, 0, blockSize);
                            server.SendTo(buffer, remoteEP);
                            Thread.Sleep(10);
                        }

                        if (remaindSize > 0)
                        {
                            stream.Read(buffer, 0, (int)remaindSize);
                            server.SendTo(buffer, 0, (int)remaindSize, SocketFlags.None, remoteEP);

                        }
                        Console.WriteLine("screen done");
                    }


                }
            }
        }

    }
}
