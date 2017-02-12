using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DateServer {
    internal class Program {
        public static void StartListening() {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localendPoint = new IPEndPoint(ipAddress, 8080);

            // create socket
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            // bind to local endpoint and listen for connections
            try {
                listener.Bind(localendPoint);
                listener.Listen(10);

                while (true) {
                    Console.WriteLine("Waiting for a connection");
                    Socket handler = listener.Accept();

                    // get current date
                    var now = DateTime.Now.ToString();
                    // encode data for client
                    byte[] date = Encoding.ASCII.GetBytes(now);
                    Console.WriteLine("Sent current time to {0}", handler.RemoteEndPoint.ToString());

                    handler.Send(date);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void Main(string[] args) {
            StartListening();
        }
    }
}