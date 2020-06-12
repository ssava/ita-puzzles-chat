using System;
using System.Collections.Generic;

namespace ItaSoftware.Puzzles.Chat.Commands
{
    class CommandMapper
    {
        private readonly Dictionary<string, Type> _map;

        public CommandMapper()
        {
            _map = new Dictionary<string, Type>();
        }

        public void Bind<TCommand>(string cmdName) where TCommand : ICommand
        {
            if (_map.ContainsKey(cmdName))
                _map[cmdName] = typeof(TCommand);
            else
                _map.Add(cmdName, typeof(TCommand));
        }

        /// <summary>
        /// Returns a Boolean value indicating whether a command name is bound 
        /// to a <see cref="Command"/> class.
        /// </summary>
        /// <param name="cmdName">Command name</param>
        /// <returns>True if the command is binded to a command class, False otherwise.</returns>
        public bool IsBound(string cmdName)
        {
            return _map.ContainsKey(cmdName);
        }

        /// <summary>
        /// Returns a Boolean value indicating whether provided arguments are valid for given <see cref="Command"/>.
        /// Command arguments requirement are specified for each command via <see cref="CommandInfoAttribute"/> decoration.
        /// </summary>
        /// <param name="cmdName">Command name</param>
        /// <param name="cmd_args">Argument to validate</param>
        /// <remarks></remarks>
        /// <returns>True if argument are missing, command does not exists or arguments are valid, false otherwise</returns>
        internal bool AreArgumentsValid(string cmdName, string[] cmd_args)
        {
            if (!IsBound(cmdName))
                return true;

            Type cmdType = _map[cmdName];

            // Get instance of the attribute.
            CommandInfoAttribute commandInfo = 
                (CommandInfoAttribute) Attribute.GetCustomAttribute(cmdType, typeof(CommandInfoAttribute));

            if (commandInfo == null)
                return true;

            return (cmd_args.Length - 1) < commandInfo.MinArgs;
        }

        internal Type GetBoundType(string cmdName)
        {
            return IsBound(cmdName) ? _map[cmdName] : null;
        }

        internal ICommand Build(string cmdName, ServerContext context, UserContext userContext, string[] cmd_args, bool hasInvalidArgsCount)
        {
            Type cmdType = GetBoundType(cmdName);

            /* No type is bound for given command name */
            if (cmdName == null)
                return null;

            var command = (cmdType) Activator.CreateInstance(cmdType);

            return command;
        }
    }
}
