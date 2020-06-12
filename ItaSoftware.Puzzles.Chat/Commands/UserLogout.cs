namespace ItaSoftware.Puzzles.Chat.Commands
{
    [CommandInfo(isLoginRequired: true)]
    internal class UserLogout : Command
    {
        public UserLogout(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            if (HasContext && HasUserContext)
            {
                context.RemoveUser(userCtx);
            }

            return Ok();
        }
    }
}