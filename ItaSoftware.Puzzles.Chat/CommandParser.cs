using ItaSoftware.Puzzles.Chat.Commands;
using System.Collections.Generic;
using System.Linq;

namespace ItaSoftware.Puzzles.Chat
{
    public class CommandParser
    {
        public const string CRLF = "\r\n";

        public static IDictionary<string, CommandInfo> ServMsg = new Dictionary<string, CommandInfo>
        {
            { "OK", new CommandInfo(0, false) },
            { "ERROR", new CommandInfo(1, false) },
            { "GOTUSERMSG", new CommandInfo(2, true) },
            { "GOTROOMMSG", new CommandInfo(3, true) }
        };

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

        public string Execute(string command, ServerContext context = null, UserContext userCtx = null)
        {
            IResult result = new Result
            {
                Response = string.Empty,
                Data = null
            };

            bool hasInvalidArgsCount = false;
            _ = new string[0];

            command = command.Replace(CRLF, string.Empty).Trim();
            string cmd_name = command.Split(' ')[0];

            /* Check if a command is supported */
            if (!Commands.Keys.Contains(cmd_name))
            {
                result.Response = "ERROR Unsupported command";
                return result.Response;
            }

            /* Split whole command line */
            string[] cmd_args = command.Split(' ');

            /* Check for correct arguments */
            if ((cmd_args.Length - 1) < Commands[cmd_name].MinArgs)
            {
                result.Response = "ERROR Invalid arguments number.";
                hasInvalidArgsCount = true;
            }
            else
            {
                cmd_args = cmd_args.Skip(1).ToArray();
            }

            /* Create command from input */
            ICommand srvCommand = CommandFactory.Create(context, userCtx, cmd_name, cmd_args, hasInvalidArgsCount);

            /* Execute command */
            result = srvCommand.Handle();

            /* Return command response */
            return result.Response;
        }
    }
}
