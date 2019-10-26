namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserLogoutCommand : Command
    {
        public UserLogoutCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (HasContext && HasUserContext)
            {
                context.RemoveUser(userCtx);
            }


            result.Response = "OK";

            return result;
        }
    }
}