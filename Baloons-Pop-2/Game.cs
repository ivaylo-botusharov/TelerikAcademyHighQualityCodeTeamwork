namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        // public const int width = 5;
        // public const int length = 10;
        // public static string[,] field = new string[width, length];
        // private static StringBuilder userInput;
        private Playfield playfield;
        private int balloonsLeft;
        private int userMoves;
        private int clearedCells;
        private SortedDictionary<int, string> statistics;

        // Initialize
        // public static void CreateTable()
        // {
        //     for (int i = 0; i < width; i++)
        //     {
        //         for (int j = 0; j < length; j++)
        //         {
        //             field[i, j] = RandomGenerator.GetRandomInt();
        //         }
        //     }
        // }
        public Game(Playfield playfield)
        {
            // userInput = new StringBuilder();
            this.playfield = playfield;
            this.balloonsLeft = playfield.Width * playfield.Height;
            this.userMoves = 0;
            this.clearedCells = 0;
            this.statistics = new SortedDictionary<int, string>();
        }

        // Environment setup
        public void Start()
        {
            ConsoleIOEngine.PrintWelcomeMessage();

            // CreateTable();
            // InitializeGameSettings();
            ConsoleIOEngine.PrintTable(this.playfield);
            this.GameLogic();
        }

        public void GameLogic()
        {
            while (true)
            {
                this.userMoves++;
                this.PlayGame();

                // userInput.Clear();
            }
        }

        // Does nothing
        private static void ClearEmptyCells()
        {
            // int row;
            // int col;

            // Queue<string> temp = new Queue<string>();

            // for (col = playfield.Height - 1; col >= 0; col--)
            // {
            //     for (row = playfield.Width - 1; row >= 0; row--)
            //     {
            //         if (playfield.Field[row, col] != ".")
            //         {
            //             temp.Enqueue(playfield.Field[row, col]);
            //             playfield.Field[row, col] = ".";
            //         }
            //     }               
            //     row = 4;               
            //     while (temp.Count > 0)
            //     {
            //         playfield.Field[row, col] = temp.Dequeue();
            //         row--;
            //     }               
            //     temp.Clear();
            // }
        }

        private void Restart()
        {
            this.Start();
        }

        private void Exit()
        {
            ConsoleIOEngine.PrintExitMessage(this.userMoves, this.balloonsLeft);

            Environment.Exit(0);
        }
        
        private void PlayGame()
        {
            int currentRow = -1;
            int currentCol = -1;

            // Play:
            //    ReadInput();
            if (this.IsFinished())
            {
                ConsoleIOEngine.PrintRegisterTopScoreMessage(this.userMoves);
                this.statistics.Add(this.userMoves, string.Empty);
                ConsoleIOEngine.PrintStatistics(this.statistics);
                this.Start();
            }

            string currentInput = ConsoleIOEngine.ReadInput();

            this.ProcessInput(currentInput);

            string activeCell;

            var splittedUserInput = currentInput.Split(' ');

            try
            {
                currentRow = int.Parse(splittedUserInput[0]);
                currentCol = int.Parse(splittedUserInput[1]);
            }
            catch (Exception)
            {
                ConsoleIOEngine.PrintInvalidInput();
            }

            if (this.IsLegalMove(currentRow, currentCol))
            {
                activeCell = this.playfield.Field[currentRow, currentCol];
                this.RemoveAllBaloons(currentRow, currentCol, activeCell);
            }
            else
            {
                ConsoleIOEngine.PrintInvalidMove();
            }

            // ClearEmptyCells(); - does nothing
            ConsoleIOEngine.PrintTable(this.playfield);
        }

        // Main magic
        private void ProcessInput(string input)
        {
            switch (input)
            {
                case "top":
                    ConsoleIOEngine.PrintStatistics(this.statistics);
                    break;
                case "restart":
                    this.Restart();
                    break;
                case "exit":
                    this.Exit();
                    break;
                case "":
                    ConsoleIOEngine.PrintInvalidInput();
                    break;
                default:
                    break;
            }
        }

        // Update
        private void RemoveAllBaloons(int row, int col, string selectedCell)
        {
            bool isRowValid = row >= 0 && row < playfield.Height;
            bool isColValid = col >= 0 && col < playfield.Width;            

            if (isRowValid && isColValid)
            {
                bool hasCellsEqualValues = this.playfield.Field[row, col] == selectedCell;

                if (hasCellsEqualValues)
                {
                    this.playfield.Field[row, col] = ".";
                    this.clearedCells++;

                    // Up
                    this.RemoveAllBaloons(row - 1, col, selectedCell);

                    // Down
                    this.RemoveAllBaloons(row + 1, col, selectedCell);

                    // Left
                    this.RemoveAllBaloons(row, col + 1, selectedCell);

                    // Right
                    this.RemoveAllBaloons(row, col - 1, selectedCell);
                }
            }
            else
            {
                this.balloonsLeft -= this.clearedCells;
                this.clearedCells = 0;

                return;
            }
        }
        
        // Checkers
        private bool IsLegalMove(int row, int col)
        {
            if ((row < 0) || (col < 0) || (col > this.playfield.Height - 1) || (row > this.playfield.Width - 1))
            {
                return false;
            }
            else
            {
                return this.playfield.Field[row, col] != ".";
            }
        }

        private bool IsFinished()
        {
            return this.balloonsLeft == 0;
        }
    }
}