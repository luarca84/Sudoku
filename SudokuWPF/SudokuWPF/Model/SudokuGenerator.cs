using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWPF.Generator
{
    class SudokuGenerator
    {
        public static int BOARD_WIDTH = 9;
        public static int BOARD_HEIGHT = 9;
        int[,] board;
        private int operations;

        public int[,] Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }

        /**
         *Constructor.  Resets board to zeros
         */
        public SudokuGenerator()
        {
            Board = new int[BOARD_WIDTH,BOARD_HEIGHT];
        }

        /**
	 *Driver method for nextBoard.
	 *@param  difficult the number of blank spaces to insert
	 *@return board, a partially completed 9x9 Sudoku board
	 */
        public int[,] nextBoard(int difficulty)
        {
            Board = new int[BOARD_WIDTH,BOARD_HEIGHT];
            nextCell(0, 0);
            makeHoles(difficulty);
            return Board;

        }

        /**
         *Recursive method that attempts to place every number in a cell.
         *
         *@param x x value of the current cell
         *@param y y value of the current cell
         *@return  true if the board completed legally, false if this cell
         *has no legal solutions.
         */
        public bool nextCell(int x, int y)
        {
            int nextX = x;
            int nextY = y;
            int[] toCheck = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Random r = new Random();
            int tmp = 0;
            int current = 0;
            int top = toCheck.Length;

            for (int i = top - 1; i > 0; i--)
            {
                current = r.Next(i);
                tmp = toCheck[current];
                toCheck[current] = toCheck[i];
                toCheck[i] = tmp;
            }

            for (int i = 0; i < toCheck.Length; i++)
            {
                if (legalMove(x, y, toCheck[i]))
                {
                    Board[x,y] = toCheck[i];
                    if (x == 8)
                    {
                        if (y == 8)
                            return true;//We're done!  Yay!
                        else
                        {
                            nextX = 0;
                            nextY = y + 1;
                        }
                    }
                    else
                    {
                        nextX = x + 1;
                    }
                    if (nextCell(nextX, nextY))
                        return true;
                }
            }
            Board[x,y] = 0;
            return false;
        }

        /**
         *Given a cell's coordinates and a possible number for that cell,
         *determine if that number can be inserted into said cell legally.
         *
         *@param x       x value of cell
         *@param y       y value of cell
         *@param current The value to check in said cell.
         *@return        True if current is legal, false otherwise.
         */
        private bool legalMove(int x, int y, int current)
        {
            for (int i = 0; i < 9; i++)
            {
                if (current == Board[x,i])
                    return false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (current == Board[i,y])
                    return false;
            }
            int cornerX = 0;
            int cornerY = 0;
            if (x > 2)
                if (x > 5)
                    cornerX = 6;
                else
                    cornerX = 3;
            if (y > 2)
                if (y > 5)
                    cornerY = 6;
                else
                    cornerY = 3;
            for (int i = cornerX; i < 10 && i < cornerX + 3; i++)
                for (int j = cornerY; j < 10 && j < cornerY + 3; j++)
                    if (current == Board[i,j])
                        return false;
            return true;
        }

        /**
         *Given a completed board, replace a given amount of cells with 0s
         *(to represent blanks)
         *@param holesToMake How many 0s to put in the board.
         */
        public void makeHoles(int holesToMake)
        {
            /* We define difficulty as follows:
                Easy: 32+ clues (49 or fewer holes)
                Medium: 27-31 clues (50-54 holes)
                Hard: 26 or fewer clues (54+ holes)
                This is human difficulty, not algorighmically (though there is some correlation)
            */
            double remainingHoles = (double)holesToMake;
            Random r = new Random();
            while (remainingHoles > 0)
            {
                int i = r.Next(0, 9);
                int j = r.Next(0, 9);
                if (Board[i, j] != 0)
                {
                    Board[i, j] = 0;
                    remainingHoles--;
                }
            }
        }
    }
}
