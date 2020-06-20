namespace ItaSoftware.Puzzles.Chat.Commands
{
    [CommandInfo(minArgs: 1, isLoginRequired: true)]
    internal class LeaveRoom : Command
    {
        public LeaveRoom(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            string room_name = string.Empty;

            if (hasInvalidArgsCount)
            {
                return Error("You need to specify a room to part.");
            }

            room_name = cmd_args[0];

            if (!cmd_args[0].StartsWith("#"))
            {
                return Error("Invalid room name.");
            }

            if (!HasContext || !HasUserContext)
            {
                return Ok();
            }

            if (!userCtx.JoinedRooms.Contains(room_name))
            {
                return Error("You haven't joined this room.");
            }

            userCtx.JoinedRooms.Remove(room_name);

            return Ok();
        }
    }
}