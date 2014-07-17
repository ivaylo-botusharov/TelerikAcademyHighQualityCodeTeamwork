namespace BalloonsPop
{
    public abstract class PopStrategy
    {
        public abstract int PopBaloons(int row, int col, string selectedCellValue, Playfield playfield);
    }
}
