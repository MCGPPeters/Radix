namespace Radix
{
    internal record NoneExistentVersion : Version
    {
        public NoneExistentVersion() : base(-1)
        {

        }
    }
}
