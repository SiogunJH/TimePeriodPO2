using System.Data;
using TimeExtendedLib;
using TimePeriodExtendedLib;

namespace TimeExtendedUnitTests
{
    [TestClass]
    public class TimeExtendedUnitTests
    {

        #region Constructor Tests


        [DataTestMethod]
        [DataRow(13, 23, 34, 333)]
        [DataRow(15, 59, 3, 123)]
        [DataRow(3, 0, 0, 121)]
        [DataRow(0, 0, 0, 1)]
        public void Constructor_4Arguments_Int_OK(int h, int m, int s, int ms)
        {
            var t = new Time(h, m, s, ms);
            Assert.AreEqual(h, t.Hour);
            Assert.AreEqual(m, t.Minute);
            Assert.AreEqual(s, t.Second);
            Assert.AreEqual(ms, t.Milisecond);
        }

        [DataTestMethod]
        [DataRow(13, 23, 34)]
        [DataRow(15, 59, 3)]
        [DataRow(3, 0, 0)]
        [DataRow(0, 0, 1)]
        public void Constructor_3Arguments_Int_OK(int h, int m, int s)
        {
            var t = new Time(h, m, s);
            Assert.AreEqual(h, t.Hour);
            Assert.AreEqual(m, t.Minute);
            Assert.AreEqual(s, t.Second);
            Assert.AreEqual(0, t.Milisecond);
        }

        [DataTestMethod]
        [DataRow(13, 23)]
        [DataRow(15, 59)]
        [DataRow(3, 0)]
        [DataRow(0, 1)]
        public void Constructor_2Arguments_Int_OK(int h, int m)
        {
            var t = new Time(h, m);
            Assert.AreEqual(h, t.Hour);
            Assert.AreEqual(m, t.Minute);
            Assert.AreEqual(0, t.Second);
            Assert.AreEqual(0, t.Milisecond);
        }

        [DataTestMethod]
        [DataRow(13)]
        [DataRow(15)]
        [DataRow(3)]
        [DataRow(0)]
        public void Constructor_1Arguments_Int_OK(int h)
        {
            var t = new Time(h);
            Assert.AreEqual(h, t.Hour);
            Assert.AreEqual(0, t.Minute);
            Assert.AreEqual(0, t.Second);
            Assert.AreEqual(0, t.Milisecond);
        }

        [DataTestMethod]
        [DataRow("01:02:03:004", 1, 2, 3, 4)]
        [DataRow("03:04:05:066", 3, 4, 5, 66)]
        [DataRow("00:00:00:000", 0, 0, 0, 0)]
        [DataRow("22:33:44:000", 22, 33, 44, 0)]
        public void Constructor_StringArgument_OK(string str, int h, int m, int s, int ms)
        {
            var t = new Time(str);
            Assert.AreEqual(h, t.Hour);
            Assert.AreEqual(m, t.Minute);
            Assert.AreEqual(s, t.Second);
            Assert.AreEqual(ms, t.Milisecond);
        }

        [TestMethod]
        public void Constructor_DefaultArguments()
        {
            var t = new Time();
            Assert.AreEqual(0, t.Hour);
            Assert.AreEqual(0, t.Minute);
            Assert.AreEqual(0, t.Second);
            Assert.AreEqual(0, t.Milisecond);
        }

        [DataTestMethod]
        [DataRow(24, 30, 0, 0)]
        [DataRow(12, 70, 0, 0)]
        [DataRow(12, 30, 110, 0)]
        [DataRow(12, 30, 2, 11110)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_ArguemntOutOfRangeException(int h, int m, int s, int ms)
        {
            new Time(h, m, s, ms);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ArguemntNullException()
        {
            new Time(null, null, null, null);
        }

        #endregion

        #region ToString Tests

        [DataTestMethod]
        [DataRow("12:23:34:123", 12, 23, 34, 123)]
        [DataRow("11:28:02:000", 11, 28, 2, 0)]
        [DataRow("10:25:00:121", 10, 25, 0, 121)]
        public void ToString(string str, int h, int m, int s, int ms)
        {
            var t = new Time(h, m, s, ms);
            Assert.AreEqual(str, t.ToString());
        }

        #endregion

        #region IEquatable Tests

        [DataTestMethod]
        [DataRow(true, 1, 2, 3, 123, 1, 2, 3, 123)]
        [DataRow(false, 1, 2, 30, 1, 1, 22, 3, 1)]
        [DataRow(false, 1, 2, 3, 1, 1, 2, 23, 23)]
        [DataRow(true, 10, 20, 30, 40, 10, 20, 30, 40)]

        public void IEquatable(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1.Equals(t2));
        }

        #endregion

        #region IComparable Tests

        [DataTestMethod]
        [DataRow('=', 1, 2, 3, 4, 1, 2, 3, 4)]
        [DataRow('>', 10, 2, 3, 0, 1, 2, 30, 100)]
        [DataRow('<', 1, 2, 3, 100, 1, 20, 3, 1)]
        public void IComparable(char result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            switch (result)
            {
                case '=':
                    Assert.AreEqual(true, t1.CompareTo(t2)==0);
                    break;
                case '<':
                    Assert.AreEqual(true, t1.CompareTo(t2) < 0);
                    break;
                case '>':
                    Assert.AreEqual(true, t1.CompareTo(t2) > 0);
                    break;
            }
        }

        #endregion

        #region Operators Tests

        [DataTestMethod]
        [DataRow(true, 10, 22, 35, 234, 10, 22, 35, 234)]
        [DataRow(false, 1, 22, 35, 12, 10, 22, 35, 12)]
        [DataRow(false, 10, 22, 35, 123, 10, 2, 35, 123)]
        public void Operator_Equals(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1==t2);
        }

