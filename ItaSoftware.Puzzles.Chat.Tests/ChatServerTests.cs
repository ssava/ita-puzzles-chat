using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItaSoftware.Puzzles.Chat.Tests
{
    [TestClass]
    public class ChatServerTests
    {
        [TestMethod]
        public void Create_a_new_server_instance()
        {
            IChatServer server = new ChatServer();

            Assert.IsNotNull(server);
        }
        
        [TestMethod]
        public void New_instance_is_listening_to_default_port()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
        }

        [TestMethod]
        public void New_instance_is_not_running()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
            Assert.IsFalse(server.IsRunning());
        }

        [TestMethod]
        public void Server_is_running_when_started()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();
            new Task(() => server.Start());

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
            Assert.IsTrue(server.IsRunning());
        }

        [TestMethod]
        public void Server_is_not_running_when_stopped()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();
            server.Start();
            server.Stop();

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
            Assert.IsFalse(server.IsRunning());
        }

        [TestMethod]
        public void Server_has_a_command_parser()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();
            server.Start();

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
            Assert.IsTrue(server.IsRunning());
            Assert.IsNotNull(((ChatServer)server).Parser);
        }

        [TestMethod]
        public void Server_has_a_context()
        {
            ushort listeningPort = 30000;
            IChatServer server = new ChatServer();
            server.Start();

            Assert.IsNotNull(server);
            Assert.AreEqual(listeningPort, server.ListeningPort);
            Assert.IsTrue(server.IsRunning());
            Assert.IsNotNull(((ChatServer)server).Parser);
            Assert.IsNotNull(((ChatServer)server).Context);
        }
    }
}
