using BMCWindows.LobbyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Utilities
{
    public class LobbyCallbackHandler : ILobbyServiceCallback
    {
        public event Action<string, string> PlayerJoined;
        public event Action<string, string> PlayerLeft;
        public event Action<string> PlayerJoinedMessage;
        public event Action<string> PlayerLeftMessage;
        public event Action<string> StartGame;


        public void NotifyPlayerJoined(string playerName, string lobbyId)
        {
            PlayerJoined?.Invoke(playerName, lobbyId);
        }

        public void NotifyPlayerLeft(string playerName, string lobbyId)
        {
            PlayerLeft?.Invoke(playerName, lobbyId);
        }

        public void NotifyPlayerJoinedMessage(string message)
        {
            PlayerJoinedMessage?.Invoke(message);
        }

        public void NotifyPlayerLeftMessage(string message)
        {
            PlayerLeftMessage?.Invoke(message);
        }


        public void StartGameNotification(string lobbyId)
        {
            StartGame?.Invoke(lobbyId);
        }
    }

}
