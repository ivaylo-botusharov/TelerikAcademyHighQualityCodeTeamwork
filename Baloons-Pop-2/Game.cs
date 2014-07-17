namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        private Playfield playfield;
        private PopStrategy popStrategy;
        private int balloonsLeft;
        private int userMoves;
        private SortedDictionary<int, string> statistics;

        public Game(Playfield playfield, PopStrategy popStrategy)
        {
            this.playfield = playfield;
            this.popStrategy = popStrategy;
            this.balloonsLeft = playfield.Width * playfield.Height;
            this.userMoves = 0;
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
                string selectedCellValue = this.playfield.Field[currentRow, currentCol];
                this.RemoveAllBaloons(currentRow, currentCol, selectedCellValue);
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
        private void RemoveAllBaloons(int row, int col, string selectedCellValue)
        {
            this.balloonsLeft -= this.popStrategy.PopBaloons(row, col, selectedCellValue, this.playfield);
        }
        
        // Checkers
        private bool IsLegalMove(int row, int col)
        {
            if ((row < 0) || (row > this.playfield.Height - 1) || (col < 0) || (col > this.playfield.Width - 1))
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
