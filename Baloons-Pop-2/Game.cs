using System;
namespace BalloonsPops
{
    using System.Collections.Generic;

    public class Game
    {
        //public const int width = 5;
        //public const int length = 10;

        //public static string[,] field = new string[width, length];
        private static Playfield playfield;

        //private static StringBuilder userInput;

        private static int balloonsLeft;
        private static int userMoves;
        private static int clearedCells;
        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();


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

        private static void InitializeGameSettings()
        {
            //userInput = new StringBuilder();
            playfield = new Playfield();

            balloonsLeft = playfield.Width * playfield.Height;
            userMoves = 0;
            clearedCells = 0;
        }


        //Environment setup

        public static void Start()
        {
            ConsoleIOEngine.PrintWelcomeMessage();
            //CreateTable();
            InitializeGameSettings();
            ConsoleIOEngine.PrintTable(playfield);
            GameLogic();
        }

        private static void Restart()
        {
            Start();
        }

        private static void Exit()
        {
            ConsoleIOEngine.PrintExitMessage(userMoves, balloonsLeft);

            Environment.Exit(0);
        }

        private static void PlayGame()
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

            ClearEmptyCells();
            ConsoleIOEngine.PrintTable(playfield);
        }


        //Main magic

        private static void ProcessInput(string input)
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

        public static void GameLogic()
        {
            while (true)
            {
                userMoves++;
                PlayGame();
                //userInput.Clear();
            }
        }

        //Update
        private static void RemoveAllBaloons(int i, int j, string activeCell)
        {
            if ((i >= 0) && (i <= 4) && (j <= 9) && (j >= 0) && (playfield.Field[i, j] == activeCell))
            {
                playfield.Field[i, j] = ".";
                clearedCells++;

                //Up
                RemoveAllBaloons(i - 1, j, activeCell);

                //Down
                RemoveAllBaloons(i + 1, j, activeCell);

                //Left
                RemoveAllBaloons(i, j + 1, activeCell);

                //Right
                RemoveAllBaloons(i, j - 1, activeCell);
            }
            else
            {
                balloonsLeft -= clearedCells;
                clearedCells = 0;

                return;
            }
        }

        private static void ClearEmptyCells()
        {
            int i;
            int j;

            Queue<string> temp = new Queue<string>();

            for (j = playfield.Height - 1; j >= 0; j--)
            {
                for (i = playfield.Width - 1; i >= 0; i--)
                {
                    if (playfield.Field[i, j] != ".")
                    {
                        temp.Enqueue(playfield.Field[i, j]);
                        playfield.Field[i, j] = ".";
                    }
                }

                i = 4;

                while (temp.Count > 0)
                {
                    playfield.Field[i, j] = temp.Dequeue();
                    i--;
                }

                temp.Clear();
            }
        }

        //Checkers
        private static bool IsLegalMove(int i, int j)
        {
            if ((i < 0) || (j < 0) || (j > playfield.Height - 1) || (i > playfield.Width - 1))
            {
                return false;
            }
            else
            {
                return (playfield.Field[i, j] != ".");
            }
        }

        private static bool IsFinished()
        {
            return (balloonsLeft == 0);
        }
    }
}