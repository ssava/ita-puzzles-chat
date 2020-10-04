using ItaSoftware.Puzzles.Chat.Domain;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    [CommandInfo(minArgs: 1)]
    internal class UserLogin : Command
    {
        public UserLogin(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            if (hasInvalidArgsCount)
            {
                return Error("Need to specify a username.");
            }

            string username = cmd_args[0];

            if (HasUserContext)
            {
                if (serverContext.IsUserLoggedIn(userCtx))
                {
                    return Error("User already logged in.");
                }

                IUser user = serverContext.AddUser(username);

                if (HasUserContext)
                {
                    userCtx.Owner = user;
                }
            }
            else if (HasContext)
            {
                if (serverContext.IsUserLoggedIn(username))
                {
                    return Error("User already logged in.");
                }

                IUser user = serverContext.AddUser(username);

                if (HasUserContext)
                {
                    userCtx.Owner = user;
                }
            }

            return Ok();
        }
    }
}