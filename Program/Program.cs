using TimeExtendedLib;

namespace Program
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            int h = 10;
            int m = 2;
            int s = 6;
            var t = new Time(h,m,s);
            Console.WriteLine(t);
        }
    }
}