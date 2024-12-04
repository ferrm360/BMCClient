using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Utilities
{
    public class EmptyLobbyCallback : LobbyServer.ILobbyServiceCallback
    {
        public void NotifyPlayerJoined(string playerName, string lobbyId) { }
        public void NotifyPlayerLeft(string playerName, string lobbyId) { }
        public void NotifyPlayerJoinedMessage(string message) { }
        public void NotifyPlayerLeftMessage(string message) { }

        public void StartGameNotification(string lobbyId)
        {
            throw new NotImplementedException();
        }
    }
}
