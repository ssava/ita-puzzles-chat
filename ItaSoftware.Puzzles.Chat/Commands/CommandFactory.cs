namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class CommandFactory
    {
        public static ICommand Create(ServerContext context, UserContext userCtx, string cmd_name, string[] cmd_args, bool hasInvalidArgsCount)
        {
            switch (cmd_name)
            {
                case "LOGIN":
                    return new UserLoginCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "JOIN":
                    return new UserJoinCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "PART":
                    return new UserPartCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "MSG":
                    return new UserMessageCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "LOGOUT":
                    return new UserLogoutCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
            }

            return null;
        }
    }
}
