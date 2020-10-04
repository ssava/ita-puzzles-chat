using ItaSoftware.Puzzles.Chat.Domain;
using System;
using System.Collections.Generic;

namespace ItaSoftware.Puzzles.Chat
{
    public sealed class ServerContext
    {

        public static ServerContext Create()
        {
            return new ServerContext();
        }

        private readonly ISet<IUser> loggedUsers;
        public int LoggedUserCount => loggedUsers.Count;

        private ServerContext()
        {
            loggedUsers = new SortedSet<IUser>();
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
            IUser user = FindByContext(ctx);

            if (user == null)
            {
                return false;
            }

            return IsUserLoggedIn(user.Username);
        }

        internal IUser AddUser(string user)
        {
            IUser newUser = new User(user);
            
            loggedUsers.Add(newUser);

            return newUser;
        }

        internal bool RemoveUser(string user)
        {
            return RemoveUser(new User(user));
        }

        internal bool RemoveUser(IUser user)
        {
            /* No user is removed */
            if (user == null)
            {
                return false;
            }

            /* Remove user object */
            if (LoggedUserCount <= 0)
            {
                return false;
            }

            loggedUsers.Remove(user);

            return true;
        }

        internal IUser GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            return FindUserBy(username);
        }

        private IUser FindUserBy(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                foreach(IUser user in loggedUsers)
                {
                    if (user.Username == username)
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        internal bool RemoveUser(UserContext ctx)
        {
            /* Find user by context */
            IUser user = FindByContext(ctx);

            return RemoveUser(user.Username);
        }

        private IUser FindByContext(UserContext ctx)
        {
            if (ctx == null)
                return null;

            if (ctx.Owner == null)
                return null;

            return FindUserBy(ctx.Owner.Username);
        }
    }
}

