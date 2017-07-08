using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpSender
{
    class Program
    {
        const string ReceiverAddress = "127.0.0.1";
        const int ReceiverPort = 8100;

        static void Main(string[] args)
        {
            var client = new UdpClient(ReceiverAddress, ReceiverPort);

            var timer = new Timer(o =>
            {
                var text = $"{DateTime.Now:HH:mm:ss.fff}";
                var data = Encoding.UTF8.GetBytes(text);
                client.Send(data, data.Length);
                Console.WriteLine(text);
            },
            null, 1000, 1000);

            Console.WriteLine("Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}
