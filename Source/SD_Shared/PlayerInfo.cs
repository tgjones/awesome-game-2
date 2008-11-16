using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SD.Shared
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [XmlIgnore] public string Password { get; set; }
        public string Name { get; set; }
        public DateTime Joined { get; set; }
        public DateTime LastLogin { get; set; }
        public int Balance { get; set; }

        public PlayerInfo() { }

        public PlayerInfo(int id, string email, string password, string name, DateTime joined, DateTime lastLogin, int balance)
            : this()
        {
            Id = id;
            Email = email;
            Password = password;
            Name = name;
            Joined = joined;
            LastLogin = lastLogin;
            Balance = balance;
        }
    }
}
