namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserLogoutCommand : Command
    {
        public UserLogoutCommand(ServerContext context, UserContext userCtx, bool hasContext, bool hasUserContext)
        {
            this.context = context;
            this.userCtx = userCtx;
            this.hasContext = hasContext;
            this.hasUserContext = hasUserContext;
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasContext && hasUserContext)
            {
                context.RemoveUser(userCtx);
            }


            result.Response = "OK";

            return result;
        }
    }
}