using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UdpMessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Local Addresses:");
            foreach (var address in GetHostAddresses())
                Console.WriteLine(address);

            var localPort = int.Parse(GetInput("Local Port: ", "8100"));
            var remoteHost = GetInput("Remote Address: ", "localhost");
            var remotePort = int.Parse(GetInput("Remote Port: ", "8101"));

            try
            {
                using (var client = new TextUdpClient(localPort, remoteHost, remotePort))
                {
                    client.TextReceived += s => Console.WriteLine($"Received: {s}");

                    var timer = new Timer(o =>
                    {
                        var text = $"{DateTime.Now:HH:mm:ss.fff}";
                        client.SendText(text);
                        Console.WriteLine($"Sent: {text}");
                    },
                    null, 1000, 2000);

                    Console.WriteLine();
                    Console.WriteLine("Press [Enter] key to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static IEnumerable<IPAddress> GetHostAddresses() =>
            Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork);

        static string GetInput(string message = "", string defaultValue = "")
        {
            Console.Write(message);
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }
    }
}
