namespace BalloonsPop
{
    using System;

    public class Program
    {
        public static void Main()
        {
            int playfieldSize = ConsoleIOEngine.ReadPlayfieldSize();
            Playfield playfield = null;


            bool isPlayfieldSizeIncorrect = true;

            while (isPlayfieldSizeIncorrect)
            {
                switch (playfieldSize)
                {
                    case 1:
                        SmallPlayfieldFactory smallPlayfieldFactory = new SmallPlayfieldFactory();
                        playfield = smallPlayfieldFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    case 2:
                        MediumPlayfieldFactory mediumPlayfieldFactory = new MediumPlayfieldFactory();
                        playfield = mediumPlayfieldFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    case 3:
                        LargePlayfieldFactory largePlayfieldFactory = new LargePlayfieldFactory();
                        playfield = largePlayfieldFactory.CreatePlayfield();
                        isPlayfieldSizeIncorrect = false;
                        break;
                    default:
                        Console.WriteLine("You have entered incorrect field size");
                        break;
                }
                
            }

            PopStrategy popStrategy = new RecursivePopStrategy();
            Game game = new Game(playfield, popStrategy);

            game.Start();
        }
    }
}
