namespace BalloonsPop
{
    using System;

    public class RecursivePopStrategy : PopStrategy
    {
        public override int PopBaloons(int row, int col, string selectedCellValue, Playfield playfield)
        {
            if (playfield == null)
            {
                throw new ArgumentNullException("playfield", "Playfield can't be null.");
            }

            int poppedBaloons = 0;
            bool isRowValid = row >= 0 && row < playfield.Height;
            bool isColValid = col >= 0 && col < playfield.Width;

            if (isRowValid && isColValid)
            {
                if (playfield.Field[row, col] == selectedCellValue)
                {
                    // Pop current cell
                    playfield.Field[row, col] = ".";
                    poppedBaloons += 1;

                    // Up
                    poppedBaloons += this.PopBaloons(row - 1, col, selectedCellValue, playfield);

                    // Down
                    poppedBaloons += this.PopBaloons(row + 1, col, selectedCellValue, playfield);

                    // Left
                    poppedBaloons += this.PopBaloons(row, col + 1, selectedCellValue, playfield);

                    // Right
                    poppedBaloons += this.PopBaloons(row, col - 1, selectedCellValue, playfield);
                }
            }

            return poppedBaloons;
        }
    }
}
