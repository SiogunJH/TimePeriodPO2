using System;
using TimePeriodExtendedLib;
using System.Diagnostics.CodeAnalysis;

namespace TimeExtendedLib
{
    public struct Time : IEquatable<Time>, IComparable<Time>
    {
        #region Fields

        private long miliseconds = 0; // 1 Day = 86 400 000 Miliseconds

        #endregion

        #region Properties

        public byte Hour
        {
            get => (byte)(miliseconds / 1000 / 60 / 60);
        }
        public byte Minute
        {
            get => (byte)(miliseconds / 1000 / 60 % 60);
        }
        public byte Second
        {
            get => (byte)(miliseconds / 1000 % 60);
        }
        public int Milisecond
        {
            get => (int)(miliseconds % 1000);
        }

        #endregion

        #region Constructors

        public Time(int? hour, int? minute, int? second, int? milisecond)
        {
            if (hour is null || minute is null || second is null || milisecond is null) throw new ArgumentNullException();
            if (hour >= 24 || minute >= 60 || second >= 60 || milisecond >= 1000) throw new ArgumentOutOfRangeException();
            if (hour < 0 || minute < 0 || second < 0 || milisecond < 0) throw new ArgumentOutOfRangeException();

            miliseconds = (long)(milisecond + (second*1000) + (minute * 1000 * 60) + (hour * 1000 * 60 * 60));
        }
        public Time(int? hour, int? minute, int? second) : this(hour, minute, second, 0) { }
        public Time(int? hour, int? minute) : this(hour, minute, 0) { }
        public Time(int? hour) : this(hour, 0) { }
        public Time() : this(0) { }
        public Time(string time)
        {
            if (time is null) throw new ArgumentNullException();

            string[] timeArr = time.Split(':');
            if (timeArr.Length != 4) throw new FormatException();

            this = new Time(int.Parse(timeArr[0]), int.Parse(timeArr[1]), int.Parse(timeArr[2]), int.Parse(timeArr[3]));
        }

        #endregion

        #region ToString

        public override string ToString() => string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", Hour,Minute,Second,Milisecond);

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
            if (Hour == other.Hour && Minute == other.Minute && Second == other.Second && Milisecond == other.Milisecond) return true;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(miliseconds);

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
            if (Milisecond != other.Milisecond)
                return Milisecond - other.Milisecond;
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
            long tTotal = t.Milisecond + (t.Second * 1000) + (t.Minute * 1000 * 60) + (t.Hour * 1000 * 60 * 60);
            long tpTotal = tp.Milisecond + (tp.Second * 1000) + (tp.Minute * 1000 * 60) + (tp.Hour * 1000 * 60 * 60);
            
            tTotal = (tTotal + tpTotal)%(24*60*60*1000); //Restrict the range to max 23:59:59
            return new Time((int)(tTotal / 1000 / 60 / 60), (int)(tTotal / 1000 / 60 % 60), (int)(tTotal / 1000 % 60), (int)(tTotal % 1000));
        }
        public static Time operator -(Time t, TimePeriod tp)
        {
            long tTotal = t.Milisecond + (t.Second * 1000) + (t.Minute * 1000 * 60) + (t.Hour * 1000 * 60 * 60);
            long tpTotal = tp.Milisecond + (tp.Second * 1000) + (tp.Minute * 1000 * 60) + (tp.Hour * 1000 * 60 * 60);

            tTotal = (tTotal - tpTotal);
            while (tTotal < 0)
                tTotal += 24 * 60 * 60 * 1000;

            return new Time((int)(tTotal / 1000 / 60 / 60), (int)(tTotal / 1000 / 60 % 60), (int)(tTotal / 1000 % 60), (int)(tTotal % 1000));
        }

        #endregion
    }
}