namespace ItaSoftware.Puzzles.Chat
{
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
            get
            {
                return _resp;
            }
            set
            {
                if (value.EndsWith(CommandParser.CRLF))
                    _resp = value;
                else
                    _resp = string.Format("{0}{1}", value, CommandParser.CRLF);
            }
        }
        public T Data { get; set; }
    }

    public sealed class Result : IResult
    {
        private string _resp;

        public string Response
        {
            get
            {
                return _resp;
            }
            set
            {
                if (value.EndsWith(CommandParser.CRLF))
                    _resp = value;
                else
                    _resp = string.Format("{0}{1}", value, CommandParser.CRLF);
            }
        }
        public object Data { get; set; }
    }

    public abstract class CommandResult
    {

    }
}
