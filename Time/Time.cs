using System;
using TimePeriodLib;
using System.Diagnostics.CodeAnalysis;

namespace TimeLib
{
    /// <summary>
    /// A structure pointing to a certain point in time via Hour, Minute and Second property
    /// </summary>
    public struct Time : IEquatable<Time>, IComparable<Time>
    {
        #region Fields

        private int seconds = 0; // 1 Day = 86 400 Seconds

        #endregion

        #region Properties

        /// <summary>
        /// Property containing an hour of a point in time as a 'byte' (ranges from 0 to 23)
        /// </summary>
        public byte Hour
        {
            get => (byte)(seconds / 60 / 60);
        }
        /// <summary>
        /// Property containing a minute of a point in time as a 'byte' (ranges from 0 to 59)
        /// </summary>
        public byte Minute
        {
            get => (byte)(seconds / 60 % 60);
        }
        /// <summary>
        /// Property containing a second of a point in time as a 'byte' (ranges from 0 to 59)
        /// </summary>
        public byte Second
        {
            get => (byte)(seconds % 60);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates Time by using Hour, Minute and Second as byte variable
        /// </summary>
        public Time(byte? hour, byte? minute, byte? second)
        {
            if (hour is null || minute is null || second is null) throw new ArgumentNullException();
            if (hour >= 24 || minute >= 60 || second >= 60) throw new ArgumentOutOfRangeException();

            seconds = (int)(second + (minute * 60) + (hour * 60 * 60));
        }
        /// <summary>
        /// Creates Time by using Hour and Minute as byte variable and Second as 0
        /// </summary>
        public Time(byte? hour, byte? minute) : this(hour, minute, 0) { }
        /// <summary>
        /// Creates Time by using Hour as byte variable and Second and Minute as 0
        /// </summary>
        public Time(byte? hour) : this(hour, 0, 0) { }
        /// <summary>
        /// Creates Time by using Hour, Minute and Second as int variable
        /// </summary>
        public Time(int? hour, int? minute, int? second)
        {
            if (hour is null || minute is null || second is null) throw new ArgumentNullException();
            if (hour > byte.MaxValue || minute > byte.MaxValue || second > byte.MaxValue) throw new ArgumentOutOfRangeException();
            if (hour < byte.MinValue || minute < byte.MinValue || second < byte.MinValue) throw new ArgumentOutOfRangeException();

            this = new Time((byte)hour, (byte)minute, (byte)second);
        }
        /// <summary>
        /// Creates Time by using Hour and Minute as int variable and Second as 0
        /// </summary>
        public Time(int? hour, int? minute) : this(hour, minute, 0) { }
        /// <summary>
        /// Creates Time by using Hour as int variable and Second and Minute as 0
        /// </summary>
        public Time(int? hour) : this(hour, 0, 0) { }
        /// <summary>
        /// Creates Time of 00:00:00
        /// </summary>
        public Time() : this(0, 0, 0) { }
        /// <summary>
        /// Creates Time based on string value of format HH:MM:SS
        /// </summary>
        public Time(string time)
        {
            if (time is null) throw new ArgumentNullException();

            string[] timeArr = time.Split(':');
            if (timeArr.Length != 3) throw new FormatException();

            for (int i=0;i<timeArr.Length;i++)
                if (timeArr[i].Length!=2) throw new FormatException();

            this = new Time(byte.Parse(timeArr[0]), byte.Parse(timeArr[1]), byte.Parse(timeArr[2]));
        }

        #endregion

        #region ToString

        public override string ToString() => string.Format("{0:D2}:{1:D2}:{2:D2}", Hour,Minute,Second);

        #endregion

        #region IEquatable

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is Time t) return Equals(t);

            return false;
        }

        public bool Equals(Time other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (Hour == other.Hour && Minute == other.Minute && Second == other.Second) return true;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(seconds);

        #endregion

        #region IComparable

        public int CompareTo(Time other)
        {
            if (Hour != other.Hour)
                return Hour - other.Hour;
            if (Minute != other.Minute)
                return Minute - other.Minute;
            if (Second != other.Second) 
                return Second - other.Second;
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(Time t1, Time t2) => t1.Equals(t2);
        public static bool operator !=(Time t1, Time t2) => !t1.Equals(t2);
        public static bool operator <(Time t1, Time t2) => t1.CompareTo(t2) < 0;
        public static bool operator >(Time t1, Time t2) => t1.CompareTo(t2) > 0;
        public static bool operator <=(Time t1, Time t2) => t1.CompareTo(t2) <= 0;
        public static bool operator >=(Time t1, Time t2) => t1.CompareTo(t2) >= 0;
        public static Time operator +(Time t, TimePeriod tp)
        {
            long tTotal = t.Second + (t.Minute * 60) + (t.Hour * 60 * 60);
            long tpTotal = tp.Second + (tp.Minute * 60) + (tp.Hour * 60 * 60);
            
            tTotal = (tTotal + tpTotal)%(24*60*60); //Restrict the range to max 23:59:59
            return new Time((byte)(tTotal / 60 / 60), (byte)(tTotal / 60 % 60), (byte)(tTotal % 60));
        }
        public static Time operator -(Time t, TimePeriod tp)
        {
            long tTotal = t.Second + (t.Minute * 60) + (t.Hour * 60 * 60);
            long tpTotal = tp.Second + (tp.Minute * 60) + (tp.Hour * 60 * 60);

            tTotal = tTotal - tpTotal;
            while (tTotal < 0)
                tTotal += 24 * 60 * 60;
            
            return new Time((byte)(tTotal / 60 / 60), (byte)(tTotal / 60 % 60), (byte)(tTotal % 60));
        }

        #endregion
    }
}