using System;
using TimeExtendedLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class RaceMember
    {
        public int Id; 
        public string Name; 
        public string Surname;
        public Time FinishTime;
        public RaceMember(int id, string name, string surname, Time time) 
        {
            Id = id;
            Name = name;
            Surname = surname;
            FinishTime = time;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Id + " ");
            sb.Append(Name + " ");
            sb.Append(Surname + " ");
            sb.Append(FinishTime + " ");
            return sb.ToString();
        }
    }
}
