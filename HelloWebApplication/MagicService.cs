namespace HelloWebApplication
{
    public interface IMagicService
    {
        int MagicNumber { get; }
    }

    public class MagicImplementation : IMagicService
    {
        public int MagicNumber { get; }
        public MagicImplementation()
        {
            MagicNumber = new Random().Next(1, 100);
        }
    }

    public class MagicConfImplementation : IMagicService
    {
        public int MagicNumber { get; }
        public MagicConfImplementation(IConfiguration conf)
        {
            MagicNumber = new Random().Next(1, conf.GetValue<int>("Chrono:Seuils:Err"));
        }
    }

    public class MagicRangeImplementation : IMagicService
    {
        public int MagicNumber { get; }
        public MagicRangeImplementation(int min, int max)
        {
            MagicNumber = new Random().Next(min, max);
        }
    }
}
