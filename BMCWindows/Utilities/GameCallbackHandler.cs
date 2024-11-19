using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCWindows.GameplayServer;

namespace BMCWindows.Utilities
{
    public class GameCallbackHandler : IGameServiceCallback
    {
        public event Action OnGameStartedEvent;

        public void OnGameStarted()
        {
            OnGameStartedEvent?.Invoke();
        }
    }
}
