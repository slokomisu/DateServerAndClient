using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DateClient {
    internal class Program {
        public static void StartClient() {
            // buffer for incoming data
            byte[] bytes = new byte[1024];

            // connect to remote device
            try {
                IPHostEntry ipHostInfo = Dns.Resolve("noahguillory.me");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);

                // create socket
                Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp );

                try {
                    clientSocket.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}",
                        clientSocket.RemoteEndPoint.ToString());
                    int bytesRec = clientSocket.Receive(bytes);
                    Console.WriteLine("Current time: {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                } catch (ArgumentNullException ane) {
                    Console.WriteLine("ArgumentNullException : {0}",ane.ToString());
                } catch (SocketException se) {
                    Console.WriteLine("SocketException : {0}",se.ToString());
                } catch (Exception e) {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            } catch (Exception e) {
                Console.WriteLine( e.ToString());
            }

        }

        public static void Main(string[] args) {
            StartClient();
        }
    }
}