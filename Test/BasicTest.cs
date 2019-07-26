using System;
using NUnit.Framework;
using websocket;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Test started.");
        }

        [Test]
        public void SendReceiveTest()
        {
            WebSocketConnection wb = new WebSocketConnection();
            wb.OnConnect("ws://echo.websocket.org");

            wb.OnMessageSend("hello! How are you?");
            var message = wb.OnMessageReceive();
            Console.WriteLine(message);
        }
    }
}