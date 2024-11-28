using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMCWindows.GameplayBoard
{
    public class GameEventsHandler
    {
        private readonly Action _onGameStarted;
        private readonly Action<string> _onPlayerReady;

        public GameEventsHandler(Action onGameStarted, Action<string> onPlayerReady)
        {
            _onGameStarted = onGameStarted;
            _onPlayerReady = onPlayerReady;
        }

        public void HandlePlayerReady(string player)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => _onPlayerReady(player));
            });
        }

        public void HandleGameStarted()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(_onGameStarted);
            });
        }
    }
}
