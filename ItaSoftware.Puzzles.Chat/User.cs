using System;

namespace ItaSoftware.Puzzles.Chat
{
    internal sealed class User : IComparable
    {
        public string Username { get; set; }

        public int CompareTo(object obj)
        {
            User userObj = (User)obj;

            return Username.CompareTo(userObj.Username);
        }
    }
}
