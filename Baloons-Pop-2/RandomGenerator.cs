namespace BalloonsPops
{
    using System;

    public static class RandomGenerator
    {
        private static Random r = new Random();

        public static string GetRandomInt()
        {
            string legalChars = "1234";
            string builder = null;

            builder = legalChars[r.Next(0, legalChars.Length)].ToString();

            return builder;
        }
    }
}
