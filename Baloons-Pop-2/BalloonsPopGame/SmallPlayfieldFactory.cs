namespace BalloonsPop
{
    public class SmallPlayfieldFactory: PlayfieldFactory
    {
        private const int Width = 5;
        private const int Height = 5;

        public override Playfield CreatePlayfield()
        {
            return new Playfield(Width, Height);
        }
    }
}
