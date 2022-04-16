namespace ItaSoftware.Puzzles.Chat;

public interface IResult<T> where T : CommandResult
{
    string Response { get; set; }
    T Data { get; set; }
}

public interface IResult
{
    string Response { get; set; }
    object Data { get; set; }
}


public sealed class Result<T> : IResult<T> where T : CommandResult
{
    private string _resp;

    public string Response
    {
        get => _resp;
        set
        {
            if (value.EndsWith(CommandArgs.CRLF))
            {
                _resp = value;
                return;
            }

            _resp = string.Format("{0}{1}", value, CommandArgs.CRLF);
        }
    }
    public T Data { get; set; }

    public Result() : this(string.Empty) { }

    public Result(string rsp) : this(rsp, null) { }

    public Result(string rsp, T data)
    {
        Response = rsp;
        Data = data;
    }
}

public sealed class Result : IResult
{
    public static Result Default = new Result();

    private string _resp;

    public string Response
    {
        get => _resp;
        set
        {
            if (value.EndsWith(CommandArgs.CRLF))
            {
                _resp = value;
                return;
            }

            _resp = string.Format("{0}{1}", value, CommandArgs.CRLF);
        }
    }
    public object Data { get; set; }

    public Result() : this(string.Empty) { }

    public Result(string rsp) : this(rsp, null) { }

    public Result(string rsp, object data)
    {
        Response = rsp;
        Data = data;
    }
}

public abstract class CommandResult
{

}
