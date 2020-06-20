namespace ItaSoftware.Puzzles.Chat.Commands
{
    [CommandInfo(minArgs: 2, isLoginRequired: true)]
    internal class SendMessage : Command
    {
        public SendMessage(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            if (hasInvalidArgsCount)
            {
                return Error("You need to specify a room/user and a message to send.");
            }

            string dest = cmd_args[0];

            /* handle resp w/o context */
            if (!HasContext)
            {
                return Ok();
            }

            bool isDstRoom = !string.IsNullOrEmpty(dest) && dest.StartsWith("#");

            /* Send message to user */
            if (!isDstRoom)
            {
                if (!context.IsUserLoggedIn(dest))
                    return Error($"User {dest} is currently not logged in.");

                context.SendMessage(cmd_args[0], cmd_args[1]);
                return Ok();
            }

            /* Else send message to room */
            if (!HasUserContext)
            {
                return Ok();
            }

            if (userCtx.JoinedRooms.Contains(dest))
            {
                return Ok();
            }

            return Error($"You haven't joined {dest} room.");
        }

    }
}