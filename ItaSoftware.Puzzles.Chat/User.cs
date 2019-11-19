using System;

namespace ItaSoftware.Puzzles.Chat
{
    public sealed class User : IComparable
    {
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

        public int CompareTo(object obj)
        {
            User userObj = (User)obj;

            return Username.CompareTo(userObj.Username);
        }
    }
}
