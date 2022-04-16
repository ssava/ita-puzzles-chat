using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ItaSoftware.Puzzles.Chat;

sealed class ServerState
{
    public bool IsRunning { get; set; }
    public Socket Listener { get; set; }
    public IDictionary<ClientState, UserContext> UserStates { get; set; }
}

sealed class ClientState
{
    public Socket clientSocket = null;
    public const int BufferSize = 4096;
    public byte[] receiveBuffer = new byte[BufferSize];
    public StringBuilder fromClient = new StringBuilder();
}

public interface IChatServer
{
    ushort ListeningPort { get; }

    bool IsRunning();
    void Start();
    void Stop();
}

public sealed class ChatServer : IChatServer
{
    const ushort DefaultPort = 30000;

    public ushort ListeningPort { get; private set; }
    public CommandParser Parser { get; private set; }
    public ServerContext Context { get; private set; }

    private readonly ServerState state;

    public ChatServer() : this(DefaultPort) { }

    public ChatServer(ushort port)
    {
        ListeningPort = port;
        state = new ServerState();
        Reset();
    }

    private void Reset()
    {
        Parser = new CommandParser();
        Context = ServerContext.Create();
        state.Listener?.Close();
        state.UserStates = new Dictionary<ClientState, UserContext>();
        state.Listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
        state.IsRunning = false;
    }

    public bool IsRunning() => state.IsRunning;

    public void Start()
    {
        try
        {
            IPEndPoint local = new IPEndPoint(IPAddress.Any, ListeningPort);
            state.Listener.Bind(local);
            state.Listener.Listen(100);
            state.IsRunning = true;

            while (IsRunning())
            {
                state.Listener.BeginAccept(new AsyncCallback(AcceptCallback), state.Listener);
            }
        }
        catch (Exception)
        {

        }
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);
        ClientState clientState = new ClientState
        {
            clientSocket = handler
        };

        state.UserStates.Add(clientState, new UserContext());
        handler.BeginReceive(clientState.receiveBuffer, 0, ClientState.BufferSize, 0,
            new AsyncCallback(ReadCallback), clientState);
    }

    private void ReadCallback(IAsyncResult ar)
    {
        String clientCommand = string.Empty;
        ClientState clientState = (ClientState)ar.AsyncState;
        Socket client = clientState.clientSocket;

        int bytesRead = client.EndReceive(ar);

        if (bytesRead <= 0)
        {
            return;
        }

        clientState.fromClient.Append(Encoding.ASCII.GetString(clientState.receiveBuffer, 0, bytesRead));
        clientCommand = clientState.fromClient.ToString();

        if (clientCommand.IndexOf(CommandArgs.CRLF) > -1)
        {
            SendTo(client, Parser.ReplyFor(clientCommand));
            clientState.fromClient.Clear();
        }
        else
        {
            client.BeginReceive(clientState.receiveBuffer, 0, ClientState.BufferSize, 0,
                new AsyncCallback(ReadCallback), clientState);
            clientState.fromClient.Clear();
        }
    }

    private void SendTo(Socket destination, string message)
    {
        byte[] dataToSend = Encoding.ASCII.GetBytes(message);

        destination.BeginSend(dataToSend, 0, dataToSend.Length, 0,
            new AsyncCallback(SendCallback), destination);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket handler = (Socket)ar.AsyncState;

            int bytesSent = handler.EndSend(ar);

            ClientState client
                = state.UserStates
                       .Keys
                       .Where(x => x.clientSocket == handler)
                       .First();

            handler.BeginReceive(client.receiveBuffer, 0, ClientState.BufferSize, 0,
                new AsyncCallback(ReadCallback), client);
        }
        catch (Exception)
        {

        }
    }

    public void Stop()
    {
        state.Listener.Close();
        Reset();
    }
}
