using FluentAssertions;
using System.Linq;
using Xunit;

namespace ItaSoftware.Puzzles.Chat.Tests;

public class CommandParserTests
{
    [Fact]
    public void Server_responds_ERROR_to_unsupported_command()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("BADCOMMAND\r\n");

        output.Should().Be("ERROR Unsupported command\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_invalid_arguments()
    {
        CommandParser parser = new();

        string output = parser.Execute(null);

        output.Should().Be("ERROR Unsupported command\r\n");
    }

    [Fact]
    public void ERROR_doesnt_alter_state()
    {
        ServerContext context = ServerContext.Create(); /* Create new server context */
        CommandParser parser = new();

        _ = parser.ReplyFor("BADCOMMAND\r\n", context);

        context.LoggedUserCount.Should().Be(0);
    }

    [Fact]
    public void Server_responds_OK_to_valid_LOGIN_command()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("LOGIN user\r\n");

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_LOGIN_command_wo_user()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("LOGIN\r\n");

        output.Should().Be("ERROR Need to specify a username.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_LOGIN_command_with_trail_space()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("LOGIN \r\n");

        output.Should().Be("ERROR Need to specify a username.\r\n");
    }

    [Fact]
    public void Valid_LOGIN_command_increments_users_count()
    {
        ServerContext context = ServerContext.Create(); /* Create new server context */
        CommandParser parser = new();

        parser.ReplyFor("LOGIN alice\r\n", context);
        parser.ReplyFor("LOGIN bob\r\n", context);
        parser.ReplyFor("LOGIN charlie\r\n", context);

        context.LoggedUserCount.Should().Be(3);
    }

    [Fact]
    public void Server_responds_ERROR_to_LOGIN_command_with_existing_user()
    {
        ServerContext context = ServerContext.Create(); /* Create new server context */
        CommandParser parser = new();

        parser.ReplyFor("LOGIN alice\r\n", context);
        string output = parser.ReplyFor("LOGIN alice\r\n", context);

        output.Should().Be("ERROR User already logged in.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_LOGIN_command_dont_alter_users_count()
    {
        ServerContext context = ServerContext.Create(); /* Create new server context */
        CommandParser parser = new();
        UserContext aliceCtx = new();
        UserContext bobCtx = new();

        parser.ReplyFor("LOGIN alice\r\n", context, aliceCtx);
        parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);
        string output = parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);

        output.Should().Be("ERROR User already logged in.\r\n");
        context.LoggedUserCount.Should().Be(2);
    }

    [Fact]
    public void Server_responds_ERROR_to_LOGIN_command_to_already_logged_user()
    {
        ServerContext context = ServerContext.Create(); /* Create new server context */
        CommandParser parser = new();
        UserContext aliceCtx = new();
        UserContext bobCtx = new();

        parser.ReplyFor("LOGIN alice\r\n", context, aliceCtx);
        parser.ReplyFor("LOGIN bob\r\n", context, bobCtx);
        string output = parser.ReplyFor("LOGIN alice\r\n", context, bobCtx);

        output.Should().Be("ERROR User already logged in.\r\n");
        context.LoggedUserCount.Should().Be(2);
    }

    [Fact]
    public void Server_responds_OK_to_JOIN_command()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();

        string output = parser.ReplyFor("JOIN #hello\r\n");

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_JOIN_command_wo_room()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("JOIN\r\n");

        output.Should().Be("ERROR You need to specify a room to join.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_JOIN_command_with_trail_space()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("JOIN \r\n");

        output.Should().Be("ERROR You need to specify a room to join.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_JOIN_command_with_invalid_room_name()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("JOIN invalidname\r\n");

        output.Should().Be("ERROR Invalid room name.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_JOIN_command_wo_server_context()
    {
        CommandParser parser = new();
        UserContext userCtx = new();
        string output = parser.ReplyFor("JOIN #invalidname\r\n", userCtx: userCtx);

        output.Should().Be("ERROR You must login first.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_JOIN_command_with_user_context()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();

        string output = parser.ReplyFor("JOIN #invalidname\r\n", context, userContext);

        output.Should().Be("ERROR You must login first.\r\n");
    }

    [Fact]
    public void Server_responds_OK_to_JOIN_command_with_user_context()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();

        parser.ReplyFor("LOGIN alice\r\n", context, userContext);
        string output = parser.ReplyFor("JOIN #meeting\r\n", context, userContext);

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_set_correct_rooms_after_user_execute_JOIN_command()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();

        parser.ReplyFor("LOGIN alice\r\n", context, userContext);
        parser.ReplyFor("JOIN #meeting\r\n", context, userContext);
        string output = parser.ReplyFor("JOIN #meeting2\r\n", context, userContext);

        string[] joined = userContext.JoinedRooms.ToArray();

        userContext.JoinedRooms.Count.Should().Be(2);
        userContext.JoinedRooms.Should().BeEquivalentTo(new[] { "#meeting", "#meeting2" });
    }

    [Fact]
    public void Server_responds_OK_to_PART_command()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("PART #hello\r\n");

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_PART_command_wo_room()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("PART\r\n");

        output.Should().Be("ERROR You need to specify a room to part.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_PART_command_with_trail_space()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("PART \r\n");

        output.Should().Be("ERROR You need to specify a room to part.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_PART_command_with_invalid_room_name()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("PART invalidname\r\n");

        output.Should().Be("ERROR Invalid room name.\r\n");
    }

    [Fact]
    public void Server_responds_OK_to_PART_command_and_remove_from_joined_rooms()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();
        parser.ReplyFor("LOGIN alice\r\n", context, userContext);
        parser.ReplyFor("JOIN #hello\r\n", context, userContext);
        parser.ReplyFor("JOIN #hello2\r\n", context, userContext);
        parser.ReplyFor("PART #hello\r\n", context, userContext);

        string[] rooms = userContext.JoinedRooms.ToArray();

        rooms.Length.Should().Be(1);
        rooms.Should().BeEquivalentTo(new[] { "#hello2" });
    }

    [Fact]
    public void Server_responds_ERROR_to_PART_command_from_not_joined_room()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();
        parser.ReplyFor("LOGIN alice\r\n", context, userContext);
        parser.ReplyFor("JOIN #hello\r\n", context, userContext);
        parser.ReplyFor("JOIN #hello2\r\n", context, userContext);
        string output = parser.ReplyFor("PART #hello3\r\n", context, userContext);

        output.Should().Be("ERROR You haven't joined this room.\r\n");
        userContext.JoinedRooms.Should().HaveCount(2);
        userContext.JoinedRooms.Should().BeEquivalentTo(new[] { "#hello", "#hello2" });
    }


    [Fact]
    public void Server_responds_OK_to_MSG_command()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("MSG #hello msg-hello\r\n");

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_responds_OK_to_MSG_command_with_user_context()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();
        parser.ReplyFor("LOGIN alice", context, userContext);
        parser.ReplyFor("JOIN #hello", context, userContext);
        string output = parser.ReplyFor("MSG #hello msg-hello\r\n", context, userContext);

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_MSG_command_to_not_joined_room()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();
        parser.ReplyFor("LOGIN alice", context, userContext);
        parser.ReplyFor("JOIN #hello", context, userContext);
        string output = parser.ReplyFor("MSG #hello2 msg-hello\r\n", context, userContext);

        output.Should().Be("ERROR You haven't joined #hello2 room.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_MSG_command_wo_message()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("MSG #hello\r\n");

        output.Should().Be("ERROR You need to specify a room/user and a message to send.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_MSG_command_wo_room_and_message()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("MSG\r\n");

        output.Should().Be("ERROR You need to specify a room/user and a message to send.\r\n");
    }

    [Fact]
    public void Server_responds_ERROR_to_MSG_command_to_not_logged_user()
    {
        CommandParser parser = new();
        ServerContext context = ServerContext.Create();
        UserContext userContext = new();

        parser.ReplyFor("LOGIN alice\r\n", context, userContext);
        string output = parser.ReplyFor("MSG bob hello.\r\n", context, userContext);

        output.Should().Be("ERROR User bob is currently not logged in.\r\n");
    }


    [Fact]
    public void Server_responds_OK_to_LOGOUT_command()
    {
        CommandParser parser = new();
        string output = parser.ReplyFor("LOGOUT\r\n");

        output.Should().Be("OK\r\n");
    }

    [Fact]
    public void Server_decrease_users_count_on_LOGOUT_command()
    {
        ServerContext context = ServerContext.Create();
        CommandParser parser = new();

        parser.ReplyFor("LOGIN hello\r\n", context);
        parser.ReplyFor("LOGIN world\r\n", context);
        parser.ReplyFor("LOGOUT\r\n", context);

        context.LoggedUserCount.Should().Be(2);
    }
}
