using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client.Simulation
{
    public class Measurement : IComparable<Measurement>
    {
        public int Weerstand { get; set; }
        public int Hartslag { get; set; }
        public int Rondes { get; set; }
        public SimpleTime Time { get; set; }

        public Measurement(int weerstand, int hartslag, int rondes, int time1, int time2)
        {
            Weerstand = weerstand;
            Hartslag = hartslag;
            Rondes = rondes;
            Time = new SimpleTime(time1, time2);
        }

        public Measurement(int weerstand, int hartslag, int rondes, SimpleTime time)
        {
            Weerstand = weerstand;
            Hartslag = hartslag;
            Rondes = rondes;
            Time = time;
        }

        public int CompareTo(Measurement other)
        {
            if (Time == other.Time)
                return 0;
            if (Time > other.Time)
                return 1;
            if (Time < other.Time)
                return -1;
            return 0;
        }

        private dynamic GetMessageToSend()
        {
            dynamic toSend = new
            {
                id = "add/measurement",
                data = new
                {
                    weerstand = Weerstand,
                    hartslag = Hartslag,
                    rondes = Rondes,
                    time = Time
                }
            };
            return toSend;
        }

        public override string ToString()
        {
            return $"weerstand: {Weerstand} Watt, hartslag: {Hartslag} BPM, rondes: {Rondes} RPM, tijd: {Time.ToString()} Min";
        }
    }

    public struct SimpleTime
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SimpleTime && Equals((SimpleTime)obj);
        }

        public bool Equals(SimpleTime other)
        {
            return (Minutes == other.Minutes) && (Seconds == other.Seconds);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Minutes * 397) ^ Seconds;
            }
        }

        public readonly int Minutes;
        public readonly int Seconds;

        public SimpleTime(int min, int sec)
        {
            Minutes = min;
            Seconds = sec;
        }

        public override string ToString()
        {
            return $"{Minutes:00}:{Seconds:00}";
        }

        public static bool operator >(SimpleTime first, SimpleTime second)
        {
            if (first.Minutes > second.Minutes)
                return true;
            if (first.Minutes == second.Minutes)
                return first.Seconds > second.Seconds ? true : false;
            return false;
        }

        public static bool operator <(SimpleTime first, SimpleTime second)
        {
            if (first.Minutes < second.Minutes)
                return true;
            if (first.Minutes == second.Minutes)
                return first.Seconds < second.Seconds ? true : false;
            return false;
        }

        public static bool operator ==(SimpleTime first, SimpleTime second)
        {
            return (first.Minutes == second.Minutes) && (first.Seconds == second.Seconds);
        }

        public static bool operator !=(SimpleTime first, SimpleTime second)
        {
            return (first.Minutes != second.Minutes) || (first.Seconds != second.Seconds);
        }
    }
}
