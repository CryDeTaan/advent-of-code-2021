using System;

namespace Day4GiantSquid
{
    class Program
    {
        private static List<int[,,]> _boards = new List<int[,,]>();
        private static int[,,] _winningBoard = new int[,,]{};
        private static int _lastnumber = new int{};
        static void Main(string[] args)
        {
            // Read string data into Array
            // string data = @"TestData.txt";
            string data = @"BingoData.txt";
            // Trim and removes all double spaces between numbers.
            string boardsData = File.ReadAllText(data).Replace("  ", " ").Trim();

            // From the string containing all the boards,
            // add each board as an item to a boards list.
            List<string> boards = MakeBoardsList(boardsData);

            // Loop over each board and convert it to a multidimensional array.
            foreach (string board in boards)
            {
                int[,,] boardArr = MakeBoardAMultidimensionalArray(board);
                // Add the multidimension board to a static property of the class.
                _boards.Add(boardArr);
            }

            // Get all the numbers to be drawn from a file and add them to an 
            // array which can be looped over. 
            string numbersString = @"DrawNumbers.txt";
            int[] numbers = File.ReadAllText(numbersString)
                                .Split(",")
                                .Select(int.Parse)
                                .ToArray();

            // Loop over each drawn number.
            foreach ( int number in numbers)
            {
                // For each number, loop over all the boards 
                // to find the number in the board. 
                for ( int i = 0; i < _boards.Count; i++)
                {
                    FindNumberOnBoard(i, number);
                    // If it is a winning board, go to the bingo function.
                    if (_winningBoard.Length > 0) goto bingo;
                } 
            }

            bingo: if (_winningBoard.Length > 0) Bingo();

        }

        private static void Bingo()
        {
            Console.WriteLine("We have a winner!!!");

            // The winning board will have "-1" at possitions where
            // the drawn number matched the number on the board.
            // These "-1's" needs to be replace with "0" to be 
            // able to calculate the final score.
            FixBoard(); 

            // Write the Winning board in its 2 different varaitions.
            WriteBoard(_winningBoard); 
            WriteMarkedBoard(_winningBoard); 

            // Calculate the sum of the remaining 
            // numbers on the winning board.
            int sum = SumRemainingNumbersOfWinningBoard(_winningBoard);

            // Write some information to the console. 
            Console.WriteLine("The sum of the remaining board's numbers is: {0}", 
                                sum
                            );
            Console.WriteLine("The last number drawn was: {0}", 
                                _lastnumber
                            );
            Console.WriteLine("The final score, {0} * {1} = {2}", 
                                sum,
                                _lastnumber,
                                sum * _lastnumber
                            );
            Environment.Exit(0);
        }

        private static List<string> MakeBoardsList(string boardsData)
        {
            // From the string containing all the boards,
            // add each board as an item to a boards list.

            List<string> boards = new List<string>();
            foreach (var board in boardsData.Split("\n\n"))
            {
                boards.Add(board);
            }

            return boards;
        }

        private static int[,,] MakeBoardAMultidimensionalArray(string grid)
        {
            int[,,] result = new int[6, 6, 2];

            // [y,x] or [x,y] - ¯\_(ツ)_/¯
            // https://stackoverflow.com/a/2203610
            int x = 0, y = 0;

            // The row number is a y coordinate
            foreach (var row in grid.Split('\n'))
            {
                // The column number is an x coordinate
                x = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[y, x, 0] = int.Parse(col.Trim());
                    x++;
                }
                y++;
            }
            return result;
        }

        private static void FindNumberOnBoard(int boardIndex, int number)
        {
            // Loop over each x and y axis to find the number. 
            bool winner = false;
            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    // If a number on the board matches,
                    if (_boards[boardIndex][y, x, 0] == number)
                    {
                        // mark it and increment the counter col
                        // for the x and y axis.
                        MarkBoardNumber(boardIndex, (y, x));
                        // Check if the newly drawn number caused the board 
                        // to have a complete row or column.
                        winner = IsAWinningBoard(boardIndex, (y, x));
                    }
                    // If the board has a complete row or column, set the last
                    // drawn number as it will be used for the final score, and
                    // return out of the function, otherwise continue.
                    if (winner == true)
                    {
                        _lastnumber = number;
                        return;
                    }
                }
            }
        }

        private static void MarkBoardNumber(int boardIndex, (int y, int x) position)
        {
            // Mark the board number by setting the 1st index dimension
            // to the drawn number to keep track of it. 
            _boards[boardIndex][position.y, position.x, 1] = _boards[boardIndex][position.y, position.x,0];
            // Mark the matched number to -1, it will be "fixed" later, but
            // cannot be 0 as that is a valid number and may be matched.
            _boards[boardIndex][position.y, position.x, 0] = -1;

            // Increment the column and row total of the marched number.
            _boards[boardIndex][5, position.x, 1]++;
            _boards[boardIndex][position.y, 5, 1]++;
        }

        private static bool IsAWinningBoard(int board, (int y, int x) position)
        {
            // Check if the board has a complete row or column,
            // and if so set the winning board field
            if (_boards[board][5, position.x, 1] == 5 || _boards[board][position.y, 5, 1] == 5)
            {
                _winningBoard = _boards[board];
                return true;
            };
            return false;
        }

        private static void FixBoard()
        {
            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    if (_winningBoard[y, x, 0] == -1)
                    {
                        _winningBoard[y, x, 0] = 0;
                    }
                }
            }

        }

        private static int SumRemainingNumbersOfWinningBoard(int[,,] winningBoard)
        {
            int sum = 0;
            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    sum += winningBoard[y, x, 0];
                }
            }
            return sum;
        }

        private static void WriteBoard(int[,,] board)
        {
            for (int y = 0; y < 5 ; y++) 
            {
                Console.WriteLine("{0, 2} {1, 2} {2, 2} {3, 2} {4, 2}",
                                   board[y, 0, 0], 
                                   board[y, 1, 0], 
                                   board[y, 2, 0], 
                                   board[y, 3, 0], 
                                   board[y, 4, 0] 
                                   );
            }
            Console.WriteLine();
        } 

        private static void WriteMarkedBoard(int[,,] board)
        {
            for (int y = 0; y < 6 ; y++) 
            {
                Console.WriteLine("{0, 2} {1, 2} {2, 2} {3, 2} {4, 2} {5, 2}",
                                   board[y, 0, 1], 
                                   board[y, 1, 1], 
                                   board[y, 2, 1], 
                                   board[y, 3, 1], 
                                   board[y, 4, 1], 
                                   board[y, 5, 1] 
                                   );
            }
            Console.WriteLine();
        } 
    }
}
