namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        private Playfield playfield;
        private int balloonsLeft;
        private int userMoves;
        private int clearedCells;
        private SortedDictionary<int, string> statistics;

        public Game(Playfield playfield)
        {
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
            ConsoleIOEngine.PrintTable(this.playfield);
            this.GameLogic();
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

        public void GameLogic()
        {
            while (true)
            {
                this.userMoves++;
                this.PlayGame();
            }
        }
        
        private void PlayGame()
        {
            int currentRow = -1;
            int currentCol = -1;

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