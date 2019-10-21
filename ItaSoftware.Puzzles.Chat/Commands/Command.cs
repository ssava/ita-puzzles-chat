namespace ItaSoftware.Puzzles.Chat.Commands
{
    public abstract class Command : ICommand
    {
        public static ICommand Create(ServerContext context, UserContext userCtx, string cmd_name, string[] cmd_args, bool hasContext, bool hasInvalidArgsCount, bool hasUserContext)
        {
            ICommand srvCommand = null;

            switch (cmd_name)
            {
                case "LOGIN":
                    srvCommand = new UserLoginCommand(context, userCtx, cmd_args, hasContext, hasInvalidArgsCount, hasUserContext);
                    break;

                case "JOIN":
                    srvCommand = new UserJoinCommand(context, userCtx, cmd_args, hasContext, hasInvalidArgsCount, hasUserContext);
                    break;
                case "PART":
                    srvCommand = new UserPartCommand(cmd_args, hasInvalidArgsCount);
                    break;
                case "MSG":
                    srvCommand = new UserMessageCommand(hasInvalidArgsCount);
                    break;
                case "LOGOUT":
                    srvCommand = new UserLogoutCommand(context, userCtx, hasContext, hasUserContext);
                    break;
            }

            return srvCommand;
        }

        public abstract IResult Handle();
    }
}
