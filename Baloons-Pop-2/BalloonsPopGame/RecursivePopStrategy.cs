namespace BalloonsPop
{
    using System;

    public class RecursivePopStrategy : PopStrategy
    {
        public override int PopBaloons(int row, int col, Playfield playfield)
        {
            if (playfield == null)
            {
                throw new ArgumentNullException("playfield", "Playfield can't be null.");
            }

            bool isRowValid = row >= 0 && row < playfield.Height;
            bool isColValid = col >= 0 && col < playfield.Width;

            if (isRowValid && isColValid)
            {
                string selectedCellValue = playfield.Field[row, col];

                if (selectedCellValue == ".")
                {
                    return 0;
                }
                else
                {
                    return this.PopBaloons(row, col, playfield, selectedCellValue);
                }
            }

            return 0;
        }

        private int PopBaloons(int row, int col, Playfield playfield, string selectedCellValue = null)
        {
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
                    poppedBaloons += this.PopBaloons(row - 1, col, playfield, selectedCellValue);

                    // Down
                    poppedBaloons += this.PopBaloons(row + 1, col, playfield, selectedCellValue);

                    // Left
                    poppedBaloons += this.PopBaloons(row, col + 1, playfield, selectedCellValue);

                    // Right
                    poppedBaloons += this.PopBaloons(row, col - 1, playfield, selectedCellValue);
                }
            }

            return poppedBaloons;
        }
    }
}
