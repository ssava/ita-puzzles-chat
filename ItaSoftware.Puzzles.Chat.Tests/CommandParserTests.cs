using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ItaSoftware.Puzzles.Chat.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void Server_responds_ERROR_to_unsupported_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("BADCOMMAND\r\n");

            Assert.AreEqual("ERROR Unsupported command\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_invalid_arguments()
        {
            CommandParser parser = new CommandParser();

            string output = parser.Execute(null);

            Assert.AreEqual("ERROR Unsupported command\r\n", output);
        }

        [TestMethod]
        public void ERROR_doesnt_alter_state()
        {
            ServerContext context = ServerContext.Create(); /* Create new server context */
            CommandParser parser = new CommandParser();
            
            _ = parser.ReplyFor("BADCOMMAND\r\n", context);

            Assert.AreEqual(0, context.LoggedUserCount);
        }

        [TestMethod]
        public void Server_responds_OK_to_valid_LOGIN_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("LOGIN user\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_wo_user()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("LOGIN\r\n");

            Assert.AreEqual("ERROR Need to specify a username.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("LOGIN \r\n");

            Assert.AreEqual("ERROR Need to specify a username.\r\n", output);
        }

        [TestMethod]
        public void Valid_LOGIN_command_increments_users_count()
        {
            ServerContext context = ServerContext.Create(); /* Create new server context */
            CommandParser parser = new CommandParser();

            parser.ReplyFor("LOGIN alice\r\n", context);
            parser.ReplyFor("LOGIN bob\r\n", context);
            parser.ReplyFor("LOGIN charlie\r\n", context);

            Assert.AreEqual(3, context.LoggedUserCount);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_with_existing_user()
        {
            ServerContext context = ServerContext.Create(); /* Create new server context */
            CommandParser parser = new CommandParser();

            parser.ReplyFor("LOGIN alice\r\n", context);
            string output = parser.ReplyFor("LOGIN alice\r\n", context);

            Assert.AreEqual("ERROR User already logged in.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_dont_alter_users_count()
        {
            ServerContext context = ServerContext.Create(); /* Create new server context */
            CommandParser parser = new CommandParser();
            UserContext aliceCtx = new UserContext();
            UserContext bobCtx = new UserContext();

            parser.ReplyFor("LOGIN alice\r\n", context, aliceCtx);
            parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);
            string output = parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);

            Assert.AreEqual("ERROR User already logged in.\r\n", output);
            Assert.AreEqual(2, context.LoggedUserCount);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_LOGIN_command_to_already_logged_user()
        {
            ServerContext context = ServerContext.Create(); /* Create new server context */
            CommandParser parser = new CommandParser();
            UserContext aliceCtx = new UserContext();
            UserContext bobCtx = new UserContext();

            parser.ReplyFor("LOGIN alice\r\n", context, aliceCtx);
            parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);
            string output = parser.ReplyFor("LOGIN alice\r\n", context, bobCtx);

            Assert.AreEqual("ERROR User already logged in.\r\n", output);
            Assert.AreEqual(2, context.LoggedUserCount);
        }

        [TestMethod]
        public void Server_responds_OK_to_JOIN_command()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();

            string output = parser.ReplyFor("JOIN #hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_wo_room()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("JOIN\r\n");

            Assert.AreEqual("ERROR You need to specify a room to join.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("JOIN \r\n");

            Assert.AreEqual("ERROR You need to specify a room to join.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_with_invalid_room_name()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("JOIN invalidname\r\n");

            Assert.AreEqual("ERROR Invalid room name.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_wo_server_context()
        {
            CommandParser parser = new CommandParser();
            UserContext userCtx = new UserContext();
            string output = parser.ReplyFor("JOIN #invalidname\r\n", userCtx: userCtx);

            Assert.AreEqual("ERROR You must login first.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_JOIN_command_with_user_context()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();

            string output = parser.ReplyFor("JOIN #invalidname\r\n", context, userContext);

            Assert.AreEqual("ERROR You must login first.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_OK_to_JOIN_command_with_user_context()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();

            parser.ReplyFor("LOGIN alice\r\n", context, userContext);
            string output = parser.ReplyFor("JOIN #meeting\r\n", context, userContext);

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_set_correct_rooms_after_user_execute_JOIN_command()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();

            parser.ReplyFor("LOGIN alice\r\n", context, userContext);
            parser.ReplyFor("JOIN #meeting\r\n", context, userContext);
            string output = parser.ReplyFor("JOIN #meeting2\r\n", context, userContext);

            string[] joined = userContext.JoinedRooms.ToArray();

            Assert.AreEqual(2, userContext.JoinedRooms.Count);
            Assert.AreEqual("#meeting", joined[0]);
            Assert.AreEqual("#meeting2", joined[1]);
        }

        [TestMethod]
        public void Server_responds_OK_to_PART_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("PART #hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_wo_room()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("PART\r\n");

            Assert.AreEqual("ERROR You need to specify a room to part.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_with_trail_space()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("PART \r\n");

            Assert.AreEqual("ERROR You need to specify a room to part.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_with_invalid_room_name()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("PART invalidname\r\n");

            Assert.AreEqual("ERROR Invalid room name.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_OK_to_PART_command_and_remove_from_joined_rooms()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();
            parser.ReplyFor("LOGIN alice\r\n", context, userContext);
            parser.ReplyFor("JOIN #hello\r\n", context, userContext);
            parser.ReplyFor("JOIN #hello2\r\n", context, userContext);
            parser.ReplyFor("PART #hello\r\n", context, userContext);

            string[] rooms = userContext.JoinedRooms.ToArray();

            Assert.AreEqual(1, rooms.Length);
            Assert.AreEqual("#hello2", rooms[0]);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_PART_command_from_not_joined_room()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();
            parser.ReplyFor("LOGIN alice\r\n", context, userContext);
            parser.ReplyFor("JOIN #hello\r\n", context, userContext);
            parser.ReplyFor("JOIN #hello2\r\n", context, userContext);
            string output = parser.ReplyFor("PART #hello3\r\n", context, userContext);

            string[] rooms = userContext.JoinedRooms.ToArray();

            Assert.AreEqual("ERROR You haven't joined this room.\r\n", output);
            Assert.AreEqual(2, rooms.Length);
            Assert.AreEqual("#hello", rooms[0]);
            Assert.AreEqual("#hello2", rooms[1]);
        }


        [TestMethod]
        public void Server_responds_OK_to_MSG_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("MSG #hello msg-hello\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_OK_to_MSG_command_with_user_context()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();
            parser.ReplyFor("LOGIN alice", context, userContext);
            parser.ReplyFor("JOIN #hello", context, userContext);
            string output = parser.ReplyFor("MSG #hello msg-hello\r\n", context, userContext);

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_to_not_joined_room()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();
            parser.ReplyFor("LOGIN alice", context, userContext);
            parser.ReplyFor("JOIN #hello", context, userContext);
            string output = parser.ReplyFor("MSG #hello2 msg-hello\r\n", context, userContext);

            Assert.AreEqual("ERROR You haven't joined #hello2 room.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_wo_message()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("MSG #hello\r\n");

            Assert.AreEqual("ERROR You need to specify a room/user and a message to send.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_wo_room_and_message()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("MSG\r\n");

            Assert.AreEqual("ERROR You need to specify a room/user and a message to send.\r\n", output);
        }

        [TestMethod]
        public void Server_responds_ERROR_to_MSG_command_to_not_logged_user()
        {
            CommandParser parser = new CommandParser();
            ServerContext context = ServerContext.Create();
            UserContext userContext = new UserContext();

            parser.ReplyFor("LOGIN alice\r\n", context, userContext);
            string output = parser.ReplyFor("MSG bob hello.\r\n", context, userContext);

            Assert.AreEqual("ERROR User bob is currently not logged in.\r\n", output);
        }

        //[TestMethod]
        //public void Server_sends_a_message_between_two_users()
        //{
        //    CommandParser parser = new CommandParser();
        //    ServerContext context = ServerContext.Create();
        //    UserContext aliceCtx = new UserContext();
        //    UserContext bobCtx = new UserContext();
        //    string output = string.Empty;

        //    output = parser.Execute("LOGIN alice", context, aliceCtx);
        //    Assert.AreEqual("OK\r\n", output);

        //    output = parser.Execute("LOGIN bob", context, bobCtx);
        //    Assert.AreEqual("OK\r\n", output);

        //    output = parser.Execute("MSG bob hello, bob.\r\n", context, aliceCtx);
        //    Assert.AreEqual("OK\r\n", output);
        //    Assert.AreEqual(1, bobCtx.Messages.Count);
        //    Assert.AreEqual("GOTUSERMSG alice hello, bob", bobCtx.Messages.Peek());
        //}


        [TestMethod]
        public void Server_responds_OK_to_LOGOUT_command()
        {
            CommandParser parser = new CommandParser();
            string output = parser.ReplyFor("LOGOUT\r\n");

            Assert.AreEqual("OK\r\n", output);
        }

        [TestMethod]
        public void Server_decrease_users_count_on_LOGOUT_command()
        {
            ServerContext context = ServerContext.Create();
            CommandParser parser = new CommandParser();

            parser.ReplyFor("LOGIN hello\r\n", context);
            parser.ReplyFor("LOGIN world\r\n", context);
            parser.ReplyFor("LOGOUT\r\n", context);

            Assert.AreEqual(2, context.LoggedUserCount);
        }
    }
}
