using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWPF.Model
{
    class SudokuStatus
    {
        public bool checkSudokuStatus(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {

                int[] row = new int[9];
                int[] square = new int[9];
                int[] column = new int[9];// (int[]) grid[i].Clone();

                for (int j = 0; j < 9; j++)
                {
                    row[j] = grid[j,i];
                    column[j] = grid[i, j];
                    square[j] = grid[(i / 3) * 3 + j / 3,i * 3 % 9 + j % 3];
                }
                if (!(validate(column) && validate(row) && validate(square)))
                    return false;
            }
            return true;
        }

        private bool validate(int[] check)
        {
            int i = 0;
            List<int> lst = new List<int>(check);
            lst.Sort();
            foreach (int number in lst)
            {
                if (number != ++i)
                    return false;
            }
            return true;
        }
    }
}
