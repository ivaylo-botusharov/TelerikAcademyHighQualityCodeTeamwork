namespace BalloonsPop
{
    using System;

    public class Playfield
    {
        public const int InitialHeight = 10;
        public const int InitialWidth = 5;

        private int height;
        private int width;

        private string[,] field;

        public Playfield(int height = InitialHeight, int width = InitialWidth)
        {
            this.Height = height;
            this.Width = width;

            this.Field = new string[height, width];

            this.InitializePlayfield();
        }
        
        public int Height
        {
            get
            {
                return this.height;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Playfield height can not be negative number.");
                }

                this.height = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Playfield width can not be negative number.");
                }

                this.width = value;
            }
        }

        public string[,] Field
        {
            get
            {
                return this.field;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Playfield can not be null.");
                }

                this.field = value;
            }
        }

        private void InitializePlayfield()
        {
            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    this.Field[row, col] = RandomGenerator.GetRandomInt();
                }
            }
        }
    }
}
