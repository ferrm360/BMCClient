using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.GameplayAttack.Rules
{
    public class GameRules
    {
        private readonly int[,] _playerMatrixLife;
        private readonly string[,] _playerMatrixName;

        public GameRules(int[,] playerMatrixLife, string[,] playerMatrixName)
        {
            _playerMatrixLife = playerMatrixLife;
            _playerMatrixName = playerMatrixName;
        }

        public (bool IsDead, string CardName, int Row, int Col) CheckForDeadCell()
        {
            for (int row = 0; row < _playerMatrixLife.GetLength(0); row++)
            {
                for (int col = 0; col < _playerMatrixLife.GetLength(1); col++)
                {
                    if (_playerMatrixLife[row, col] == 0 && !string.IsNullOrEmpty(_playerMatrixName[row, col]))
                    {
                        return (true, _playerMatrixName[row, col], row, col);
                    }
                }
            }
            return (false, null, -1, -1);
        }

        public bool IsGameOver()
        {
            for (int row = 0; row < _playerMatrixLife.GetLength(0); row++)
            {
                for (int col = 0; col < _playerMatrixLife.GetLength(1); col++)
                {
                    if (_playerMatrixLife[row, col] > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
