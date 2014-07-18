namespace BalloonsPop
{
    using System;
    using System.Collections.Generic;

    public static class ConsoleIOEngine
    {
        public static void PrintTable(Playfield playfield)
        {
            string columnHeader = "    ";
            for (int i = 0; i < playfield.Width; i++)
            {
                columnHeader += i.ToString().PadLeft(3);
            }

            Console.WriteLine(columnHeader);


            Console.WriteLine("    " + new String('-', playfield.Width * 3));



            for (int row = 0; row < playfield.Height; row++)
            {
                Console.Write(row.ToString().PadLeft(2) + " | ");
                for (int col = 0; col < playfield.Width; col++)
                {
                    Console.Write(playfield.Field[row, col].ToString().PadLeft(2) + " ");
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("    " + new String('-', playfield.Width * 3));
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
            Console.Write("You popped all balloons in {0} moves. Please enter your name for the top scoreboard:", userMoves);
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
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
        }

        public static string ReadInput()
        {
            Console.Write("Enter a row and column: ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public static int ReadPlayfieldSize()
        {
            Console.WriteLine("You can choose from the following playfield sizes: ");
            Console.WriteLine("1 - Small");
            Console.WriteLine("2 - Medium");
            Console.WriteLine("3 - Large");
            Console.WriteLine("Enter number: ");

            string input = Console.ReadLine();
            int size;
            bool isInputCorrect = int.TryParse(input, out size);
            
            while (!isInputCorrect)
            {
                Console.WriteLine("Wrong input. Enter again: ");
                input = Console.ReadLine();
                isInputCorrect = int.TryParse(input, out size);
            }

            return size;
        }
    }
}
