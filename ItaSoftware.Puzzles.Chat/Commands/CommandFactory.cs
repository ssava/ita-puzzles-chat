using System.Collections.Generic;
using System.Linq;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class CommandFactory
    {
        public static IDictionary<string, CommandInfo> Commands = new Dictionary<string, CommandInfo>
        {
            { "LOGIN", new CommandInfo(1, false) },
            { "JOIN", new CommandInfo (1, true) },
            { "PART", new CommandInfo(1, true) },
            { "MSG", new CommandInfo(2, true) },
            { "GOTROOMMSG", new CommandInfo(3, true) },
            { "GOTUSERMSG", new CommandInfo(2, true) },
            { "LOGOUT", new CommandInfo(0, true) }
        };

        public static ICommand Create(ServerContext context, UserContext userCtx, string cmd_name, string command)
        {
            bool hasInvalidArgsCount = false;

            /* Check if a command is supported */
            if (!Commands.Keys.Contains(cmd_name))
                throw new CommandParseException(new Result("ERROR Unsupported command"));


            /* Split whole command line */
            string[] cmd_args = command.Split(' ');

            /* Check for correct arguments */
            if ((cmd_args.Length - 1) < Commands[cmd_name].MinArgs)
                hasInvalidArgsCount = true;
            else
                cmd_args = cmd_args.Skip(1).ToArray();

            switch (cmd_name)
            {
                case "LOGIN":
                    return new UserLoginCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "JOIN":
                    return new UserJoinCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "PART":
                    return new UserPartCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "MSG":
                    return new UserMessageCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
                case "LOGOUT":
                    return new UserLogoutCommand(context, userCtx, cmd_args, hasInvalidArgsCount);
            }

            return null;
        }
    }
}
