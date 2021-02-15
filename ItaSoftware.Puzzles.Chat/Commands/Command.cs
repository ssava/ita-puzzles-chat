namespace ItaSoftware.Puzzles.Chat.Commands
{
    public interface ICommand
    {
        IResult Handle();
    }

    public abstract class Command : ICommand
    {
        protected ServerContext context;
        protected UserContext userCtx;
        protected string[] cmd_args;
        protected bool hasInvalidArgsCount;

        public bool HasContext { get => context != null; }
        public bool HasUserContext { get => userCtx != null; }

        protected Command(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
        {
            this.context = context;
            this.userCtx = userCtx;
            this.cmd_args = cmd_args;
            this.hasInvalidArgsCount = hasInvalidArgsCount;
        }

        public abstract IResult Handle();

        public static IResult Ok()
        {
            return new Result
            {
                Response = "OK"
            };
        }

        public static IResult Error(string reason)
        {
            return new Result
            {
                Response = $"ERROR {reason}"
            };
        }
    }
}
