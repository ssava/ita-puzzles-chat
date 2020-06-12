namespace ItaSoftware.Puzzles.Chat.Commands
{
    [CommandInfo(minArgs: 1, isLoginRequired: true)]
    internal class JoinRoom : Command
    {
        public JoinRoom(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            if (hasInvalidArgsCount)
                return Error("You need to specify a room to join.");

            if (!cmd_args[0].StartsWith("#"))
                return Error("Invalid room name.");

            if (!HasContext && !HasUserContext)
                return Ok();

            else if (HasContext && !context.IsUserLoggedIn(userCtx))
                return Error("You must login first.");

            else if (!HasContext && HasUserContext)
                return Error("You must login first.");
            else
            {
                userCtx.JoinedRooms.Add(cmd_args[0]);
                return Ok();
            }
        }
    }
}