namespace BalloonsPop
{
    public class Program
    {
        public static void Main()
        {
            var playfieldDimensions = ConsoleIOEngine.ReadPlayfieldDimensions();
            Playfield playfield = new Playfield(playfieldDimensions.Item1, playfieldDimensions.Item2);
            PopStrategy popStrategy = new RecursivePopStrategy();

            Game game = new Game(playfield, popStrategy);

            game.Start();
        }
    }
}
