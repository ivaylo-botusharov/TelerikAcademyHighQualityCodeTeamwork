namespace BalloonsPop
{
    using System;
    using Wintellect.PowerCollections;

    /// <summary>
    /// This class implements the main logic of the game.
    /// </summary>
    public static class Game
    {
        private static Playfield playfield;
        private static PopStrategy popStrategy;
        private static int balloonsLeft;
        private static int userMoves;
        private static OrderedMultiDictionary<int, string> statistics = new OrderedMultiDictionary<int, string>(true);
        
        /// <summary>
        /// This property simply returns the state of the game.
        /// </summary>
        private static bool IsFinished
        {
            get
            {
                return balloonsLeft == 0;
            }
        }

        /// <summary>
        /// This method initialize the playfield and the strategy for popping the balloons and then
        /// initilize the game. After everything is ready starts the game life cycle.
        /// </summary>
        public static void Start()
        {
            Playfield gamePlayfield = InitializePlayfield();
            PopStrategy gamePopStrategy = new RecursivePopStrategy();

            InitializeGame(gamePlayfield, gamePopStrategy);
            ConsoleIOEngine.PrintWelcomeMessage();
            ConsoleIOEngine.PrintTable(playfield);
            PlayGame();
        }

        /// <summary>
        /// This method requires a playfield and a pop strategy and initialize all the game properties 
        /// to their initial state.
        /// </summary>
        /// <param name="gamePlayfield"></param>
        /// <param name="gamePopStrategy"></param>
        private static void InitializeGame(Playfield gamePlayfield, PopStrategy gamePopStrategy)
        {
            playfield = gamePlayfield;
            popStrategy = gamePopStrategy;
            balloonsLeft = gamePlayfield.Width * gamePlayfield.Height;
            userMoves = 0;
        }

        /// <summary>
        /// This method implements the factory design pattern for instantiating a playfield
        /// with the user desired dimensions.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This method runs the game life cycle, and waits for a user input on each loop. When the life cycle
        /// ends it adds the user to the score board and print the statistics.
        /// </summary>
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

        /// <summary>
        /// This method is responsible for processing the user input on each loop. If the user input is not 
        /// a valid text command it process it as coordinates for a move.
        /// </summary>
        /// <param name="input"></param>
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