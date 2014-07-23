namespace BalloonsPop
{
    using System;
    using Wintellect.PowerCollections;

    /// <summary>
    /// This class implements the main logic of the game.
    /// </summary>
    public class Game
    {
        private static Game instance;
        private Playfield playfield;
        private PopStrategy popStrategy;
        private int balloonsLeft;
        private int userMoves;
        private OrderedMultiDictionary<int, string> statistics = new OrderedMultiDictionary<int, string>(true);        

        private Game()
        {
        }

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }

                return instance;
            }
        }

        /// <summary>
        /// This property simply returns the state of the game.
        /// </summary>
        private bool IsFinished
        {
            get
            {
                return this.balloonsLeft == 0;
            }
        }

        /// <summary>
        /// This method initialize the playfield and the strategy for popping the balloons and then
        /// initilize the game. After everything is ready starts the game life cycle.
        /// </summary>
        public void Start()
        {
            Playfield gamePlayfield = this.InitializePlayfield();
            PopStrategy gamePopStrategy = new RecursivePopStrategy();

            this.InitializeGame(gamePlayfield, gamePopStrategy);
            ConsoleIOEngine.PrintWelcomeMessage();
            ConsoleIOEngine.PrintTable(this.playfield);
            this.PlayGame();
        }

        /// <summary>
        /// This method requires a playfield and a pop strategy and initialize all the game properties 
        /// to their initial state.
        /// </summary>
        /// <param name="gamePlayfield"></param>
        /// <param name="gamePopStrategy"></param>
        private void InitializeGame(Playfield gamePlayfield, PopStrategy gamePopStrategy)
        {
            this.playfield = gamePlayfield;
            this.popStrategy = gamePopStrategy;
            this.balloonsLeft = gamePlayfield.Width * gamePlayfield.Height;
            this.userMoves = 0;
        }

        /// <summary>
        /// This method implements the factory design pattern for instantiating a playfield
        /// with the user desired dimensions.
        /// </summary>
        /// <returns></returns>
        private Playfield InitializePlayfield()
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
        private void PlayGame()
        {
            while (!IsFinished)
            {
                this.userMoves++;

                string currentInput = ConsoleIOEngine.ReadInput();

                this.ProcessInput(currentInput);

                ConsoleIOEngine.PrintTable(this.playfield);
            }

            this.AddUserToScoreboard();
            ConsoleIOEngine.PrintStatistics(this.statistics);
            this.ProcessUserDescision();
        }

        /// <summary>
        /// This method is responsible for processing the user input on each loop. If the user input is not 
        /// a valid text command it process it as coordinates for a move.
        /// </summary>
        /// <param name="input"></param>
        private void ProcessInput(string input)
        {
            switch (input)
            {
                case "top":
                    ConsoleIOEngine.PrintStatistics(this.statistics);
                    break;
                case "restart":
                    this.Start();
                    break;
                case "exit":
                    this.Exit();
                    break;
                default:
                    this.ProcessInputBalloonPosition(input);
                    break;
            }
        }

        private void Exit()
        {
            ConsoleIOEngine.PrintExitMessage(this.userMoves, this.balloonsLeft);

            Environment.Exit(0);
        }

        private void ProcessInputBalloonPosition(string input)
        {
            try
            {
                var splittedUserInput = input.Split(' ');
                int currentRow = int.Parse(splittedUserInput[0]);
                int currentCol = int.Parse(splittedUserInput[1]);

                if (this.IsLegalMove(currentRow, currentCol))
                {
                    this.RemoveAllBaloons(currentRow, currentCol);
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

        private void AddUserToScoreboard()
        {
            ConsoleIOEngine.PrintWinMessage(this.userMoves);

            string username = Console.ReadLine();

            this.statistics.Add(this.userMoves, username);
        }

        private void ProcessUserDescision()
        {
            Console.WriteLine("Do you want to play again: Yes/No");
            string userDescision = Console.ReadLine().ToLower();

            if (userDescision == "yes")
            {
                this.Start();
            }
            else
            {
                this.Exit();
            }
        }

        private void RemoveAllBaloons(int row, int col)
        {
            this.balloonsLeft -= this.popStrategy.PopBaloons(row, col, this.playfield);
        }

        private bool IsLegalMove(int row, int col)
        {
            bool isValidRow = (row >= 0) && (row < this.playfield.Height);
            bool isValidCol = (col >= 0) && (col < this.playfield.Width);

            if (isValidRow && isValidCol)
            {
                return this.playfield.Field[row, col] != ".";
            }
            else
            {
                return false;
            }
        }
    }
}