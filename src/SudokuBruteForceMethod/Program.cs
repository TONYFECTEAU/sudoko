using System;

namespace SudokuBruteForceMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sudoku puzzle solve...");
            //string puzzleCode = "---------------------------------------------------------------------------------";
            //string puzzleCode = "---12-83-----481-7-713---2----4613--1---9---6--9735----1---396-9-358-----25-16---";
            //string puzzleCode = "1--12-83-----481-7-713---2----4613--1---9---6--9735----1---396-9-358-----25-16---";
            //string puzzleCode = "--9--6-----4-1--5----4---7--9---5-24--2---9--47-3---6--5---9----6--7-3-----2--8--";
            //string puzzleCode = "--51---7-8----3------2----5-6-8-91-3---------3-97-2-4-4----5------3----1-7---63--";
            string puzzleCode =   "7--6----1----3-48-5----1--9-95--7----2--9--6----1--92-8--4----3-47-1----2----3--8";

            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);

            Console.WriteLine();
            Console.WriteLine("Source puzzle  Solved puzzle");
            Console.WriteLine("=============  =============");

            var sourceLines = sourcePuzzle.ToString().Split("\r\n");
            var solvedLines = solvedPuzzle.ToString().Split("\r\n");

            for(int line = 0; line < sourceLines.Length; line++)
            {
                Console.WriteLine($"{sourceLines[line]}      {solvedLines[line]}");
            }

            Console.WriteLine();
            Console.WriteLine(bruteForce.AttempDataDump());
            Console.Write("Press enter to end:");
            Console.ReadLine();
        }
    }
}
