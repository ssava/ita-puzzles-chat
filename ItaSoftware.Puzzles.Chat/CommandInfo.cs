namespace ItaSoftware.Puzzles.Chat
{
    public struct CommandInfo
    {
        public ushort MinArgs { get; set; }
        public bool IsLoginRequired { get; set; }

        public CommandInfo(ushort minArgs) : this()
        {
            MinArgs = minArgs;
            IsLoginRequired = false;
        }

        public CommandInfo(ushort minArgs, bool isLoginRequired) : this(minArgs)
        {
            IsLoginRequired = isLoginRequired;
        }

    }
}
