﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesla.IO;
using Tesla.Net;

namespace Tesla.Core.Tests.Net
{
    [TestClass]
    public class SocketServerTests
    {
        [TestMethod]
        public void ServerGenericTest()
        {
            var sample = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            var handler = new Func<Socket, Task>(async sock =>
            {
                var stream = sock.GetStream();
                var bytes = new byte[sample.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            });

            var server = new SocketServer(handler, 0);
            server.Start();

            var client = new TcpClient();
            client.Connect(IPAddress.Loopback, server.Port);

            var networkStream = client.GetStream();
            networkStream.Write(sample, 0, sample.Length);
            var response = networkStream.ReadToTimeout();
            //var response = new byte[sample.Length];
            //networkStream.Read(response, 0, response.Length);

            CollectionAssert.AreEqual(sample, response);
        }
    }
}
