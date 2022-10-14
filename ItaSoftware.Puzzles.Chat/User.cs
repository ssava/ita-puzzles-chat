namespace ItaSoftware.Puzzles.Chat;

public sealed class User : IComparable
{
    public static readonly User None
        = new(string.Empty);

    public string Username { get; private set; }
    public UserContext Context { get; private set; }

    public User(string user)
    {
        Username = user;
        Context = new UserContext
        {
            Owner = this
        };
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return -1;

        User userObj = (User)obj;

        return Username.CompareTo(userObj.Username);
    }
}
