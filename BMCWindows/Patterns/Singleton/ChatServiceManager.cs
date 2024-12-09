using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Patterns.Singleton
{
    public static class ChatServiceManager
    {
        public static ChatServer.ChatServiceClient ChatClient { get; private set; }

        public static void InitializeChatClient(InstanceContext context)
        {
            if (ChatClient == null)
            {
                ChatClient = new ChatServer.ChatServiceClient(context);
            }
        }

        public static void CloseConnection()
        {
            if (ChatClient != null)
            {
                ChatClient.Close();
                ChatClient = null;
            }
        }
    }

}
