using ItaSoftware.Puzzles.Chat.Commands;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserMessageCommand : Command
    {
        private readonly bool hasInvalidArgsCount;

        public UserMessageCommand(bool hasInvalidArgsCount)
        {
            this.hasInvalidArgsCount = hasInvalidArgsCount;
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
                result.Response = "ERROR You need to specify a room/user and a message to send.";
            else
                result.Response = "OK";

            return result;
        }

    }
}