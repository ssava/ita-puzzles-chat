namespace ItaSoftware.Puzzles.Chat.Commands;

[Serializable]
internal class CommandParseException : Exception
{
    public Result Result { get; private set; }
    public bool HasInvalidArguments { get; private set; }

    public CommandParseException()
    {
    }

    public CommandParseException(Result result)
    {
        this.Result = result;
        this.HasInvalidArguments = false;
    }

    public CommandParseException(Result result, bool hasInvalidArgs)
    {
        this.Result = result;
        this.HasInvalidArguments = hasInvalidArgs;
    }
}
