using System;

namespace ItaSoftware.Puzzles.Chat
{
    public class Client
    {
        CommandParser parser;
        ServerContext context;
        UserContext userContext;

        public event EventHandler<MessageArgs> MessageReceived;

        public static Client Create()
        {
            return new Client();
        }

        private Client()
        {
            userContext = new UserContext();
        }

        public void Send(string msg)
        {
            string output = parser.Execute(msg, context, userContext);

            MessageReceived?.Invoke(this, new MessageArgs
            {
                Body = output
            });
        }

        public void Connect(string host_name, ushort port)
        {
            if ("localhost".Equals(host_name) && 3000.Equals(port))
            {
                context = ServerContext.Create();
                parser = new CommandParser();
            }
        }
    }
}
