using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ItaSoftware.Puzzles.Chat.Tests;

public class ChatServerTests
{
    [Fact]
    public void Create_a_new_server_instance()
    {
        IChatServer server = new ChatServer();

        server.Should().NotBeNull();
    }

    [Fact]
    public void New_instance_is_listening_to_default_port()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
    }

    [Fact]
    public void New_instance_is_not_running()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
        server.IsRunning().Should().BeFalse();
    }

    [Fact(Skip = "Handle async invoking")]
    public void Server_is_running_when_started()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();
        new Task(() => server.Start());

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
        server.IsRunning().Should().BeTrue();
    }

    [Fact(Skip = "Handle async invoking")]
    public void Server_is_not_running_when_stopped()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();
        server.Start();
        server.Stop();

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
        server.IsRunning().Should().BeFalse();
    }

    [Fact(Skip = "Handle async invoking")]
    public void Server_has_a_command_parser()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();
        server.Start();

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
        server.IsRunning().Should().BeTrue();
        (server as ChatServer)?.Parser.Should().NotBeNull();
    }

    [Fact(Skip = "Handle async invoking")]
    public void Server_has_a_context()
    {
        ushort listeningPort = 30000;
        IChatServer server = new ChatServer();
        server.Start();

        server.Should().NotBeNull();
        server.ListeningPort.Should().Be(listeningPort);
        server.IsRunning().Should().BeTrue();
        (server as ChatServer)?.Parser.Should().NotBeNull();
        (server as ChatServer)?.Context.Should().NotBeNull();
    }
}
