namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;

    public static class ConsoleIOEngine
    {
        public static void PrintTable(Playfield playfield)
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
        public static void PrintExitMessage(int userMoves, int balloonsLeft)
        {
            Console.WriteLine("Good Bye \n{0} \n{1}", userMoves, balloonsLeft);
        }
        public static void PrintWelcomeMessage()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");
        }
        public static void PrintRegisterTopScoreMessage(int userMoves)
        {
            Console.Write("You popped all baloons in {0} moves. Please enter your name for the top scoreboard:", userMoves);
        }
        public static void PrintStatistics(SortedDictionary<int, string> statistics)
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
        public static void PrintInvalidInput()
        {
            Console.WriteLine("Invalid move or command");
        }
        public static void PrintInvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");
        }
        public static string ReadInput()
        {
            Console.Write("Enter a row and column: ");
            string userInput = Console.ReadLine();
            return userInput;
        }

    }
}
