namespace BalloonsPop.UserInputOutput
{
    using System;
    using System.Text;
    using Wintellect.PowerCollections;

    public static class ConsoleIOFacade
    {
        public static string CreateScoreboardString(OrderedMultiDictionary<int, string> statistics)
        {
            int resultsCount = Math.Min(5, statistics.Count);
            int counter = 0;

            StringBuilder scoreboard = new StringBuilder();

            scoreboard.AppendLine("Scoreboard:");

            foreach (var result in statistics)
            {
                if (counter == resultsCount)
                {
                    break;
                }
                else
                {
                    counter++;
                    var format = String.Format("{0}. {1} --> {2} moves", resultsCount, result.Value, result.Key);
                    scoreboard.AppendLine(format);
                }
            }

            return scoreboard.ToString();
        }

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

                Console.WriteLine("| ");
            }

            Console.WriteLine("    " + new String('-', playfield.Width * 3));
        }

        public static void PrintExitMessage(int userMoves, int balloonsLeft)
        {
            ConsoleMessagePrinter.PrintLine(ConsoleMessagePrinter.ByeMessage);
            ConsoleMessagePrinter.PrintLine(userMoves.ToString());
            ConsoleMessagePrinter.PrintLine(balloonsLeft.ToString());
        }

        public static void PrintWelcomeMessage()
        {
            ConsoleMessagePrinter.PrintLine(ConsoleMessagePrinter.WelcomeMessage);
        }

        public static void PrintInvalidInput()
        {
            ConsoleMessagePrinter.PrintLine(ConsoleMessagePrinter.InvalidCommandMessage);
        }

        public static void PrintInvalidMove()
        {
            ConsoleMessagePrinter.PrintLine(ConsoleMessagePrinter.InvalidMoveMessage);
        }

        public static void PrintWinMessage(int userMoves)
        {
            string message = string.Format(ConsoleMessagePrinter.WinMessageFormat, userMoves);
            ConsoleMessagePrinter.PrintLine(message);
        }

        public static string ReadInput()
        {
            ConsoleMessagePrinter.Print("Enter a row and column: ");

            string userInput = Console.ReadLine();

            return userInput;
        }

        public static string ReadUserName()
        {
            ConsoleMessagePrinter.Print("Please enter your name for the top scoreboard: ");

            string userName = Console.ReadLine();

            return userName;
        }

        public static int ReadPlayfieldSize()
        {
            ConsoleMessagePrinter.PrintLine("You can choose from the following playfield sizes: ");
            ConsoleMessagePrinter.PrintLine("1 - Small");
            ConsoleMessagePrinter.PrintLine("2 - Medium");
            ConsoleMessagePrinter.PrintLine("3 - Large");
            ConsoleMessagePrinter.PrintLine("Enter number: ");

            string input = Console.ReadLine();
            int size;
            bool isInputCorrect = int.TryParse(input, out size);

            while (!isInputCorrect)
            {
                ConsoleMessagePrinter.PrintLine("Wrong input. Enter again: ");
                input = Console.ReadLine();
                isInputCorrect = int.TryParse(input, out size);
            }

            return size;
        }
    }
}
