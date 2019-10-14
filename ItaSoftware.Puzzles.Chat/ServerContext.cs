using System;
using System.Collections.Generic;

namespace ItaSoftware.Puzzles.Chat.Tests
{
    public sealed class ServerContext
    {
        public static ServerContext Instance { get; private set; }

        static ServerContext()
        {
            if (Instance == null)
                Instance = new ServerContext();
        }

        private readonly ISet<User> usersSet;
        public int ConnectedUsers => usersSet.Count;

        private ServerContext()
        {
            usersSet = new SortedSet<User>();
        }

        internal void AddUser(string user)
        {
            usersSet.Add(new User
            {
                Username = user
            });
        }

        internal bool IsUserLoggedIn(string user)
        {
            return usersSet.Contains(new User
            {
                Username = user
            });
        }
    }
}
