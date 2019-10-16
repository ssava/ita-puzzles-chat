using System;
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

            string cmd_name = string.Empty;
            string[] cmd_args = new string[0];
            bool hasContext = context != null;

            command = command.Replace(CRLF, string.Empty).Trim();
            cmd_name = command.Split(' ')[0];

            /* Check if a command is supported */
            if (!Commands.Keys.Contains(cmd_name))
            {
                result.Response = "ERROR Unsupported command";
                return result.Response;
            }


            /* Split whole command line */
            cmd_args = command.Split(' ');

            /* Check for correct arguments */
            if ((cmd_args.Length - 1) < Commands[cmd_name].MinArgs)
                result.Response = "ERROR Invalid arguments number.";
            else
                cmd_args = cmd_args.Skip(1).ToArray();

            /* Handle each commands */
            bool hasInvalidArgsCount = result.Response.StartsWith("ERROR");
            bool hasUserContext = userCtx != null;

            switch (cmd_name)
            {
                case "LOGIN":
                    if (hasInvalidArgsCount)
                        result.Response = "ERROR Need to specify a username.";
                    else
                    {
                        result.Response = "OK";

                        if (hasContext)
                        {
                            string username = cmd_args[0];

                            if (!context.IsUserLoggedIn(username))
                            {
                                User user = context.AddUser(username);

                                if (hasUserContext)
                                    userCtx.Owner = user;
                            }
                            else
                                result.Response = "ERROR User already logged in.";
                        }
                    }
                    break;

                case "JOIN":
                    if (hasInvalidArgsCount)
                        result.Response = "ERROR You need to specify a room to join.";
                    else
                    {
                        if (!cmd_args[0].StartsWith("#"))
                            result.Response = "ERROR Invalid room name.";
                        else
                        {
                            if (!hasContext && !hasUserContext)
                                result.Response = "OK";
                            else if (hasContext && !context.IsUserLoggedIn(userCtx))
                                result.Response = "ERROR You must login first.";
                            else if (!hasContext && hasUserContext)
                                result.Response = "ERROR You must login first.";
                            else
                                result.Response = "OK";
                        }
                    }
                    break;
                case "PART":
                    if (hasInvalidArgsCount)
                        result.Response = "ERROR You need to specify a room to part.";
                    else
                    {
                        if (!cmd_args[0].StartsWith("#"))
                            result.Response = "ERROR Invalid room name.";
                        else
                            result.Response = "OK";
                    }
                    break;
                case "MSG":
                    if (hasInvalidArgsCount)
                        result.Response = "ERROR You need to specify a room/user and a message to send.";
                    else
                        result.Response = "OK";
                    break;
                case "LOGOUT":
                    if (hasContext && hasUserContext)
                    {
                        context.RemoveUser(userCtx);
                    }


                    result.Response = "OK";
                    break;
            }

            return result.Response;
        }
    }
}
