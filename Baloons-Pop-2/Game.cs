using System;
namespace BalloonsPops
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

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
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");

            //CreateTable();
            InitializeGameSettings();
            PrintTable();
            GameLogic();
        }

        private static void Restart()
        {
            Start();
        }

        private static void Exit()
        {
            Console.WriteLine("Good Bye");

            Thread.Sleep(1000);

            Console.WriteLine(userMoves.ToString());
            Console.WriteLine(balloonsLeft.ToString());

            Environment.Exit(0);
        }

        private static void PlayGame()
        {
            int currentRow = -1;
            int currentCol = -1;

        //Play:
        //    ReadInput();

            string currentInput = ReadInput();

            if (currentInput == "")
            {
                InvalidInput();
            }

            if (currentInput == "top")
            {
                ShowStatistics();
                //goto Play;
            }

            if (currentInput == "restart")
            {
                Restart();
            }

            if (currentInput == "exit")
            {
                Exit();
            }

            string activeCell;

            var splittedUserInput = currentInput.Split(' ');

            try
            {
                currentRow = Int32.Parse(splittedUserInput[0]);
                currentCol = Int32.Parse(splittedUserInput[1]);
            }
            catch (Exception)
            {
                InvalidInput();
            }

            if (IsLegalMove(currentRow, currentCol))
            {
                activeCell = playfield.Field[currentRow, currentCol];
                RemoveAllBaloons(currentRow, currentCol, activeCell);
            }
            else
            {
                InvalidMove();
            }

            ClearEmptyCells();
            PrintTable();
        }


        //Main magic
        private static string ReadInput()
        {
            string userInput = string.Empty;
            if (!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                userInput = Console.ReadLine();

                return userInput;
            }
            else
            {
                Console.Write("opal;aaaaaaaa! You popped all baloons in " + userMoves + " moves." +
                              "Please enter your name for the top scoreboard:");

                statistics.Add(userMoves, userInput);
                PrintTheScoreBoard();
                Start();
            }

            return userInput;
        }

        private static void ProcessInput()
        {
        }

        public static void GameLogic()
        {
            userMoves++;

            PlayGame();
            //userInput.Clear();
            GameLogic();
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


        //Renderer
        public static void PrintTable()
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");

            for (int i = 0; i < playfield.Width; i++)
            {
                Console.Write(i + " | ");

                for (int j = 0; j < playfield.Height; j++)
                {
                    Console.Write(playfield.Field[i, j] + " ");
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------");
        }

        private static void ShowStatistics()
        {
            PrintTheScoreBoard();
        }

        private static void PrintTheScoreBoard()
        {
            int p = 0;

            Console.WriteLine("Scoreboard:");

            foreach (KeyValuePair<int, string> s in statistics)
            {
                if (p == 4)
                {
                    break;
                }
                else
                {
                    p++;
                    Console.WriteLine("{0}. {1} --> {2} moves", p, s.Value, s.Key);
                }
            }
        }


        //Exception messages

        private static void InvalidInput()
        {
            Console.WriteLine("Invalid move or command");

            GameLogic();
        }

        private static void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");

            GameLogic();
        }
    }
}