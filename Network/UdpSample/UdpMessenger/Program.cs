using System;
using System.Collections.Generic;
using System.Linq;

namespace UdpMessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var localPort = int.Parse(GetInput("Local Port: ", "8100"));
                var remoteHost = GetInput("Remote Host: ", "localhost");
                var remotePort = int.Parse(GetInput("Remote Port: ", "8101"));

                using (var client = new TextUdpClient(localPort, remoteHost, remotePort))
                {
                    client.TextReceived += s => Console.WriteLine($"Received: {s}");

                    Console.WriteLine();
                    Console.WriteLine("Input message.");
                    Console.WriteLine("Input [x] to exit.");

                    foreach (var message in GetInputs())
                    {
                        client.SendText(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string GetInput(string message = "", string defaultValue = "")
        {
            Console.Write(message);
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        static IEnumerable<string> GetInputs(string message = "", string defaultValue = "")
        {
            while (true)
            {
                var input = GetInput(message, defaultValue);
                if (string.Equals(input, "x", StringComparison.InvariantCultureIgnoreCase)) yield break;
                yield return input;
            }
        }
    }
}
