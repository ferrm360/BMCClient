using BMCWindows.ChatFriendServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Utilities
{
    public class FriendChatCallBackHandler : IChatFriendServiceCallback
    {
        public event Action<string, string, string> FriendMessageReceived;
        public void ReceiveMessage(string sender, string receiver, string message)
        {
            FriendMessageReceived?.Invoke(sender, receiver, message);
        }
    }
}
