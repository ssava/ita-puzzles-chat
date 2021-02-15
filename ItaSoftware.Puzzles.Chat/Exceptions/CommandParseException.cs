using System;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    [Serializable]
    internal class CommandParseException : Exception
    {
        public IResult Result { get; private set; }
        public bool HasInvalidArguments { get; private set; }

        public CommandParseException() : this(null) { }

        public CommandParseException(IResult result) : this(result, false) { }

        public CommandParseException(IResult result, bool hasInvalidArgs) =>
            (Result, HasInvalidArguments) = (result, hasInvalidArgs);
    }
}