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

        private static bool getTCPMessage(TcpListener listener, out string ipAddress, out string dataReceived)
        {
            ipAddress = null;
            dataReceived = null;

            try
            {
                TcpClient client = listener.AcceptTcpClient();

                if (!IPAddress.TryParse(client.Client.RemoteEndPoint.ToString().Split(':')[0], out IPAddress ip))
                {
                    Logger.LogError("Could not parse IP address from: " + client.Client.RemoteEndPoint.ToString());
                    client.Close();

                    return false;
                }

                ipAddress = ip.ToString();
                NetworkStream networkStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
                dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                client.Close();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return false;
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
                    string ipAddr;
                    string dataReceived;

                    if (!getTCPMessage(listener, out ipAddr, out dataReceived))
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

                        Console.WriteLine("Saved data: RfIdReader: {0}, User: {1}, Time: {2}", idReader, idUser, DateTime.Now);
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
