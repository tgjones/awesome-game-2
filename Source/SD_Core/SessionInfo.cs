using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Core
{
    internal class SessionInfo
    {
        internal int player_id;
        internal Guid session_key;
        internal DateTime last_active;

        internal SessionInfo(int player_id)
        {
            this.player_id = player_id;
            this.session_key = Guid.NewGuid();
            NotifyActivity();
        }

        /// <summary>
        /// Call this method when this player is active to log the last activity time
        /// </summary>
        internal void NotifyActivity()
        {
            last_active = DateTime.Now;
        }
    }

    internal class SessionList : List<SessionInfo>
    {
        internal SessionInfo FindSession(int player_id)
        {
            foreach (SessionInfo session in this)
            {
                if (session.player_id == player_id)
                    return session;
            }
            return null;
        }

        internal SessionInfo FindSession(Guid session_key)
        {
            foreach (SessionInfo session in this)
            {
                if (session.session_key == session_key)
                    return session;
            }
            return null;
        }

    }
}
