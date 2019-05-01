namespace Radix.Tests
{
    public static class Assert
    {
        public static void Fail()
        {
            Xunit.Assert.True(false);
        }

        public static void Pass()
        {
            Xunit.Assert.True(true);
        }
    }
}