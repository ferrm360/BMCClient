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
        public event Action<string> OnPlayerReadyEvent;
        public event Action<AttackPositionDTO> OnAttackReceivedEvent;
        public event Action<bool> OnTurnChangedEvent;
        public event Action<string> OnGameOverEvent;
        public event Action<CellDeadDTO> OnCellDeadEvent;


        public void OnAttackReceived(AttackPositionDTO attackPosition)
        {
            OnAttackReceivedEvent?.Invoke(attackPosition);
        }

        public void OnGameStarted()
        {
            OnGameStartedEvent?.Invoke();
        }

        public void OnPlayerReady(string player)
        {
            OnPlayerReadyEvent?.Invoke(player);
        }

        public void OnTurnChanged(bool isPlayerTurn)
        {
            OnTurnChangedEvent?.Invoke(isPlayerTurn);
        }

        public void OnGameOver(string loser)
        {
            OnGameOverEvent?.Invoke(loser);
        }

        public void OnCellDead(CellDeadDTO cellDeadDTO)
        {
            OnCellDeadEvent?.Invoke(cellDeadDTO);
        }

        public void OnGameOver()
        {
            throw new NotImplementedException();
        }
    }
}
