using System;
using System.Diagnostics.CodeAnalysis;

namespace TimePeriodLib
{
    /// <summary>
    /// A structure containing a certain time period displayed by Hour, Minute and Second property
    /// </summary>
    public struct TimePeriod : IEquatable<TimePeriod>, IComparable<TimePeriod>
    {
        #region Fields

        private long seconds = 0; // 1 Day = 86 400 Seconds

        #endregion

        #region Properties

        /// <summary>
        /// Property containing an hour of a time period as a 'byte' (ranges from 0 to 255)
        /// </summary>
        public byte Hour
        {
            get => (byte)(seconds / 60 / 60);
        }
        /// <summary>
        /// Property containing a minute of a time period as a 'byte' (ranges from 0 to 59)
        /// </summary>
        public byte Minute
        {
            get => (byte)(seconds / 60 % 60);
        }
        /// <summary>
        /// Property containing a second of a time period as a 'byte' (ranges from 0 to 59)
        /// </summary>
        public byte Second
        {
            get => (byte)(seconds % 60);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates TimePeriod by using Hour, Minute and Second as byte variable
        /// </summary>
        public TimePeriod(byte? hour, byte? minute, byte? second)
        {
            if (hour is null || minute is null || second is null) throw new ArgumentNullException();
            if (minute >= 60 || second >= 60) throw new ArgumentOutOfRangeException();

            seconds = (long)(second + (minute * 60) + (hour * 60 * 60));
        }
        /// <summary>
        /// Creates TimePeriod by using Hour and Minute as byte variable and Second as 0
        /// </summary>
        public TimePeriod(byte? hour, byte? minute) : this(hour, minute, 0) { }
        /// <summary>
        /// Creates TimePeriod by using Second as byte variable and Hour and Minute as 0
        /// </summary>
        public TimePeriod(byte? second) : this(0, 0, second) { }
        /// <summary>
        /// Creates TimePeriod by using Hour, Minute and Second as int variable
        /// </summary>
        public TimePeriod(int? hour, int? minute, int? second)
        {
            if (hour is null || minute is null || second is null) throw new ArgumentNullException();
            if (hour > byte.MaxValue || minute > byte.MaxValue || second > byte.MaxValue) throw new ArgumentOutOfRangeException();
            if (hour < byte.MinValue || minute < byte.MinValue || second < byte.MinValue) throw new ArgumentOutOfRangeException();

            this = new TimePeriod((byte)hour, (byte)minute, (byte)second);
        }
        /// <summary>
        /// Creates TimePeriod by using Hour and Minute as int variable and Second as 0
        /// </summary>
        public TimePeriod(int? hour, int? minute) : this(hour, minute, 0) { }
        /// <summary>
        /// Creates TimePeriod by using Second as int variable and Hour and Minute as 0
        /// </summary>
        public TimePeriod(int? second) : this(0, 0, second) { }
        /// <summary>
        /// Creates TimePeriod of 00:00:00
        /// </summary>
        public TimePeriod() : this(0, 0, 0) { }
        /// <summary>
        /// Creates TimePeriod based on string value of format H:MM:SS
        /// </summary>
        public TimePeriod(string time)
        {
            if (time is null) throw new ArgumentNullException();

            string[] timeArr = time.Split(':');
            if (timeArr.Length != 3) throw new FormatException();

            for (int i = 1; i < timeArr.Length; i++)
                if (timeArr[i].Length != 2) throw new FormatException();

            this = new TimePeriod(byte.Parse(timeArr[0]), byte.Parse(timeArr[1]), byte.Parse(timeArr[2]));
        }
        /// <summary>
        /// Creates TimePeriod based on two Time variables
        /// </summary>
        public TimePeriod(Time t1, Time t2)
        {
            //ToDo
        }

        #endregion

        #region ToString

        public override string ToString() => string.Format("{0}:{1:D2}:{2:D2}", Hour, Minute, Second);

        #endregion

        #region IEquatable

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is TimePeriod t) return Equals(t);

            return false;
        }

        public bool Equals(TimePeriod other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (Hour == other.Hour && Minute == other.Minute && Second == other.Second) return true;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(seconds);

        #endregion

        #region IComparable

        public int CompareTo(TimePeriod other)
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
        //public static Time operator +(Time t, TimePeriod t2) => throw new NotImplementedException(); //TODO
        //public static Time operator -(Time t, TimePeriod t2) => throw new NotImplementedException(); //TODO

        #endregion
    }
}