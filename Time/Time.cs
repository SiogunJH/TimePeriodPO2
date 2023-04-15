using System;
using System.Diagnostics.CodeAnalysis;

namespace TimeLib
{
    public struct Time : IEquatable<Time>, IComparable<Time>
    {
        #region Fields

        private int seconds = 0; // 1 Day = 86 400 Seconds

        #endregion

        #region Properties

        public byte Hour
        {
            get => (byte)(seconds / 60 / 60);
        }
        public byte Minute
        {
            get => (byte)(seconds / 60 % 60);
        }
        public byte Second
        {
            get => (byte)(seconds % 60);
        }

        #endregion

        #region Constructors

        public Time(byte? hour, byte? minute, byte? second)
        {
            if (hour is null || minute is null || second is null) throw new ArgumentNullException();
            if (hour >= 24 || minute >= 60 || second >= 60) throw new ArgumentOutOfRangeException();

            seconds = (int)(second + (minute * 60) + (hour * 60 * 60));
        }
        public Time(byte? hour, byte? minute) : this(hour, minute, 0) { }
        public Time(byte? hour) : this(hour, 0, 0) { }
        public Time(int? hour, int? minute, int? second)
        {
            if (hour > byte.MaxValue || minute > byte.MaxValue || second > byte.MaxValue) throw new ArgumentOutOfRangeException();
            if (hour < byte.MinValue || minute < byte.MinValue || second < byte.MinValue) throw new ArgumentOutOfRangeException();

            this = new Time((byte)hour, (byte)minute, (byte)second);
        }
        public Time(int? hour, int? minute) : this(hour, minute, 0) { }
        public Time(int? hour) : this(hour, 0, 0) { }
        public Time() : this(0, 0, 0) { }
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
    }
}