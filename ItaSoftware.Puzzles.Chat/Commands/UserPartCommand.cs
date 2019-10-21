using ItaSoftware.Puzzles.Chat.Commands;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserPartCommand : Command
    {
        private readonly string[] cmd_args;
        private readonly bool hasInvalidArgsCount;

        public UserPartCommand(string[] cmd_args, bool hasInvalidArgsCount)
        {
            this.cmd_args = cmd_args;
            this.hasInvalidArgsCount = hasInvalidArgsCount;
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
            {
                result.Response = "ERROR You need to specify a room to part.";
                return result;
            }

            if (!cmd_args[0].StartsWith("#"))
            {
                result.Response = "ERROR Invalid room name.";
                return result;
            }

            result.Response = "OK";

            return result;
        }
    }
}