namespace ItaSoftware.Puzzles.Chat
{
    public interface IResult<T>
    {
        string Response { get; set; }
        T Data { get; set; }
    }

    public interface IResult : IResult<object> { }


    public sealed class Result<T> : IResult<T>
    {
        private string _resp;

        public string Response
        {
            get => _resp;
            set => _resp = value.EndsWith(CommandArgs.CRLF) ? value : $"{value}{CommandArgs.CRLF}";
        }
        public T Data { get; set; }
    }

    public sealed class Result : IResult
    {
        public static Result Default = new Result();

        private string _resp;

        public string Response
        {
            get => _resp;
            set => _resp = value.EndsWith(CommandArgs.CRLF) ? value : $"{value}{CommandArgs.CRLF}";
        }
        public object Data { get; set; }

        public Result() : this(string.Empty, null) { }

        public Result(string rsp) : this(rsp, null) { }

        public Result(string rsp, object data) =>
            (Response, Data) = (rsp, data);
    }
}
