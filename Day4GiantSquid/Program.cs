using System;

namespace Day4GiantSquid
{
    class Program
    {
        private static List<int[,]> _boards = new List<int[,]>();
        private static int[,] _winningBoard = new int[,]{};
        private static int _winningNumber = new int{};
        private static int[,] _lastCompleteBoard = new int[,]{};
        private static int _lastnumber = new int{};

        static void Main(string[] args)
        {
            // Read string data.
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
                int[,] boardArr = MakeBoardAMultidimensionalArray(board);
                // Add the multidimension board to a static property of the class.
                _boards.Add(boardArr);
            }

            // Get all the numbers to be drawn from a file and add them to an 
            // array which can be looped over. 
            string numbersString = @"DrawNumbers.txt";
            // string numbersString = @"TestDrawNumbers.txt";
            int[] numbers = File.ReadAllText(numbersString)
                                .Split(",")
                                .Select(int.Parse)
                                .ToArray();


            // Loop over each drawn number.
            foreach ( int number in numbers)
            {
                // Maintain a list of indexes for the boards which have completed 
                // a row or column af the number was found on it. These boards will
                // be removed from the boards lists because there is no need to try 
                // and match the next number in the loop
                List<int> indexToRemove = new List<int>();

                // For each number, loop over all the boards 
                // to find the number on the board. 
                for ( int boardIndex = 0; boardIndex < _boards.Count; boardIndex++)
                {
                    // Find the number on the board and get the position
                    (int row, int column) position = FindNumberOnBoard(boardIndex, number);

                    // Using a random value if number was not 
                    // found to continue to next iteration
                    if(position == (13,37)) continue;

                    // Mark it and increment the counter for the col and row.
                    MarkNumberOnBoard(boardIndex, (position.row, position.column));

                    // Check if the newly drawn number caused the board to have a complete row or column.
                    bool boardComplete = IsBoardComplete(boardIndex, (position.row, position.column));

                    // If the board has a complete row or column, set the last
                    // drawn number as it will be used for the final score.
                    if (boardComplete == true)
                    {

                        // If there isn't a winning board, enter this branch to set
                        // some winning board fields which will be used later to
                        // calculate the winning board score.
                        if(_winningBoard.Length == 0)
                        {
                            _winningBoard = FixBoard(_boards[boardIndex]);
                            _winningNumber = number;
                            Bingo();
                        }

                        // If it is the last board in the boards list, set some
                        // fields which will be used later to calculate the score 
                        // of the last board which had it's row or column complete.
                        if(_boards.Count() == 1)
                        {
                            _lastCompleteBoard = FixBoard(_boards[boardIndex]);
                            _lastnumber = number;
                            LastCompleteBoard();
                        }

                        // Once a board has a completed row or column, add it to a list
                        // which will be used to remove the board from the main boards list.
                        indexToRemove.Add(boardIndex);
                    }
                    
                } 

                // Now that the number has been attempted on all the boards, 
                // the boards which had a completed row or column after
                // the number can be removed from the main list of boards.
                if (indexToRemove.Count > 0)
                {
                    // Need to order the list to remove from behind to prevent 
                    // Index out of range issues.
                    foreach (int index in indexToRemove.OrderByDescending(v => v))
                    {
                        _boards.RemoveAt(index);
                    }
                }
                
            }

        }

        private static void Bingo()
        {
            Console.WriteLine("We have a winner!!!");

            // Write the Winning board to the console.
            WriteBoard(_winningBoard); 

            // Calculate the sum of the remaining 
            // numbers on the winning board.
            int sum = SumRemainingNumbersOnBoard(_winningBoard);

            // Write some information to the console. 
            Console.WriteLine("The sum of the remaining board's numbers is: {0}", 
                                sum
                            );
            Console.WriteLine("The winning number drawn was: {0}", 
                                _winningNumber
                            );
            Console.WriteLine("The final score, {0} * {1} = {2}", 
                                sum,
                                _winningNumber,
                                sum * _winningNumber
                            );
        }

        private static void LastCompleteBoard()
        {
            Console.WriteLine("\n--- Part Two ---");
            Console.WriteLine("Last completed board");

            // Write the board to the console.
            WriteBoard(_lastCompleteBoard); 

            // Calculate the sum of the remaining 
            // numbers on the board.
            int sum = SumRemainingNumbersOnBoard(_lastCompleteBoard);

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

        private static int[,] MakeBoardAMultidimensionalArray(string grid)
        {
            int[,] result = new int[6, 6];

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
                    result[y, x] = int.Parse(col.Trim());
                    x++;
                }
                y++;
            }
            return result;
        }

        private static (int, int) FindNumberOnBoard(int boardIndex, int number)
        {
            // Loop over each col and row to find the number. 
            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    // If a number on the board matches,
                    if (_boards[boardIndex][y, x] == number)
                    {
                        return (y, x); 
                    }
                }
            }

            // Using a random value if number was not 
            // found to continue to next iteration
            return (13,37);
        }

        private static void MarkNumberOnBoard(int boardIndex, (int y, int x) position)
        {
            // Mark the matched number as -1, it will be "fixed" later, but
            // cannot be 0 as that is a valid number and may be matched.
            _boards[boardIndex][position.y, position.x] = -1;

            // Increment the column and row total of the marched number.
            _boards[boardIndex][5, position.x]++;
            _boards[boardIndex][position.y, 5]++;
        }

        private static bool IsBoardComplete(int board, (int y, int x) position)
        {
            // Check if the board has a complete row or column,
            // and if so set the winning board field
            if
            (
                _boards[board][5, position.x] == 5 || 
                _boards[board][position.y, 5] == 5
            ) return true;

            return false;
        }

        private static int SumRemainingNumbersOnBoard(int[,] board)
        {
            int sum = 0;
            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    sum += board[y, x];
                }
            }
            return sum;
        }

        private static void WriteBoard(int[,] board)
        {
            for (int y = 0; y < 6 ; y++) 
            {
                Console.WriteLine("{0, 2} {1, 2} {2, 2} {3, 2} {4, 2} {5, 2}",
                                   board[y, 0], 
                                   board[y, 1], 
                                   board[y, 2], 
                                   board[y, 3], 
                                   board[y, 4],
                                   board[y, 5] 
                                   );
            }
            Console.WriteLine();
        } 

        private static int[,] FixBoard(int[,] board)
        {
            // The winning board will have "-1" at possitions where
            // the drawn number matched the number on the board.
            // These "-1's" needs to be replace with "0" to be 
            // able to calculate the final score.

            for (int y = 0; y < 5 ; y++) 
            {
                for (int x = 0; x < 5 ; x++) 
                {
                    if (board[y, x] == -1)
                    {
                        board[y, x] = 0;
                    }
                }
            }

            return board;
        }
    }
}
