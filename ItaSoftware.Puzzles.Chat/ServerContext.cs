namespace ItaSoftware.Puzzles.Chat;

public sealed class ServerContext
{

    public static ServerContext Create()
    {
        return new ServerContext();
    }

    private readonly ISet<User> loggedUsers;
    public int LoggedUserCount => loggedUsers.Count;

    private ServerContext()
    {
        loggedUsers = new SortedSet<User>();
    }

    internal bool IsUserLoggedIn(string user)
    {
        if (string.IsNullOrEmpty(user))
        {
            return false;
        }

        return loggedUsers.Contains(new User(user));
    }

    internal bool IsUserLoggedIn(UserContext ctx)
    {
        User user = FindByContext(ctx);

        if (user == null)
        {
            return false;
        }

        return IsUserLoggedIn(user.Username);
    }

    internal User AddUser(string user)
    {
        User newUser = new User(user);

        loggedUsers.Add(newUser);

        return newUser;
    }

    internal bool RemoveUser(string user)
    {
        /* No user is removed */
        if (string.IsNullOrEmpty(user))
        {
            return false;
        }

        /* Remove user object */
        if (LoggedUserCount <= 0)
        {
            return false;
        }

        loggedUsers.Remove(new User(user));

        return true;
    }

    internal void SendMessage(string dstUser, string msg)
    {
        if (string.IsNullOrEmpty(dstUser))
        {
            return;
        }

        if (string.IsNullOrEmpty(msg))
        {
            return;
        }

        UserContext dstCtx = FindContextByName(dstUser);

        if (dstCtx == null)
        {
            return;
        }

        dstCtx.Messages.Enqueue(string.Format("GOTROOMMSG {0} {1}", dstUser, msg));
    }

    private UserContext FindContextByName(string dstUser)
    {
        foreach (User u in loggedUsers)
        {
            if (u.Username.Equals(dstUser))
            {
                return u.Context;
            }
        }

        return null;
    }

    internal bool RemoveUser(UserContext ctx)
    {
        /* Find user by context */
        User user = FindByContext(ctx);

        return RemoveUser(user.Username);
    }

    private User FindByContext(UserContext ctx)
    {
        if (ctx == null)
            return null;

        if (ctx.Owner == null)
            return null;

        foreach (User user in loggedUsers)
        {
            if (user.Username.Equals(ctx.Owner.Username))
            {
                return user;
            }
        }

        return null;
    }
}
