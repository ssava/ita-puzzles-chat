using System;
using System.Linq;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    internal class CommandFactory
    {
        // MAP { "GOTROOMMSG", new CommandInfo(3, true) },
        // MAP { "GOTUSERMSG", new CommandInfo(2, true) },

        private static readonly CommandMapper Commands;
        
        static CommandFactory()
        {
            Commands = new CommandMapper();

            Commands.Bind<UserLogin>("LOGIN");
            Commands.Bind<JoinRoom>("JOIN");
            Commands.Bind<LeaveRoom>("PART");
            Commands.Bind<SendMessage>("MSG");
            Commands.Bind<UserLogout>("LOGOUT");
        }

        public static ICommand Create(ICommandArgs args)
        {
            bool hasInvalidArgsCount = false;
            string cmdName = string.Empty;

            /* Retrieve command name */
            if (args != null)
                cmdName = args.FullCommand.Split(' ')[0];

            /* Check if a command is supported */
            if (!Commands.IsBound(cmdName))
                throw new CommandParseException(new Result("ERROR Unsupported command"));

            /* Split whole command line */
            string[] cmd_args = args.FullCommand.Split(' ');

            /* Check for correct arguments */
            if (!Commands.AreArgumentsValid(cmdName, cmd_args))
                hasInvalidArgsCount = true;
            else
                cmd_args = cmd_args.Skip(1).ToArray();

            return Commands.Build(cmdName, args.Context, args.UserContext, cmd_args, hasInvalidArgsCount);
        }
    }
}
