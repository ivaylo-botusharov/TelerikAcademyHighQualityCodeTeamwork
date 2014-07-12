using System;
namespace BalloonsPops
{
    using System.Collections.Generic;

    public class Game
    {
        //public const int width = 5;
        //public const int length = 10;

        //public static string[,] field = new string[width, length];
        private Playfield playfield;

        //private static StringBuilder userInput;

        private int balloonsLeft;
        private int userMoves;
        private int clearedCells;
        private SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();


        //Initialize
        //public static void CreateTable()
        //{
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < length; j++)
        //        {
        //            field[i, j] = RandomGenerator.GetRandomInt();
        //        }
        //    }
        //}

        public Game()
        {
            //userInput = new StringBuilder();
            this.playfield = new Playfield();

            this.balloonsLeft = playfield.Width * playfield.Height;
            this.userMoves = 0;
            this.clearedCells = 0;
        }


        //Environment setup

        public void Start()
        {
            ConsoleIOEngine.PrintWelcomeMessage();
            //CreateTable();
           // InitializeGameSettings();
            ConsoleIOEngine.PrintTable(this.playfield);
            this.GameLogic();
        }

        private void Restart()
        {
            Start();
        }

        private void Exit()
        {
            ConsoleIOEngine.PrintExitMessage(userMoves, balloonsLeft);

            Environment.Exit(0);
        }

        private void PlayGame()
        {
            int currentRow = -1;
            int currentCol = -1;

        //Play:
        //    ReadInput();

            if (IsFinished())
            {
                ConsoleIOEngine.PrintRegisterTopScoreMessage(userMoves);
                statistics.Add(userMoves, string.Empty);
                ConsoleIOEngine.PrintStatistics(statistics);
                Start();
            }

            string currentInput = ConsoleIOEngine.ReadInput();

            ProcessInput(currentInput);

            string activeCell;

            var splittedUserInput = currentInput.Split(' ');

            try
            {
                currentRow = Int32.Parse(splittedUserInput[0]);
                currentCol = Int32.Parse(splittedUserInput[1]);
            }
            catch (Exception)
            {
                ConsoleIOEngine.PrintInvalidInput();
            }

            if (IsLegalMove(currentRow, currentCol))
            {
                activeCell = playfield.Field[currentRow, currentCol];
                RemoveAllBaloons(currentRow, currentCol, activeCell);
            }
            else
            {
                ConsoleIOEngine.PrintInvalidMove();
            }

            // ClearEmptyCells(); - does nothing
            ConsoleIOEngine.PrintTable(playfield);
        }


        //Main magic
        private void ProcessInput(string input)
        {
            switch (input)
            {
                case "top": 
                    ConsoleIOEngine.PrintStatistics(statistics); 
                    break;
                case "restart":
                    Restart();
                    break;
                case "exit":
                    Exit();
                    break;
                case "": 
                    ConsoleIOEngine.PrintInvalidInput();
                    break;
                default:
                    break;
            }
        }

        public void GameLogic()
        {
            while (true)
            {
                this.userMoves++;
                this.PlayGame();
                //userInput.Clear();
            }
        }

        //Update
        private void RemoveAllBaloons(int row, int col, string selectedCell)
        {
            bool isRowValid = row >= 0 && row <= 4;
            bool isColValid = col <= 9 && col >= 0;            

            if (isRowValid && isColValid)
            {
                bool hasCellsEqualValues = this.playfield.Field[row, col] == selectedCell;

                if (hasCellsEqualValues)
                {
                    this.playfield.Field[row, col] = ".";
                    this.clearedCells++;

                    //Up
                    this.RemoveAllBaloons(row - 1, col, selectedCell);

                    //Down
                    this.RemoveAllBaloons(row + 1, col, selectedCell);

                    //Left
                    this.RemoveAllBaloons(row, col + 1, selectedCell);

                    //Right
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

        // Does nothing
        private static void ClearEmptyCells()
        {
            //int row;
            //int col;

            //Queue<string> temp = new Queue<string>();

            //for (col = playfield.Height - 1; col >= 0; col--)
            //{
            //    for (row = playfield.Width - 1; row >= 0; row--)
            //    {
            //        if (playfield.Field[row, col] != ".")
            //        {
            //            temp.Enqueue(playfield.Field[row, col]);
            //            playfield.Field[row, col] = ".";
            //        }
            //    }

            //    row = 4;

            //    while (temp.Count > 0)
            //    {
            //        playfield.Field[row, col] = temp.Dequeue();
            //        row--;
            //    }

            //    temp.Clear();
            //}
        }

        //Checkers
        private bool IsLegalMove(int row, int col)
        {
            if ((row < 0) || (col < 0) || (col > this.playfield.Height - 1) || (row > this.playfield.Width - 1))
            {
                return false;
            }
            else
            {
                return (this.playfield.Field[row, col] != ".");
            }
        }

        private bool IsFinished()
        {
            return (this.balloonsLeft == 0);
        }
    }
}