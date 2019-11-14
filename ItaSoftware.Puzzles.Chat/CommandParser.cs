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

        public string Execute(string command, ServerContext context = null, UserContext userCtx = null)
        {
            _ = new string[0];

            /* Clean input command and retrieve the command name */
            command = command.Replace(CRLF, string.Empty).Trim();
            string cmd_name = command.Split(' ')[0];

            IResult result = Result.Default;

            try
            {
                /* Create command from input */
                ICommand srvCommand = CommandFactory.Create(context, userCtx, cmd_name, command);

                /* Execute command */
                result = srvCommand.Handle();
            }
            catch (CommandParseException cmdEx)
            {
                result = cmdEx.Result;
            }

            /* Return command response */
            return result.Response;
        }
    }
}
