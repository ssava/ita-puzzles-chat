using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ItaSoftware.Puzzles.Chat.Domain
{
    internal interface IRoom : IComparable
    {
        Guid Id { get; }
        string Name { get; }
        List<IUser> Users { get; }

        void AddUser(IUser user);
        bool Contains(IUser user);
        IUser FindUserByName(string name);
    }

    internal sealed class Room : IRoom
    {
        const string RoomPattern = @"/([#&][^\x07\x2C\s]{,200})/";

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public List<IUser> Users { get; private set; }

        public static bool HasValidName(string name) =>
            Regex.IsMatch(name, RoomPattern);


        public Room(string name)
        {
            if (HasValidName(name))
                throw new ArgumentException("Room name is not valid");

            Id = Guid.NewGuid();
            Name = name;

            Users = new List<IUser>();
        }

        public int CompareTo(object obj)
        {
            Room roomObj = (Room)obj;

            return Id.CompareTo(roomObj.Id);
        }

        public void AddUser(IUser user)
        {
            if (Contains(user))
                return;

            Users.Add(user);
        }

        public bool Contains(IUser user) =>
            FindUserByName(user.Username) != null;

        public IUser FindUserByName(string name) =>
            Users?.Find(u => u.Username == name);
    }
}
