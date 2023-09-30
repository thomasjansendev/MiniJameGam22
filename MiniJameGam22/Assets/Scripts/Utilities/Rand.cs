namespace Utilities
{
    public static class Rand
    {
        public static float Between(float min, float max)
        {
            System.Random rnd = new System.Random();
            float rndGranularity = 100f;
            var val = rnd.Next((int)(min * rndGranularity), (int)(max * rndGranularity));
            return (float)val / rndGranularity;
        }

        public static bool CoinFlip()
        {
            System.Random rnd = new System.Random();
            return rnd.Next(2) == 0;
        }
    }
}