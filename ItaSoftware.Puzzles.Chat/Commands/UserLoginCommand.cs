namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserLoginCommand : Command
    {
        public UserLoginCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
            {
                result.Response = "ERROR Need to specify a username.";
                return result;
            }

            result.Response = "OK";

            if (HasUserContext)
            {
                string username = cmd_args[0];

                if (!context.IsUserLoggedIn(userCtx))
                {
                    User user = context.AddUser(username);

                    if (HasUserContext)
                        userCtx.Owner = user;
                }
                else
                    result.Response = "ERROR User already logged in.";
            }
            else if (HasContext)
            {
                string username = cmd_args[0];


                if (!context.IsUserLoggedIn(username))
                {
                    User user = context.AddUser(username);

                    if (HasUserContext)
                        userCtx.Owner = user;
                }
                else
                    result.Response = "ERROR User already logged in.";
            }

            return result;
        }
    }
}