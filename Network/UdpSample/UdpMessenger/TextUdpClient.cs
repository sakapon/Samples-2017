using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpMessenger
{
    public class TextUdpClient : IDisposable
    {
        UdpClient client;

        public TextUdpClient(int localPort, string remoteHost, int remotePort)
        {
            client = new UdpClient(localPort);
            client.Connect(remoteHost, remotePort);

            Task.Run(() =>
            {
                var remoteEP = default(IPEndPoint);

                while (true)
                {
                    try
                    {
                        var data = client.Receive(ref remoteEP);
                        if (!remoteEP.Equals(client.Client.RemoteEndPoint)) continue;

                        var text = Encoding.UTF8.GetString(data);
                        TextReceived?.Invoke(text);
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
        }

        public event Action<string> TextReceived;

        public void SendText(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            client.Send(data, data.Length);
        }

        ~TextUdpClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Close();
            }
        }
    }
}
