namespace BalloonsPop.UserInputOutput
{
    using System;

    public static class ConsoleMessagePrinter
    {
        public static string ByeMessage = "Good Bye";
        public static string WelcomeMessage = "Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.";
        public static string InvalidCommandMessage = "Invalid move or command";
        public static string InvalidMoveMessage = "Illegal move: cannot pop missing balloon!";
        public static string WinMessageFormat = "Congratulations! You popped all balloons in {0} moves.";

        public static void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void Print(string message)
        {
            Console.Write(message);
        }
    }
}
