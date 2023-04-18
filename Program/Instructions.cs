using System;
using TimeExtendedLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimePeriodExtendedLib;

namespace Program
{
    public static partial class MyConsoleApp
    {
        private static void ADD(string data)
        {
            string[] dataArr = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (dataArr.Length != 4 )
            {
                InvalidArgNum(4, dataArr.Length);
                return;
            }

            try
            {
                raceMembers.Add(new RaceMember(NextID(), dataArr[1], dataArr[2], new Time(dataArr[3])));
            }
            catch (Exception)
            {
                UnknownError();
            }
        }
        private static void DEL(string data)
        {
            string[] dataArr = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (dataArr.Length != 2)
            {
                InvalidArgNum(2, dataArr.Length);
                return;
            }
            int id;
            try
            {
                id = int.Parse(dataArr[1]);
            } catch (Exception)
            {
                UnknownError();
                return;
            }
            id = raceMembers.FindIndex(x => x.Id == id);
            if (id == -1) 
            {
                IdNotFound();
                return;
            }
            raceMembers.Remove(raceMembers[id]);
        }
        private static void CMP(string data)
        {
            string[] dataArr = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if(dataArr.Length != 3 )
            {
                InvalidArgNum(3, dataArr.Length);
                return;
            }

            //find ids
            int id1;
            int id2;
            try
            {
                id1 = int.Parse(dataArr[1]);
                id2 = int.Parse(dataArr[2]);
            } catch ( Exception )
            {
                UnknownError();
                return;
            }
            id1 = raceMembers.FindIndex(x => x.Id == id1);
            id2 = raceMembers.FindIndex(x => x.Id == id2);
            if (id1 == -1 || id2 == -1) 
            {
                IdNotFound();
                return;
            }

            //return message
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            var sb = new StringBuilder();
            if (raceMembers[id1].FinishTime == raceMembers[id2].FinishTime)
            {
                sb.Append($"{raceMembers[id1].Name} {raceMembers[id2].Surname}'s finish time was indifferent from {raceMembers[id2].Name} {raceMembers[id2].Surname}'s");
            } else 
            if (raceMembers[id1].FinishTime < raceMembers[id2].FinishTime)
            {
                var timeDifference = new TimePeriod(raceMembers[id1].FinishTime, raceMembers[id2].FinishTime);
                sb.AppendLine($"{raceMembers[id1].Name} {raceMembers[id1].Surname} finished before {raceMembers[id2].Name} {raceMembers[id2].Surname}");
                sb.Append($"They were faster by {timeDifference.Hour} hour(s), {timeDifference.Minute} minute(s), {timeDifference.Second} second(s) and {timeDifference.Milisecond} milisecond(s)");
            } else 
            {
                var timeDifference = new TimePeriod(raceMembers[id1].FinishTime, raceMembers[id2].FinishTime);
                sb.AppendLine($"{raceMembers[id1].Name} {raceMembers[id1].Surname} finished after {raceMembers[id2].Name} {raceMembers[id2].Surname}");
                sb.Append($"They were slower by {timeDifference.Hour} hour(s), {timeDifference.Minute} minute(s), {timeDifference.Second} second(s) and {timeDifference.Milisecond} milisecond(s)");
            }
            Console.WriteLine(sb);
            Console.ForegroundColor = consoleColor;
        }
        private static void LST() 
        { 
            ConsoleColor consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var member in raceMembers)
            {
                Console.WriteLine(member);
            }
            Console.ForegroundColor = consoleColor;
        }
        private static void CLS()
        {
            shouldContinue = false;
        }

        private static void NotRecognized(string invalid)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("The following instruction was not recognized: {0}\nMake sure there are no typos!", invalid));

            Console.ForegroundColor = consoleColor;
        }
        private static void InvalidArgNum(int expected, int actual)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Invalid number of arguments. Expected: {0}, but recieved {1} instead", expected, actual));

            Console.ForegroundColor = consoleColor;
        }
        private static void UnknownError()
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Oops! Somethig went wrong!"));

            Console.ForegroundColor = consoleColor;
        }
        private static void IdNotFound()
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("ID was not found"));

            Console.ForegroundColor = consoleColor;
        }
        private static void DuplicateId()
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("A race member with this ID already exists"));

            Console.ForegroundColor = consoleColor;
        }
    }
}
