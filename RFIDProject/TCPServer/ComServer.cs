using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    class ComServer
    {
        private const int PORT_NO = 5000;

        private static int? GetReaderId(RFID_DBEntities db, string ipAddr)
        {
            IQueryable<RfIdReader> query = db.RfIdReader.Where(x => x.IPAddress == ipAddr);
            if (query.Count() >= 1)
                return query.First().IDReader;

            return null;
        }

        private static int? GetUserId(RFID_DBEntities db, string received)
        {
            IQueryable<RfIdCard> queryCard = db.RfIdCard.Where(x => x.SerialNumber == received);
            int cardId;
            if (queryCard.Count() >= 1)
                cardId = queryCard.First().IDCard;
            else
                return null;

            IQueryable<RfIdUser> queryUser = db.RfIdUser.Where(x => x.CardId == cardId);
            if (queryUser.Count() >= 1)
                return queryUser.First().IDRfIdUser;

            return null;
        }

        private static void getTCPMessage(TcpListener listener, out string ipAddress, out string dataReceived)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();

                IPAddress ip;
                if (IPAddress.TryParse(client.Client.RemoteEndPoint.ToString().Split(':')[0], out ip))
                {
                    ipAddress = ip.ToString();
                    Console.WriteLine("Parsed IP address: " + ipAddress);
                }
                else
                {
                    Logger.LogError("Could not parse IP address from: " + client.Client.RemoteEndPoint.ToString());
                    ipAddress = null;
                    dataReceived = null;

                    client.Close();
                    return;
                }

                NetworkStream networkStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
                dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received from: " + ipAddress + " message: " + dataReceived);
                client.Close();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                ipAddress = null;
                dataReceived = null;
            }
        }

        static void Main(string[] args)
        {
            using (RFID_DBEntities db = new RFID_DBEntities())
            {
                TcpListener listener = new TcpListener(IPAddress.Any, PORT_NO);
                listener.Start();

                while (true)
                {
                    Console.WriteLine("Inside the loop");

                    string ipAddr;
                    string dataReceived;

                    getTCPMessage(listener, out ipAddr, out dataReceived);
                    if (ipAddr == null || dataReceived == null)
                        continue;

                    try
                    {
                        int? idReader = GetReaderId(db, ipAddr);
                        if (idReader == null)
                            continue;

                        int? idUser = GetUserId(db, dataReceived);
                        if (idUser == null)
                            continue;

                        db.EntryLog.Add(new EntryLog
                        {
                            RfIdReaderId = idReader,
                            RfIdUserId = idUser,
                            EntryTime = DateTime.Now
                        });

                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                };
            }
        }
    }
}
