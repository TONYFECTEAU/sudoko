using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuBruteForceMethod
{
    /// <summary>
    /// Puzzle Brute Force algorithm
    /// </summary>
    public class PuzzleBruteForceMethod
    {
        public readonly ILogger Log;
        public long[,] PuzzleCellAttemps = new long[9, 9];
        public int LogIndent = 0;

        /// <summary>
        /// Constructor, pass in log interface
        /// </summary>
        /// <param name="log"></param>
        public PuzzleBruteForceMethod(ILogger log)
        {
            Log = log;
        }

        /// <summary>
        /// Sum of all attempts to solve pouzzle
        /// </summary>
        public long AttemptsTotalSum
        {
            get
            {
                return PuzzleCellAttemps.Cast<long>().Sum();
            }
        }
        /// <summary>
        /// Solve the Sudoko puzzle
        /// </summary>
        /// <param name="puzzle">Puzzle to solve</param>
        /// <returns></returns>
        public Puzzle SolveItNowPlease(Puzzle puzzle)
        {
            if (!puzzle.ValidatePuzzle(Log))
            {
                return puzzle;
            }

            var solvedPuzzle = TryCellSolve(puzzle);
            if (solvedPuzzle.PuzzleIsSolved)
            {
                return solvedPuzzle;
            }
            return puzzle;
        }

        /// <summary>
        /// The main worker for solving the puzzle
        /// Uses recursion to keep trying to solve the puzzle
        /// </summary>
        /// <param name="puzzle"></param>
        /// <returns></returns>
        private Puzzle TryCellSolve(Puzzle puzzle)
        {
            LogIndent++;
            try
            {
                var possibleValues = puzzle.GetPossibleValues();
                if (possibleValues.Count == 0)
                {
                    return SolvedPuzzle(puzzle);
                }

                // Houston, we have an issue!  Cannot be solved
                if (possibleValues.Any(v => v.Values.Count() == 0))
                {
                    LogWithIndent(LogIndent, $"Unable to solve this puzzle!");
                    return puzzle;
                }

                var copyPuzzle = new Puzzle(puzzle);
               
                // Now for the fun recursion!
                var tryCell = possibleValues[0];
                foreach (var tryValue in tryCell.Values)
                {
                    AddAttempt(tryCell.X, tryCell.Y);
                    copyPuzzle[tryCell.X, tryCell.Y] = tryValue;
                    LogWithIndent(LogIndent, $"try cell[{tryCell.X},{tryCell.Y}] = {tryValue}...");
                    var returnedPuzzle = TryCellSolve(copyPuzzle);
                    if (returnedPuzzle.PuzzleIsSolved)
                    {
                        if (LogIndent == 1)
                        {
                            var sumAttempts = AttemptsTotalSum;
                            LogWithIndent(LogIndent, $"Puzzle was solved in {sumAttempts} steps!");
                        }
                        return SolvedPuzzle(returnedPuzzle);
                    }
                }
            }
            finally
            {
                LogIndent--;
            }

            // Not resolved, so return the original puzzle, and sigh in defeat!
            LogWithIndent(LogIndent, "Puzzle was **not** solved!");
            return puzzle;
        }

        private void AddAttempt(byte x, byte y)
        {
            PuzzleCellAttemps[x, y]++;
        }

        private Puzzle SolvedPuzzle(Puzzle puzzle)
        {
            puzzle.PuzzleIsSolved = true;
            return puzzle;
        }

        private void LogWithIndent(int indent, string message)
        {
            Log.LogInfo($"{message.PadLeft(message.Length + indent, ' ')}");
        }

        /// <summary>
        /// Output puzzle data attempts in readable format
        /// </summary>
        /// <returns></returns>
        public string AttempDataDump()
        {
            var dumpOutput = new StringBuilder();
            dumpOutput.AppendLine($"# of attempts by cell to solve the grid, total of {AttemptsTotalSum}");
            dumpOutput.AppendLine("----------------------------------------------------------------");
            dumpOutput.Append($"        ");
            for (int col = 1; col <= 9; col++)
            {
                dumpOutput.Append($"Col: {Convert.ToString(col)} ");
            }
            dumpOutput.AppendLine();

            for (byte y = 0; y < 9; y++)
            {
                dumpOutput.Append($"Row #{y+1}:");
                for (byte x = 0; x < 9; x++)
                {
                    var dataSum = PuzzleCellAttemps[x, y];
                    dumpOutput.Append($"{Convert.ToString(dataSum).PadLeft(7)}");
                }
                dumpOutput.AppendLine();
            }
            return dumpOutput.ToString();
        }
    }
}
