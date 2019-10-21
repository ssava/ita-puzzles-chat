namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserLoginCommand : Command
    {
        private readonly ServerContext context;
        private readonly UserContext userCtx;
        private readonly string[] cmd_args;
        private readonly bool hasContext;
        private readonly bool hasInvalidArgsCount;
        private readonly bool hasUserContext;

        public UserLoginCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasContext, bool hasInvalidArgsCount, bool hasUserContext)
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
                result.Response = "ERROR Need to specify a username.";
            else
            {
                result.Response = "OK";

                if (hasUserContext)
                {
                    string username = cmd_args[0];


                    if (!context.IsUserLoggedIn(userCtx))
                    {
                        User user = context.AddUser(username);

                        if (hasUserContext)
                            userCtx.Owner = user;
                    }
                    else
                        result.Response = "ERROR User already logged in.";
                }
                else if (hasContext)
                {
                    string username = cmd_args[0];


                    if (!context.IsUserLoggedIn(username))
                    {
                        User user = context.AddUser(username);

                        if (hasUserContext)
                            userCtx.Owner = user;
                    }
                    else
                        result.Response = "ERROR User already logged in.";
                }
            }

            return result;
        }
    }
}