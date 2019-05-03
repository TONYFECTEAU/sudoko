using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuBruteForceMethod
{
    public class ConsoleLogger: ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}
