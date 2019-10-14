using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItaSoftware.Puzzles.Chat.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void Server_responds_ERROR_to_unsupported_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("BADCOMMAND\r\n");

            Assert.AreEqual("ERROR Unsupported command\r\n", output);
        }

        [TestMethod]
        public void ERROR_doesnt_alter_state()
        {
            ServerContext context = ServerContext.Instance; /* Get current server context */
            CommandParser parser = new CommandParser();
            
            _ = parser.Execute("BADCOMMAND\r\n", context);

            Assert.AreEqual(0, context.ConnectedUsers);
        }

        [TestMethod]
        public void Server_responds_OK_to_valid_LOGIN_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("LOGIN user\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_wo_user()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("LOGIN\r\n");

            Assert.AreEqual("ERROR Need to specify a username.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("LOGIN \r\n");

            Assert.AreEqual("ERROR Need to specify a username.\r\n", output);
        }

        [TestMethod]
        public void Valid_LOGIN_command_increments_users_count()
        {
            ServerContext context = ServerContext.Instance; /* Get current server context */
            CommandParser parser = new CommandParser();

            parser.Execute("LOGIN alice\r\n", context);
            parser.Execute("LOGIN bob\r\n", context);
            parser.Execute("LOGIN charlie\r\n", context);

            Assert.AreEqual(3, context.ConnectedUsers);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_with_existing_user()
        {
            ServerContext context = ServerContext.Instance; /* Get current server context */
            CommandParser parser = new CommandParser();

            parser.Execute("LOGIN alice\r\n", context);
            string output = parser.Execute("LOGIN alice\r\n", context);

            Assert.AreEqual("ERROR User already logged in.\r\n", output);
            Assert.AreEqual(1, context.ConnectedUsers);
        }

        [TestMethod]
        public void Server_responds_OK_to_JOIN_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("JOIN #hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_wo_room()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("JOIN\r\n");

            Assert.AreEqual("ERROR You need to specify a room to join.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("JOIN \r\n");

            Assert.AreEqual("ERROR You need to specify a room to join.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_with_invalid_room_name()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("JOIN invalidname\r\n");

            Assert.AreEqual("ERROR Invalid room name.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_OK_to_PART_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("PART #hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_wo_room()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("PART\r\n");

            Assert.AreEqual("ERROR You need to specify a room to part.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("PART \r\n");

            Assert.AreEqual("ERROR You need to specify a room to part.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_with_invalid_room_name()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("PART invalidname\r\n");

            Assert.AreEqual("ERROR Invalid room name.\r\n", output);
        }


        [TestMethod]
        public void Server_responds_OK_to_MSG_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("MSG #hello msg-hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_wo_message()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("MSG #hello\r\n");

            Assert.AreEqual("ERROR You need to specify a room/user and a message to send.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_wo_room_and_message()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("MSG\r\n");

            Assert.AreEqual("ERROR You need to specify a room/user and a message to send.\r\n", output);
        }


        [TestMethod]
        public void Server_responds_OK_to_LOGOUT_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.Execute("LOGOUT\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_decrease_users_count_on_LOGOUT_command()
        {
            ServerContext context = ServerContext.Instance;
            CommandParser parser = new CommandParser();

            parser.Execute("LOGIN hello\r\n");
            parser.Execute("LOGOUT\r\n", context);

            Assert.AreEqual(0, context.ConnectedUsers);
        }
    }
}
