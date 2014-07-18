using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalloonsPop
{
    public class LargePlayfieldFactory: PlayfieldFactory
    {
        private const int Width = 15;
        private const int Height = 15;

        public override Playfield CreatePlayfield()
        {
            return new Playfield(Width, Height);
        }
    }
}
