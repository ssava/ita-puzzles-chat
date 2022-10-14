namespace ItaSoftware.Puzzles.Chat;

public sealed class UserContext : IComparable
{
    public User Owner { get; set; }
    public ISet<string> JoinedRooms { get; private set; }

    public Queue<string> Messages { get; private set; }

    public UserContext()
    {
        JoinedRooms = new SortedSet<string>();
        Messages = new Queue<string>();
        Owner = User.None;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return -1;

        UserContext ctxObj = (UserContext)obj;

        return Owner.CompareTo(ctxObj.Owner);
    }
}
