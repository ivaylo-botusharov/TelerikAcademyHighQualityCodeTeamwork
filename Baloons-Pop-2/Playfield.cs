namespace BalloonsPops
{
    using System;

    public class Playfield
    {
        public const int InitialWidth = 5;
        public const int InitialHeight = 10;

        private int width;
        private int height;
        private string[,] field;

        public Playfield(int width = InitialWidth, int height = InitialHeight)
        {
            this.Width = width;
            this.Height = height;
            this.Field = new string[this.Width, this.Height];

            this.InitializePlayfield();
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
            for (int row = 0; row < this.Width; row++)
            {
                for (int col = 0; col < this.Height; col++)
                {
                    this.Field[row, col] = RandomGenerator.GetRandomInt();
                }
            }
        }
    }
}
