using System;
using System.Windows;

namespace BMCWindows.Utilities
{
    public class EventMediator
    {
        private static readonly EventMediator _instance = new EventMediator();
        public static EventMediator Instance => _instance;

        public event Action<string> PlayerJoined; // Ahora recibe el nombre del jugador
        public event Action<string> PlayerLeft;   // Ahora recibe el nombre del jugador

        public void NotifyPlayerJoined(string username)
        {
            PlayerJoined?.Invoke(username); // Invoca el evento con el nombre del jugador
            MessageBox.Show($"Notificación de jugador unido: {username}");
        }

        public void NotifyPlayerLeft(string username)
        {
            PlayerLeft?.Invoke(username); // Invoca el evento con el nombre del jugador
        }
    }
}
