using System;

namespace ItaSoftware.Puzzles.Chat
{
    public class CommandInfoAttribute : Attribute
    {
        public ushort MinArgs { get; private set; }
        public bool IsLoginRequired { get; private set; }

        public CommandInfoAttribute(ushort minArgs) : this(minArgs, false) { }
        public CommandInfoAttribute(bool isLoginRequired) : this(0, isLoginRequired) { }

        public CommandInfoAttribute(ushort minArgs, bool isLoginRequired) =>
            (MinArgs, IsLoginRequired) = (minArgs, isLoginRequired);
    }
}
