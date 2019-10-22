namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserMessageCommand : Command
    {
        public UserMessageCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasContext, bool hasInvalidArgsCount, bool hasUserContext)
        {
            this.context = context;
            this.userCtx = userCtx;
            this.cmd_args = cmd_args;
            this.hasContext = hasContext;
            this.hasInvalidArgsCount = hasInvalidArgsCount;
            this.hasUserContext = hasUserContext;
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
                result.Response = "ERROR You need to specify a room/user and a message to send.";
            else
            {
                string dest = cmd_args[0];

                if (context == null)
                    result.Response = "OK";
                else if(context.IsUserLoggedIn(dest))
                    result.Response = "OK";
                else
                    result.Response = "ERROR User " + dest + " is currently not logged in.";
            }
                

            return result;
        }

    }
}