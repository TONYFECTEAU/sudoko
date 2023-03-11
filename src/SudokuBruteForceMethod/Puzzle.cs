using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuBruteForceMethod
{
    /// <summary>
    /// Puzzle class
    /// </summary>
    public class Puzzle
    {
        /// <summary>
        /// Possible Cell values
        /// </summary>
        public class PossibleCellValues
        {
            /// <summary>
            /// Column (X)
            /// </summary>
            public byte X { get; set; }

            /// <summary>
            /// Row (Y)
            /// </summary>
            public byte Y { get; set; }

            /// <summary>
            /// Array of possible values
            /// </summary>
            public byte[] Values { get; set; }

            public PossibleCellValues(byte x, byte y, byte[] values)
            {
                X = x;
                Y = y;
                Values = values;
            }
        }

        private static char[] _PuzzleDump = "-123456789".ToCharArray();

        /// <summary>
        /// PuzzleData
        /// </summary>
        public byte[,] PuzzleData = new byte[9, 9];

        /// <summary>
        /// Puzzle Is Solved flag
        /// </summary>
        public bool PuzzleIsSolved;

        /// <summary>
        /// Constructor, load string puzzle into PuzzleData
        /// </summary>
        /// <param name="data"></param>
        public Puzzle(string data)
        {
            if (data.Length != 81)
                throw new ArgumentException($"{nameof(data)} needs to have 81 characters, values of - or 1 through 9.");

            char[] cellValueArray = data.ToCharArray();
            for (byte y = 0; y < 9; y++)
            {
                for (byte x = 0; x < 9; x++)
                {
                    var cellValue = cellValueArray[x * 9 + y];
                    if (!_PuzzleDump.Contains(cellValue))
                        throw new ArgumentException($"{nameof(data)} needs to have values of - or 1 through 9, found an {cellValue}.");

                    if (cellValue >= '1' && cellValue <= '9')
                    {
                        this[x,y] = Convert.ToByte(cellValue - '0');
                    }
                }
            }
        }

        public Puzzle(Puzzle source)
        {
            PuzzleData = source.PuzzleData.Clone() as byte[,];
        }

        /// <summary>
        /// Access puzzle data, 9x9 grid, zero based
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <returns></returns>
        public byte this[byte x, byte y]
        {
            get
            {
                return PuzzleData[x, y];
            }
            set
            {
                PuzzleData[x, y] = value;
            }
        }

        /// <summary>
        /// Output puzzle data in readable format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var dumpOutput = new StringBuilder();
            for (byte y = 0; y < 9; y++)
            {
                for (byte x = 0; x < 9; x++)
                {
                    var data = this[x, y];
                    if (data >= 0 && data <= 9)
                    {
                        dumpOutput.Append(_PuzzleDump[data]);
                    }
                    else
                    {
                        dumpOutput.Append('?');
                    }
                }
                dumpOutput.AppendLine();
            }
            return dumpOutput.ToString();
        }

        /// <summary>
        /// Output puzzle to the log
        /// </summary>
        /// <param name="log"></param>
        public void Dump(ILogger log)
        {
            log.LogInfo(ToString());
        }

        /// <summary>
        /// Get pozzible values for each cell
        /// </summary>
        /// <returns>list of posible cell values</returns>
        public List<PossibleCellValues> GetPossibleValues()
        {
            var possibleValues = new List<PossibleCellValues>();
            for (byte y = 0; y < 9; y++)
            {
                for (byte x = 0; x < 9; x++)
                {
                    if (this[x,y] == 0)
                    {
                        possibleValues.Add(GetPossibleValuesXY(x, y));
                    }
                }
            }
            // Perform order in the human equivalent....  lowest length, lowest row, lowest column
            return possibleValues.OrderBy(v => v.Values.Length * 100 + v.Y * 10 + v.X).ToList();
        }

        /// <summary>
        /// Verify puzzle data is valid, no duplicates
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool ValidatePuzzle(ILogger log)
        {
            for (byte y = 0; y < 9; y++)
            {
                var row = new List<byte>();
                var col = new List<byte>();
                for (byte x = 0; x < 9; x++)
                {
                    if (this[x, y] != 0)
                    {
                        row.Add(this[x,y]);
                    }
                    if (this[y, x] != 0)
                    {
                        col.Add(this[y, x]);
                    }
                }
                if (row.Count != row.Distinct().Count())
                {
                    if (log != null)
                    {
                        log.LogInfo($"Row #{y + 1} has duplicate values!");
                    }
                    return false;
                }
                if (col.Count != col.Distinct().Count())
                {
                    if (log != null)
                    {
                        log.LogInfo($"Column #{y + 1} has duplicate values!");
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get possible values for a specifc cell
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <returns></returns>
        private PossibleCellValues GetPossibleValuesXY(byte x, byte y)
        {
            var xValues = new List<byte>();
            var yValues = new List<byte>();
            for (byte cnt = 0; cnt < 9; cnt++)
            {
                if (this[cnt, y] > 0)
                {
                    xValues.Add(this[cnt, y]);
                }
                if (this[x, cnt] > 0)
                {
                    yValues.Add(this[x, cnt]);
                }
            }

            // Also add quadrant values
            var qValues = new List<byte>();
            byte qxMod = (byte)(x / 3);
            byte qxStart = (byte)(3 * qxMod);
            byte qxEnd = (byte)(3 * qxMod + 2);

            byte qyMod = (byte)(y / 3);
            byte qyStart = (byte)(3 * qyMod);
            byte qyEnd = (byte)(3 * qyMod + 2);

            for (byte qx = qxStart; qx <= qxEnd; qx++)
            {
                for(byte qy = qyStart; qy <= qyEnd; qy++)
                {
                    if (this[qx, qy] > 0)
                    {
                        qValues.Add(this[qx, qy]);
                    }
                }
            }

            xValues = GetMissingValues(xValues);
            yValues = GetMissingValues(yValues);
            qValues = GetMissingValues(qValues);
            var possibleValues = xValues.Intersect(yValues).Intersect(qValues).ToArray();
            return new PossibleCellValues(x, y, possibleValues);
        }

        /// <summary>
        /// Based on possible values, get ,issing values
        /// </summary>
        /// <param name="listPossible">list os possible values</param>
        /// <returns></returns>
        private List<byte> GetMissingValues(List<byte> listPossible)
        {
            var results = new List<byte>();
            for(byte p = 1; p <= 9; p++)
            {
                if (!listPossible.Contains(p))
                {
                    results.Add(p);
                }
            }
            return results;
        }
    }
}
