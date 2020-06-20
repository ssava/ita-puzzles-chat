using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItaSoftware.Puzzles.Chat.Tests
{
    [TestClass]
    public class StreamHandlingTests
    {
        [TestMethod]
        public void Client_receive_OK_on_valid_LOGIN()
        {
            string last = string.Empty;

            Client client = Client.Create();
            client.MessageReceived += (object sender, MessageArgs e) => last = e.Body;

            client.Connect("localhost", 3000);

            client.Send("LOGIN alice");

            Assert.AreEqual("OK\r\n", last);
        }

        [TestMethod]
        public void Client_receive_OK_when_joining_a_room()
        {
            string last = string.Empty;

            Client client = Client.Create();
            client.MessageReceived += (object sender, MessageArgs e) => last = e.Body;

            client.Connect("localhost", 3000);

            client.Send("LOGIN alice");
            Assert.AreEqual("OK\r\n", last);

            client.Send("JOIN #hello");
            Assert.AreEqual("OK\r\n", last);
        }

        [TestMethod]
        public void Client_receive_ERROR_when_joining_an_invalid_room()
        {
            string last = string.Empty;

            Client client = Client.Create();
            client.MessageReceived += (object sender, MessageArgs e) => last = e.Body;

            client.Connect("localhost", 3000);

            client.Send("LOGIN alice");
            Assert.AreEqual("OK\r\n", last);

            client.Send("JOIN hello");
            Assert.AreEqual("ERROR Invalid room name.\r\n", last);
        }
    }
}