        [DataTestMethod]
        [DataRow(false, 10, 22, 35, 200, 10, 22, 35, 200)]
        [DataRow(true, 1, 22, 35, 123, 10, 22, 35, 123)]
        [DataRow(true, 10, 22, 35, 1, 10, 2, 35, 1)]
        public void Operator_NotEquals(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1 != t2);
        }

        [DataTestMethod]
        [DataRow(false, 10, 22, 35, 1, 10, 22, 35, 1)]
        [DataRow(true, 1, 22, 35, 1, 10, 22, 35, 3)]
        [DataRow(false, 10, 22, 35, 4, 10, 2, 35, 7)]
        public void Operator_Lesser(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1 < t2);
        }

        [DataTestMethod]
        [DataRow(false, 10, 22, 35, 10, 10, 22, 35, 10)]
        [DataRow(false, 1, 22, 35, 222, 10, 22, 35, 234)]
        [DataRow(true, 10, 22, 35, 100, 10, 2, 35, 200)]
        public void Operator_Greater(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1 > t2);
        }

        [DataTestMethod]
        [DataRow(true, 10, 22, 35, 333, 10, 22, 35, 333)]
        [DataRow(true, 1, 22, 35, 123, 10, 22, 35, 234)]
        [DataRow(false, 10, 22, 35, 344, 10, 2, 35, 23)]
        public void Operator_LesserOrEqual(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1 <= t2);
        }

        [DataTestMethod]
        [DataRow(true, 10, 22, 35, 555, 10, 22, 35, 555)]
        [DataRow(false, 1, 22, 35, 666, 10, 22, 35, 777)]
        [DataRow(true, 10, 22, 35, 888, 10, 2, 35, 888)]
        public void Operator_GreaterOrEqual(bool result, int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var t2 = new Time(h2, m2, s2, ms2);
            Assert.AreEqual(result, t1 >= t2);
        }

        [DataTestMethod]
        [DataRow(10, 10, 10, 10, 2, 2, 2, 2, 12, 12, 12, 12)]
        [DataRow(23, 59, 59, 999, 0, 0, 0, 1, 0, 0, 0, 0)]
        [DataRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 0, 500, 0, 0, 0, 600, 0, 0, 1, 100)]
        public void Operator_Add(int h1, int m1, int s1, int ms1, int h2, int m2, int s2, int ms2, int h3, int m3, int s3, int ms3)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var tp = new TimePeriod(h2, m2, s2, ms2);
            var t2 = new Time(h3, m3, s3, ms3);
            Assert.AreEqual(t1+tp, t2);
        }

        [DataTestMethod]
        [DataRow(10, 10, 10, 10, 2, 2, 2, 2, 12, 12, 12, 12)]
        [DataRow(23, 59, 59, 999, 0, 0, 0, 1, 0, 0, 0, 0)]
        [DataRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 0, 500, 0, 0, 0, 600, 0, 0, 1, 100)]
        public void Operator_Subtract(int h3, int m3, int s3, int ms3, int h2, int m2, int s2, int ms2, int h1, int m1, int s1, int ms1)
        {
            var t1 = new Time(h1, m1, s1, ms1);
            var tp = new TimePeriod(h2, m2, s2, ms2);
            var t2 = new Time(h3, m3, s3, ms3);
            Assert.AreEqual(t1 - tp, t2);
        }

        #endregion
    }
}