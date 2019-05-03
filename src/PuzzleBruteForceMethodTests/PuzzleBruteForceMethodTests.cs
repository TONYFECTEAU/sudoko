using SudokuBruteForceMethod;
using System;
using Xunit;

namespace PuzzleBruteForceMethodTests
{ 
    public class PuzzleBruteForceMethodTests
    {
        [Fact]
        public void TestEmptyPuzzle_solve()
        {
            string puzzleCode = "---------------------------------------------------------------------------------";
            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);
            Assert.True(solvedPuzzle.PuzzleIsSolved);
            Assert.Equal(84, bruteForce.AttemptsTotalSum);
        }

        [Fact]
        public void TestEasyPuzzle_solve()
        {
            string puzzleCode = "---12-83-----481-7-713---2----4613--1---9---6--9735----1---396-9-358-----25-16---";
            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);
            Assert.True(solvedPuzzle.PuzzleIsSolved);
            Assert.Equal(46, bruteForce.AttemptsTotalSum);
        }

        [Fact]
        public void TestEvilPuzzle_solve()
        {
            string puzzleCode = "--9--6-----4-1--5----4---7--9---5-24--2---9--47-3---6--5---9----6--7-3-----2--8--";
            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);
            Assert.True(solvedPuzzle.PuzzleIsSolved);
            Assert.Equal(120, bruteForce.AttemptsTotalSum);
        }

        [Fact]
        public void TestBadDuplicateColumnPuzzle_unsolved()
        {
            string puzzleCode = "1--12-83-----481-7-713---2----4613--1---9---6--9735----1---396-9-358-----25-16---";
            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);
            Assert.False(solvedPuzzle.PuzzleIsSolved);
            Assert.Equal(0, bruteForce.AttemptsTotalSum);
        }

        [Fact]
        public void TestBadDuplicateRowPuzzle_unsolved()
        {
            string puzzleCode = "1---2-831----481-7-713---2----4613--1---9---6--9735----1---396-9-358-----25-16---";
            var bruteForce = new PuzzleBruteForceMethod(new ConsoleLogger());
            var sourcePuzzle = new Puzzle(puzzleCode);
            var solvedPuzzle = bruteForce.SolveItNowPlease(sourcePuzzle);
            Assert.False(solvedPuzzle.PuzzleIsSolved);
            Assert.Equal(0, bruteForce.AttemptsTotalSum);
        }
    }

}