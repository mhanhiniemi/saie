using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Saie
{
    class ThreadCreationProgram
    {
        const int SIZE = 9;
        const int UNNASSIGNED = 0;

        public static void CallToChildThread()
        {
            try
            {
                Console.WriteLine("Lapsisäie alkaa");
                for (int counter = 0; counter <= 10; counter++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine(counter);
                }
                Console.WriteLine("Lapsisäie loppu");
            }

            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort");
            }
            finally
            {
                Console.WriteLine("Säie- exepcionia ei saatu kiinni");
            }
        }

        private static bool FindUnassignedLocation(int[,] grid, ref int row, ref int col)
        {
            for (row = 0; row < SIZE; row++)
            {
                for (col = 0; col < SIZE; col++)
                {
                    if (grid[row, col] == UNNASSIGNED)
                        return true;
                }
            }
            return false;
        }

        private static bool UsedInRow(int[,] grid, int row, int num)
        {
            for (int col = 0; col < SIZE; col++)
            {
                if (grid[row, col] == num)
                    return true;
            }
            return false;
        }

        private static bool UsedInCol(int[,] grid, int col, int num)
        {

            for (int row = 0; row < SIZE; row++)
            {
                if (grid[row, col] == num)
                    return true;
            }
            return false;
        }

        private static bool UsedInBox(int[,] grid, int boxStartRow, int boxStartCol, int num)
        {
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                {
                    if (grid[row + boxStartRow, col + boxStartCol] == num)
                        return true;
                }
            return false;
        }

        private static bool isSafe(int[,] grid, int row, int col, int num)
        {
            int rowStart = row - (row % 3);
            int colStart = col - (col % 3);
            return !UsedInRow(grid, row, num) && !UsedInCol(grid, col, num) && !UsedInBox(grid, rowStart, colStart, num);
        }

        public static bool SolveSudoku(int[,] grid)
        {
            int row, col;

            row = 0;
            col = 0;
            if (!FindUnassignedLocation(grid, ref row, ref col))
            {
                return true;
            }
            for (int num = 1; num <= 9; num++)
            {
                if (isSafe(grid, row, col, num))
                {
                    grid[row, col] = num;
                    if (SolveSudoku(grid))
                        return true;
                    grid[row, col] = UNNASSIGNED;
                }
            }
            return false;
        }

        private static void printGrid(int[,] grid)
        {
            string RowStr;
            int num;

            RowStr = "";
            Console.WriteLine("Solution:");
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    num = grid[row, col];
                    RowStr = RowStr + num.ToString();
                }
                Console.WriteLine(RowStr);
                RowStr = "";
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            /* ThreadStart childref = new ThreadStart(CallToChildThread);
             Console.WriteLine("In Main: Creating the Child thread");
             Thread childThread = new Thread(childref);
             childThread.Start();

             //stop the main thread for some time
             Thread.Sleep(2000);

             //now abort the child
             Console.WriteLine("In Main: Aborting the Child thread");

             childThread.Abort();
             Console.ReadKey(); */
            //           int row, col;
            //            row = 0;
            //            col = 0;
            int[,] a = new int[SIZE, SIZE] {
                { 3, 0, 6, 5, 0, 8, 4, 0, 0 },
                { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
                { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
                { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
                { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
                { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
                { 0, 0, 5, 2, 0, 6, 3, 0, 0 }};


            if (SolveSudoku(a))
            {
                printGrid(a);
            }
            else
            {
                Console.WriteLine("No solution exists");
            }

            int[,] b = new int[SIZE, SIZE] {
                { 7, 1, 2, 0, 9, 8, 5, 0, 4 },
                { 0, 0, 5, 7, 0, 2, 0, 8, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 7, 0 },
                { 5, 9, 0, 0, 0, 0, 7, 0, 2 },
                { 2, 0, 0, 0, 4, 0, 0, 3, 5 },
                { 8, 0, 0, 5, 0, 0, 0, 9, 6 },
                { 4, 0, 7, 0, 0, 0, 1, 0, 0 },
                { 9, 0, 3, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 9 }
            };

            if (SolveSudoku(b))
            {
                printGrid(b);
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            string line = "";
            string[] entries;
            int rounds=0;
            using (StreamReader sw = new StreamReader("testi.txt"))
            {
                while ((line = sw.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    entries = line.Split(',');
                    //Console.WriteLine("entries: {0}", entries.Length);
                    for (int col = 0; col < SIZE; col++)
                    {
                        a[rounds, col] = int.Parse(entries[col]);
                    }
                    rounds++;
                    if (rounds == SIZE)
                    {
                        if (SolveSudoku(a))
                        {
                            printGrid(a);
                        }
                        else
                        {
                            Console.WriteLine("No solution exists");
                        }
                        rounds = 0;
                    }
                }

            }
            //            Console.WriteLine("Päivää! Studio");

        }
    }
}