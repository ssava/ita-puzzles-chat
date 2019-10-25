﻿namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class UserJoinCommand : Command
    {
        public UserJoinCommand(ServerContext context, UserContext userCtx, string[] cmd_args, bool hasInvalidArgsCount)
            : base(context, userCtx, cmd_args, hasInvalidArgsCount)
        {
        }

        public override IResult Handle()
        {
            IResult result = new Result();

            if (hasInvalidArgsCount)
            {
                result.Response = "ERROR You need to specify a room to join.";
                return result;
            }

            if (!cmd_args[0].StartsWith("#"))
            {
                result.Response = "ERROR Invalid room name.";
                return result;
            }

            if (!hasContext && !hasUserContext)
                result.Response = "OK";
            else if (hasContext && !context.IsUserLoggedIn(userCtx))
                result.Response = "ERROR You must login first.";
            else if (!hasContext && hasUserContext)
                result.Response = "ERROR You must login first.";
            else
            {
                userCtx.JoinedRooms.Add(cmd_args[0]);
                result.Response = "OK";
            }
                

            return result;
        }
    }
}