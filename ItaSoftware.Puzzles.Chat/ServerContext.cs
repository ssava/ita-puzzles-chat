using System.Collections.Generic;
using System.Linq;

namespace ItaSoftware.Puzzles.Chat
{
    public sealed class ServerContext
    {

        public static ServerContext Create() =>
            new ServerContext();

        private readonly ISet<User> loggedUsers;
        public int LoggedUserCount => loggedUsers.Count;

        private ServerContext() =>
            loggedUsers = new SortedSet<User>();

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

            return user != null && IsUserLoggedIn(user.Username);
        }

        internal User AddUser(string user)
        {
            User newUser = new User(user);
            
            loggedUsers.Add(newUser);

            return newUser;
        }

        internal User AddUser(User user)
        {
            loggedUsers.Add(user);

            return user;
        }

        internal bool RemoveUser(string user)
        {
            /* No user is removed */
            if (string.IsNullOrEmpty(user) || LoggedUserCount <= 0)
            {
                return false;
            }

            loggedUsers.Remove(new User(user));

            return true;
        }

        internal void SendMessage(string dstUser, string msg)
        {
            if (string.IsNullOrEmpty(dstUser) || string.IsNullOrEmpty(msg))
            {
                return;
            }

            FindContextByName(dstUser)?.Messages.Enqueue($"GOTROOMMSG {dstUser} {msg}");
        }

        private UserContext FindContextByName(string dstUser) =>
            loggedUsers.Where(u => u.Username.Equals(dstUser))
                       .Select(c => c.Context)
                       .FirstOrDefault();

        internal bool RemoveUser(UserContext ctx) =>
            RemoveUser(FindByContext(ctx)?.Username);

        private User FindByContext(UserContext ctx)
        {
            if (ctx == null || ctx.Owner == null)
            {
                return null;
            }

            return loggedUsers.Where(u => u.Username == ctx.Owner.Username)
                              .FirstOrDefault();
        }
    }
}
