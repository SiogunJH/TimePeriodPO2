using System;
using TimeExtendedLib;
using System.Diagnostics.CodeAnalysis;

namespace TimePeriodExtendedLib
{
    /// <summary>
    /// A structure containing a certain time period displayed by Hour, Minute and Second property
    /// </summary>
    public struct TimePeriod : IEquatable<TimePeriod>, IComparable<TimePeriod>
    {
        #region Fields

        private long miliseconds = 0; // 1 Day = 86 400 Seconds

        #endregion

        #region Properties

        public int Hour
        {
            get => (int)(miliseconds / 1000 / 60 / 60);
        }
        public byte Minute
        {
            get => (byte)(miliseconds / 1000 / 60 % 60);
        }
        public byte Second
        {
            get => (byte)(miliseconds / 1000 % 60);
        }
        public byte Milisecond
        {
            get => (byte)(miliseconds % 1000);
        }

        #endregion

        #region Constructors

        public TimePeriod(int? hour, int? minute, int? second, int? milisecond)
        {
            if (hour is null || minute is null || second is null || milisecond is null) throw new ArgumentNullException();
            if (milisecond >= 1000 || minute >= 60 || second >= 60) throw new ArgumentOutOfRangeException();
            if (milisecond < 0 || minute < 0 || second < 0 || hour < 0) throw new ArgumentOutOfRangeException();

            miliseconds = (long)(milisecond + (second*1000) + (minute*1000 * 60) + (hour*1000 * 60 * 60));
        }
        public TimePeriod(int? hour, int? minute, int? second) : this(hour, minute, second, 0) { }
        public TimePeriod(int? hour, int? minute) : this(hour, minute, 0, 0) { }
        public TimePeriod(int? hour) : this(hour, 0, 0, 0) { }
        public TimePeriod() : this(0, 0, 0, 0) { }
        public TimePeriod(string time)
        {
            if (time is null) throw new ArgumentNullException();

            string[] timeArr = time.Split(':');
            if (timeArr.Length != 4) throw new FormatException();

            this = new TimePeriod(int.Parse(timeArr[0]), int.Parse(timeArr[1]), int.Parse(timeArr[2]), int.Parse(timeArr[3]));
        }
        public TimePeriod(Time t1, Time t2)
        {
            long t1Total = t1.Hour * 1000 * 60 * 60 + t1.Minute * 1000 * 60 + t1.Second * 1000 + t1.Milisecond;
            long t2Total = t2.Hour * 1000 * 60 * 60 + t2.Minute * 1000 * 60 + t2.Second * 1000 + t2.Milisecond;
            long t1t2 = t1Total<t2Total ? t2Total - t1Total : t1Total - t2Total;

            this = new TimePeriod((int)(t1t2 / 1000 / 60 / 60), (int)(t1t2 / 1000 / 60 % 60), (int)(t1t2 / 1000 % 60), (int)(t1t2 % 1000));
        }

        #endregion

        #region ToString

        public override string ToString() => string.Format("{0}:{1:D2}:{2:D2}:{3:D3}", Hour, Minute, Second, Milisecond);

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
            if (Hour == other.Hour && Minute == other.Minute && Second == other.Second && Milisecond == other.Milisecond) return true;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(miliseconds);

        #endregion

        #region IComparable

        public int CompareTo(TimePeriod other)
        {
            if (Hour != other.Hour)
                return (int)(Hour - other.Hour);
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

        public static bool operator ==(TimePeriod t1, TimePeriod t2) => t1.Equals(t2);
        public static bool operator !=(TimePeriod t1, TimePeriod t2) => !t1.Equals(t2);
        public static bool operator <(TimePeriod t1, TimePeriod t2) => t1.CompareTo(t2) < 0;
        public static bool operator >(TimePeriod t1, TimePeriod t2) => t1.CompareTo(t2) > 0;
        public static bool operator <=(TimePeriod t1, TimePeriod t2) => t1.CompareTo(t2) <= 0;
        public static bool operator >=(TimePeriod t1, TimePeriod t2) => t1.CompareTo(t2) >= 0;
        public static TimePeriod operator +(TimePeriod t1, TimePeriod t2)
        {
            long t1Total = t1.Milisecond + (t1.Second * 1000) + (t1.Minute * 1000 * 60) + (t1.Hour * 1000 * 60 * 60);
            long t2Total = t2.Milisecond + (t2.Second * 1000) + (t2.Minute * 1000 * 60) + (t2.Hour * 1000 * 60 * 60);

            t1Total = t1Total + t2Total; //Restrict the range to max 23:59:59
            return new TimePeriod((int)(t1Total / 1000 / 60 / 60), (int)(t1Total / 1000 / 60 % 60), (int)(t1Total / 1000 % 60), (int)(t1Total % 1000));
        }
        public static TimePeriod operator -(TimePeriod t1, TimePeriod t2)
        {
            long t1Total = t1.Milisecond + (t1.Second * 1000) + (t1.Minute * 1000 * 60) + (t1.Hour * 1000 * 60 * 60);
            long t2Total = t2.Milisecond + (t2.Second * 1000) + (t2.Minute * 1000 * 60) + (t2.Hour * 1000 * 60 * 60);

            t1Total = t1Total - t2Total;
            while (t1Total < 0)
                t1Total += 1000 * 24 * 60 * 60;

            return new TimePeriod((int)(t1Total / 1000 / 60 / 60), (int)(t1Total / 1000 / 60 % 60), (int)(t1Total / 1000 % 60), (int)(t1Total % 1000));
        }

        #endregion
    }
}