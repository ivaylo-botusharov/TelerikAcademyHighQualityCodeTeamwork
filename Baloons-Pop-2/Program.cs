namespace BalloonsPops
{
    public class Program
    {
        public static void Main()
        {
            var playfieldDimensions = ConsoleIOEngine.ReadPlayfieldDimensions();
            Playfield playfield = new Playfield(playfieldDimensions.Item1, playfieldDimensions.Item2);
            Game game = new Game(playfield);

            game.Start();
        }
    }
}
