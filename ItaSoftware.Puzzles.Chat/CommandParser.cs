using ItaSoftware.Puzzles.Chat.Commands;

namespace ItaSoftware.Puzzles.Chat;

public class CommandParser
{
    //public static IDictionary<string, CommandInfo> ServMsg = new Dictionary<string, CommandInfo>
    //{
    //    { "OK", new CommandInfo(0, false) },
    //    { "ERROR", new CommandInfo(1, false) },
    //    { "GOTUSERMSG", new CommandInfo(2, true) },
    //    { "GOTROOMMSG", new CommandInfo(3, true) }
    //};

    public string ReplyFor(string command, ServerContext context = null, UserContext userCtx = null)
    {
        /* Create command arguments */
        ICommandArgs args = CommandArgs.Create(command, context, userCtx);

        return Execute(args);
    }

    public string Execute(ICommandArgs args)
    {
        IResult result;

        try
        {
            /* Create command from input */
            ICommand srvCommand = CommandFactory.Create(args);

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
