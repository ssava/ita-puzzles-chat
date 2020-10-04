using System;

namespace ItaSoftware.Puzzles.Chat.Domain
{
    public interface IUser : IComparable
    {
        UserContext Context { get; }
        Guid Id { get; }
        string Username { get; }

        bool Send(string message);
    }

    public sealed class User : IUser
    {
        public Guid Id { get; private set; }
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

        public bool Send(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }

            Context.Messages.Enqueue(string.Format("GOTROOMMSG {0} {1}", Username, message));

            return true;
        }

        public int CompareTo(object obj)
        {
            User userObj = (User)obj;

            return Username.CompareTo(userObj.Username);
        }
    }
}
