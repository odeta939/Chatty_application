using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_application.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }

        // TODO make a function to read a opcode instead of using .ReadByte()

        public int ReadOpcode()
        {
            int opcode;
            opcode = _networkStream.ReadByte();
            return opcode;
        }
        public string ReadMessage()
        {
            byte[] messageBuffer;
            var length = ReadInt32();
            messageBuffer = new byte[length];
            _networkStream.Read(messageBuffer, 0, length);
            var message = Encoding.ASCII.GetString(messageBuffer);
            return message;
        }
    }
}
