namespace ItaSoftware.Puzzles.Chat.Commands
{
    public abstract class Command : ICommand
    {
        protected ServerContext context;
        protected UserContext userCtx;
        protected string[] cmd_args;
        protected bool hasContext;
        protected bool hasInvalidArgsCount;
        protected bool hasUserContext;

        protected Command(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
        {
            this.hasContext = context != null;
            this.hasUserContext = userCtx != null;
            this.cmd_args = cmd_args;
            this.hasInvalidArgsCount = hasInvalidArgsCount;
        }

        public abstract IResult Handle();
    }
}
