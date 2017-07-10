using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpReceiver
{
    class Program
    {
        const int ReceiverPort = 8100;

        static void Main(string[] args)
        {
            Console.WriteLine("IP addresses of this computer:");
            foreach (var address in GetHostAddresses())
                Console.WriteLine(address);

            var client = new UdpClient(ReceiverPort);

            Task.Run(() =>
            {
                var senderEP = default(IPEndPoint);

                while (true)
                {
                    try
                    {
                        var data = client.Receive(ref senderEP);
                        var text = Encoding.UTF8.GetString(data);

                        Console.WriteLine($"{senderEP} {DateTime.Now:HH:mm:ss.fff}: {text}");
                    }
                    catch (SocketException ex)
                    {
                        switch (ex.SocketErrorCode)
                        {
                            case SocketError.ConnectionReset:
                                continue;
                            case SocketError.Interrupted:
                                return;
                            default:
                                throw;
                        }
                    }
                }
            });

            Console.WriteLine("Press [Enter] key to exit.");
            Console.ReadLine();

            client.Close();
        }

        static IEnumerable<IPAddress> GetHostAddresses() =>
            Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork);
    }
}
