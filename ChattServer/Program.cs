
using ChatServer.Net.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static TcpListener? _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                Console.WriteLine(client.Username);
                /*Broadcast the connection to everyone on the server */
                BroadcastConnection();


            }

        }

        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();

                    broadcastPacket.WriteOpCode(20);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());

                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {

                var broadcastPacket = new PacketBuilder();

                broadcastPacket.WriteOpCode(30);
                broadcastPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());


            }

        }

        public static void BroadcastDisconnectMessage(string uid)
        {
            var disconnectedUser = _users.Where(user => user.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);
            foreach (var user in _users)
            {

                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(40);
                broadcastPacket.WriteMessage(user.Username);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"[{disconnectedUser.Username}]: Disconnected");

        }
    }
}