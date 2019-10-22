namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserPartCommand : Command
    {
        public UserPartCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasContext, bool hasInvalidArgsCount, bool hasUserContext)
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

            if (hasContext && hasUserContext)
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