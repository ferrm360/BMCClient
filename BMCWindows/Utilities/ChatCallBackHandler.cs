using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCWindows.ChatLobbyServer;

namespace BMCWindows.Utilities
{
    public class ChatCallBackHandler : IChatLobbyServiceCallback
    {
        public event Action<string, string> MessageReceived;

        public void ReceiveMessage(string username, string message)
        {
            MessageReceived?.Invoke(username, message);
        }
    }
}
