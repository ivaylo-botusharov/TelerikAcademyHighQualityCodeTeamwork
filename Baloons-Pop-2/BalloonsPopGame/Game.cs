namespace BalloonsPop
{
    using BalloonsPop.UserInputOutput;
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
            ConsoleIOFacade.PrintWelcomeMessage();
            ConsoleIOFacade.PrintTable(this.playfield);
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
            int playfieldSize = ConsoleIOFacade.ReadPlayfieldSize();
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
                        // Extract to consoleIO
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

                string currentInput = ConsoleIOFacade.ReadInput();

                this.ProcessInput(currentInput);

                ConsoleIOFacade.PrintTable(this.playfield);
            }

            this.AddUserToScoreboard();

            string scoreboard = ConsoleIOFacade.CreateScoreboardString(this.statistics);
            Console.WriteLine(scoreboard);

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
                    ConsoleIOFacade.CreateScoreboardString(this.statistics);
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

        /// <summary>
        /// The method prints an exit message and exit the game.
        /// </summary>
        private void Exit()
        {
            ConsoleIOFacade.PrintExitMessage(this.userMoves, this.balloonsLeft);

            Environment.Exit(0);
        }

        /// <summary>
        /// The method get the user input for wanted baloon to pop. It is used if no other command from the user
        /// is executed. If the user input for row and col is not valid, the method print appropriate message to the user.
        /// </summary>
        /// <param name="input"></param>
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
                    ConsoleIOFacade.PrintInvalidMove();
                }
            }
            catch (FormatException)
            {
                //extract to consoleIO or remove
                Console.WriteLine("Row and col are not entered in the valid format.");
                ConsoleIOFacade.PrintInvalidInput();
            }
            catch (IndexOutOfRangeException)
            {
                // extract to ConsoleIOFacade or remove
                Console.WriteLine("You did not enter two numbers for row and col.");
                ConsoleIOFacade.PrintInvalidInput();
            }
        }

        /// <summary>
        /// After winning a game, this method is used to ask the user for it's name
        /// and add him / her to the scoreboard.
        /// </summary>
        private void AddUserToScoreboard()
        {
            ConsoleIOFacade.PrintWinMessage(this.userMoves);

            string username = ConsoleIOFacade.ReadUserName();

            this.statistics.Add(this.userMoves, username);
        }

        /// <summary>
        /// This method is used after the user win the game. The method simply asks the user if
        /// he / she wants to play again.
        /// </summary>
        private void ProcessUserDescision()
        {
            // extract to consoleIO
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

        /// <summary>
        /// The method pops all baloons which are neighbours to the poped baloon
        /// if they have got equal values. After all baloons are poped removed it from all baloons.
        /// </summary>
        /// <param name="row">The row position for the baloon the user wants to pop</param>
        /// <param name="col">The col position for the baloon the user wants to pop</param>
        private void RemoveAllBaloons(int row, int col)
        {
            this.balloonsLeft -= this.popStrategy.PopBaloons(row, col, this.playfield);
        }

        /// <summary>
        /// The method check if the user input for baloon to pop is valid.
        /// </summary>
        /// <param name="row">The row position for the baloon the user wants to pop</param>
        /// <param name="col">The col position for the baloon the user wants to pop</param>
        /// <returns></returns>
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