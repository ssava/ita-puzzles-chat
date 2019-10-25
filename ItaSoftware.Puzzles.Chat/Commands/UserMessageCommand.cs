namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserMessageCommand : Command
    {
        public UserMessageCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
                result.Response = "ERROR You need to specify a room/user and a message to send.";
            else
            {
                string dest = cmd_args[0];

                /* handle resp w/o context */
                if (context == null)
                {
                    result.Response = "OK";
                    return result;
                }

                bool isDstRoom = !string.IsNullOrEmpty(dest) && dest.StartsWith("#");

                if (!isDstRoom)
                {
                    if (context.IsUserLoggedIn(dest))
                        result.Response = "OK";
                    else
                        result.Response = "ERROR User " + dest + " is currently not logged in.";
                }
                else {
                    if (!hasUserContext)
                        result.Response = "OK";
                    else
                    {
                        if (userCtx.JoinedRooms.Contains(dest))
                            result.Response = "OK";
                        else
                            result.Response = "ERROR You haven't joined " + dest + " room.";
                    }
                }
            }


            return result;
        }

    }
}