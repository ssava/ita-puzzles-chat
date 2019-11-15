namespace ItaSoftware.Puzzles.Chat
{
    internal interface ICommandArgs
    {
        string CommandName { get; set; }
        ServerContext Context { get; set; }
        string FullCommand { get; set; }
        UserContext UserContext { get; set; }
    }

    internal class CommandArgs : ICommandArgs
    {
        public ServerContext Context { get; set; }
        public UserContext UserContext { get; set; }
        public string CommandName { get; set; }
        public string FullCommand { get; set; }
    }
}