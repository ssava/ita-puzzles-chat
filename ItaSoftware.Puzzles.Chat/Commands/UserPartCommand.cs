namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserPartCommand : Command
    {
        public UserPartCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            IResult result = new Result();
            string room_name = string.Empty;

            if (hasInvalidArgsCount)
            {
                result.Response = "ERROR You need to specify a room to part.";
                return result;
            }

            if (!cmd_args[0].StartsWith("#"))
            {
                result.Response = "ERROR Invalid room name.";
                return result;
            }

            room_name = cmd_args[0];

            if (HasContext && HasUserContext)
            {
                if(!userCtx.JoinedRooms.Contains(room_name))
                {
                    result.Response = "ERROR You haven't joined this room.";
                } else
                {
                    
                    userCtx.JoinedRooms.Remove(room_name);
                    result.Response = "OK";
                }
            } else
            {
                result.Response = "OK";
            }

            return result;
        }
    }
}