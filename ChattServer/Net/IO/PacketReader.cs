﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
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
