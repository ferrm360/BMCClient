using BMCWindows.GameplayServer;
using BMCWindows.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.GameplayBoard
{
    public class ConnectionHandler
    {
        private GameServiceClient _proxy;
        private GameCallbackHandler _callbackHandler;

        public ConnectionHandler(GameCallbackHandler callbackHandler)
        {
            _callbackHandler = callbackHandler;
            var context = new InstanceContext(callbackHandler);
            _proxy = new GameServiceClient(context);
        }

        public async Task<bool> NotifyPlayerReady(string lobbyId, string player)
        {
            var response = await _proxy.MarkPlayerReadyAsync(lobbyId, player);
            return response.IsSuccess;
        }

        public void CloseConnection()
        {
            try
            {
                if (_proxy.State != CommunicationState.Closed)
                    _proxy.Close();
            }
            catch
            {
                _proxy.Abort();
            }
        }
    }
}
