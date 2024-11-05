using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMCWindows.Utilities.Callbacks
{
    public class LobbyCallbackHandler : LobbyServer.ILobbyServiceCallback
    {
        public event Action<string, string> PlayerJoined;

        public void NotifyPlayerJoined(string playerName, string lobbyId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Console.WriteLine($"Notificando que el jugador {playerName} se ha unido a la lobby {lobbyId}.");
                PlayerJoined?.Invoke(playerName, lobbyId);
            });
        }

    }
}
