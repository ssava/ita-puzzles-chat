﻿using ItaSoftware.Puzzles.Chat.Commands;
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

        //public string Execute<T>(string command, ServerContext context = null) where T : CommandResult

        public string Execute(string command, ServerContext context = null, UserContext userCtx = null)
        {
            IResult result = new Result
            {
                Response = string.Empty,
                Data = null
            };

            _ = new string[0];
            bool hasContext = context != null;

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
                result.Response = "ERROR Invalid arguments number.";
            else
                cmd_args = cmd_args.Skip(1).ToArray();

            /* Handle each commands */
            bool hasInvalidArgsCount = result.Response.StartsWith("ERROR");
            bool hasUserContext = userCtx != null;

            /* Create command from input */
            ICommand srvCommand = Command.Create(context, userCtx, cmd_name, cmd_args, hasContext, hasInvalidArgsCount, hasUserContext);

            /* Execute command */
            result = srvCommand.Handle();

            /* Return command response */
            return result.Response;
        }
    }
}
