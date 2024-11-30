using BMCWindows.AttackServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Utilities
{
    public class AttackCallbackHandler : IAttackServiceCallback
    {
        public event Action<AttackPositionDTO> OnAttackReceivedEvent;

        public void OnAttackReceived(AttackPositionDTO attackPosition)
        {
            OnAttackReceivedEvent?.Invoke(attackPosition);
        }
    }
}
