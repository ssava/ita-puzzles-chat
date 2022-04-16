namespace ItaSoftware.Puzzles.Chat;

public interface ICommandArgs
{
    ServerContext Context { get; set; }
    string FullCommand { get; set; }
    UserContext UserContext { get; set; }
}

public class CommandArgs : ICommandArgs
{
    public const string CRLF = "\r\n";

    public static ICommandArgs Create(string command, ServerContext context = null, UserContext userCtx = null)
    {
        /* Clean input command and retrieve the command name */
        command = command.Replace(CRLF, string.Empty).Trim();

        return new CommandArgs
        {
            FullCommand = command,
            Context = context,
            UserContext = userCtx
        };
    }

    public ServerContext Context { get; set; }
    public UserContext UserContext { get; set; }
    public string FullCommand { get; set; }
}
