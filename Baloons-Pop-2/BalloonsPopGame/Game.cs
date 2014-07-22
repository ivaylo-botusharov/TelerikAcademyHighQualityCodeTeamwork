namespace BalloonsPop
{
    using System;

    using Wintellect.PowerCollections;

    public static class Game
    {
        private static Playfield playfield;
        private static PopStrategy popStrategy;
        private static int balloonsLeft;
        private static int userMoves;
        private static OrderedMultiDictionary<int, string> statistics = new OrderedMultiDictionary<int, string>(true);

        public static void Start()
        {
            Playfield gamePlayfield = InitializePlayfield();
            PopStrategy gamePopStrategy = new RecursivePopStrategy();

            InitializeGame(gamePlayfield, gamePopStrategy);
            ConsoleIOEngine.PrintWelcomeMessage();
            ConsoleIOEngine.PrintTable(playfield);
            PlayGame();
        }

        private static void InitializeGame(Playfield gamePlayfield, PopStrategy gamePopStrategy)
        {
            playfield = gamePlayfield;
            popStrategy = gamePopStrategy;
            balloonsLeft = gamePlayfield.Width * gamePlayfield.Height;
            userMoves = 0;
        }

        private static Playfield InitializePlayfield()
        {
            int playfieldSize = ConsoleIOEngine.ReadPlayfieldSize();
            Playfield playfield = null;

            bool isPlayfieldSizeIncorrect = true;

            while (isPlayfieldSizeIncorrect)
            {
                PlayfieldFactory playfiledFactory = null;

                switch (playfieldSize)
                {
                    case 1:
                        playfiledFactory = new SmallPlayfieldFactory();
                        playfield = playfiledFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    case 2:
                        playfiledFactory = new MediumPlayfieldFactory();
                        playfield = playfiledFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    case 3:
                        playfiledFactory = new LargePlayfieldFactory();
                        playfield = playfiledFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    default:
                        Console.WriteLine("You have entered incorrect field size");
                        break;
                }
            }

            return playfield;
        }

        private static void PlayGame()
        {
            while (!IsFinished)
            {
                userMoves++;

                string currentInput = ConsoleIOEngine.ReadInput();

                ProcessInput(currentInput);

                ConsoleIOEngine.PrintTable(playfield);
            }

            AddUserToScoreboard();
            ConsoleIOEngine.PrintStatistics(statistics);
            ProcessUserDescision();
        }

        private static void ProcessInput(string input)
        {
            switch (input)
            {
                case "top":
                    ConsoleIOEngine.PrintStatistics(statistics);
                    break;
                case "restart":
                    Start();
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    ProcessInputBalloonPosition(input);
                    break;
            }
        }

        private static bool IsFinished
        {
            get
            {
                return balloonsLeft == 0;
            }
        }

        private static void Exit()
        {
            ConsoleIOEngine.PrintExitMessage(userMoves, balloonsLeft);

            Environment.Exit(0);
        }

        private static void ProcessInputBalloonPosition(string input)
        {
            try
            {
                var splittedUserInput = input.Split(' ');
                int currentRow = int.Parse(splittedUserInput[0]);
                int currentCol = int.Parse(splittedUserInput[1]);

                if (IsLegalMove(currentRow, currentCol))
                {
                    RemoveAllBaloons(currentRow, currentCol);
                }
                else
                {
                    ConsoleIOEngine.PrintInvalidMove();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Row and col are not entered in the valid format.");
                ConsoleIOEngine.PrintInvalidInput();
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("You did not enter two numbers for row and col.");
                ConsoleIOEngine.PrintInvalidInput();
            }
        }

        private static void AddUserToScoreboard()
        {
            ConsoleIOEngine.PrintWinMessage(userMoves);

            string username = Console.ReadLine();

            statistics.Add(userMoves, username);
        }

        private static void ProcessUserDescision()
        {
            Console.WriteLine("Do you want to play again: Yes/No");
            string userDescision = Console.ReadLine().ToLower();

            if (userDescision == "yes")
            {
                Start();
            }
            else
            {
                Exit();
            }
        }

        private static void RemoveAllBaloons(int row, int col)
        {
            balloonsLeft -= popStrategy.PopBaloons(row, col, playfield);
        }

        private static bool IsLegalMove(int row, int col)
        {
            bool isValidRow = (row >= 0) && (row < playfield.Height);
            bool isValidCol = (col >= 0) && (col < playfield.Width);

            if (isValidRow && isValidCol)
            {
                return playfield.Field[row, col] != ".";
            }
            else
            {
                return false;
            }
        }
    }
}