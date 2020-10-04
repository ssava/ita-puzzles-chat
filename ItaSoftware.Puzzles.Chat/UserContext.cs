using System;
using System.Collections.Generic;
using ItaSoftware.Puzzles.Chat.Domain;

namespace ItaSoftware.Puzzles.Chat
{
    public sealed class UserContext : IComparable
    {
        public IUser Owner { get; set; }
        public ISet<string> JoinedRooms { get; private set; }

        public Queue<string> Messages { get; private set; }

        public UserContext()
        {
            JoinedRooms = new SortedSet<string>();
            Messages = new Queue<string>();
        }

        public int CompareTo(object obj)
        {
            UserContext ctxObj = (UserContext)obj;

            return Owner.CompareTo(ctxObj.Owner);
        }
    }
}
