using Chatty_application.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_application.Net
{
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action messageReceivedEvent;
        public event Action disconnectedEvent;

        // Constructor
        public Server()
        {

            // Instantiate new client
            _client = new TcpClient();


        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(10);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }


                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            /* 
             * To avoid deadlocking the application, need to offload this data
             * to another thread
             */
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadOpcode();
                    switch (opcode)
                    {
                        case 20:
                            connectedEvent?.Invoke();
                            break;
                        case 30:
                            messageReceivedEvent?.Invoke();
                            break;
                        case 40:
                            disconnectedEvent?.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            });

        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(30);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());

        }
    }
}
