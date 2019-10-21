using ItaSoftware.Puzzles.Chat.Commands;

namespace ItaSoftware.Puzzles.Chat
{
    internal class UserLogoutCommand : Command
    {
        private readonly ServerContext context;
        private readonly UserContext userCtx;
        private readonly bool hasContext;
        private readonly bool hasUserContext;

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