using System;
using System.Collections.Generic;
using System.Linq;
using ItaSoftware.Puzzles.Chat.Tests;

namespace ItaSoftware.Puzzles.Chat
{
    public class CommandParser
    {
        public const string CRLF = "\r\n";

        public static IDictionary<string, ushort> ServMsg = new Dictionary<string, ushort>
        {
            { "OK", 0 },
            { "ERROR", 1 },
            { "GOTUSERMSG", 2 },
            { "GOTROOMMSG", 3 }
        };

        public static IDictionary<string, ushort> Commands = new Dictionary<string, ushort>
        {
            { "LOGIN", 1 },
            { "JOIN", 1 },
            { "PART", 1 },
            { "MSG", 2 },
            { "GOTROOMMSG", 3 },
            { "GOTUSERMSG", 2 },
            { "LOGOUT", 0 }
        };

        public string Execute(string command, ServerContext context = null)
        {
            string result = string.Empty;
            string cmd_name = string.Empty;
            string[] cmd_args = new string[0];
            bool hasContext = context != null;

            command = command.Replace(CRLF, string.Empty).Trim();
            cmd_name = command.Split(' ')[0];

            /* Check if a command is supported */
            if (!Commands.Keys.Contains(cmd_name))
            {
                result = "ERROR Unsupported command";
                return string.Format("{0}{1}", result, CRLF);
            }


            /* Split whole command line */
            cmd_args = command.Split(' ');

            /* Check for correct arguments */
            if ((cmd_args.Length - 1) < Commands[cmd_name])
                result = "ERROR Invalid arguments number.";
            else
                cmd_args = cmd_args.Skip(1).ToArray();

            /* Handle each commands */
            bool hasInvalidArgsCount = result.StartsWith("ERROR");

            switch (cmd_name)
            {
                case "LOGIN":
                    if (hasInvalidArgsCount)
                        result = "ERROR Need to specify a username.";
                    else
                    {
                        result = "OK";

                        if (hasContext)
                        {
                            string username = cmd_args[0];

                            if (!context.IsUserLoggedIn(username))
                                context.AddUser(username);
                            else
                                result = "ERROR User already logged in.";
                        }
                    }
                    break;

                case "JOIN":
                    if (hasInvalidArgsCount)
                        result = "ERROR You need to specify a room to join.";
                    else
                    {
                        if (!cmd_args[0].StartsWith("#"))
                            result = "ERROR Invalid room name.";
                        else
                            result = "OK";
                    }
                    break;
                case "PART":
                    if (hasInvalidArgsCount)
                        result = "ERROR You need to specify a room to part.";
                    else
                    {
                        if (!cmd_args[0].StartsWith("#"))
                            result = "ERROR Invalid room name.";
                        else
                            result = "OK";
                    }
                    break;
                case "MSG":
                    if (hasInvalidArgsCount)
                        result = "ERROR You need to specify a room/user and a message to send.";
                    else
                        result = "OK";
                    break;
                case "LOGOUT":
                    if(hasContext)
                    {
                        context.RemoveUser();
                    }


                    result = "OK";
                    break;
            }

            return string.Format("{0}{1}", result, CRLF);
        }
    }
}
