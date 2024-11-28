using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.GameplayBoard
{
    public class MatrixHandler
    {
        public int[,] Matrix { get; private set; }

        public MatrixHandler(int rows, int cols)
        {
            Matrix = new int[rows, cols];
        }

        public void UpdateCell(int row, int col, int value)
        {
            Matrix[row, col] = value;
        }

        public ObservableCollection<int> FlattenMatrix()
        {
            var flatMatrix = new ObservableCollection<int>();
            foreach (var value in Matrix)
            {
                flatMatrix.Add(value);
            }
            return flatMatrix;
        }
    }
}
