using TimeExtendedLib;
using TimePeriodExtendedLib;

namespace Program
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            var t1 = new Time(0, 0, 0, 500);
            var tp = new TimePeriod(0, 0, 0, 600);
            var t2 = t1 + tp;
            Console.WriteLine(t1);
            Console.WriteLine(tp);
            Console.WriteLine(t2);
        }
    }
}