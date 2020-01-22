using System;
using System.Net.Sockets;
using System.Text;

namespace TCPSender
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "192.168.0.16";

        static void Main(string[] args)
        {
            while (true)
            {
                TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
                NetworkStream networkStream = client.GetStream();
                Console.Write("Message: ");
                string message = Console.ReadLine();
                byte[] bytesToSend = Encoding.ASCII.GetBytes(message);

                Console.WriteLine("Sending: " + message);
                networkStream.Write(bytesToSend, 0, bytesToSend.Length);
                
                networkStream.Close();
                client.Close();
            }
        }
    }
}
