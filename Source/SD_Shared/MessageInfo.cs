using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class MessageInfo
    {
        public int Id { get; set; }
        public int ToPlayerId { get; set; }
        public int FromPlayerId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public PlayerInfo FromPlayer { get; set; }
        public PlayerInfo ToPlayer { get; set; }

        public MessageInfo() { }

        public MessageInfo(int id, int toPlayerId, int fromPlayerId, string subject, string body)
            : this()
        {
            Id = id;
            ToPlayerId = toPlayerId;
            FromPlayerId = fromPlayerId;
            Subject = subject;
            Body = body;
        }

        public MessageInfo(int id, int toPlayerId, int fromPlayerId, string subject, string body, PlayerInfo fromPlayer, PlayerInfo toPlayer)
            : this(id, toPlayerId, fromPlayerId, subject, body)
        {
            FromPlayer = fromPlayer;
            ToPlayer = ToPlayer;
        }
    }
}
