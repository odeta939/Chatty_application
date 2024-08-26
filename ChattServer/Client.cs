using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Client
    {
        // todo Rename this class to something better descriptive?
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader { get; set; }

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            // TODO should verify that the opcode is same code we send from the app
            var opcode = _packetReader.ReadByte();

            Username = _packetReader.ReadMessage();
            Console.WriteLine($"[{DateTime.Now}]: User {Username} has connected to the server");
            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    string message;
                    switch (opcode)
                    {
                        case 30:
                            message = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: {Username}: {message}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: {Username}: {message}");
                            break;

                        default: break;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine($"[{Username}]: Disconnected");
                    Program.BroadcastDisconnectMessage(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
