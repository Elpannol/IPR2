using Newtonsoft.Json;

namespace IPR2
{
    public class Measurement
    {
        public int Weerstand { get; set; }
        public int Hartslag { get; set; }
        public int Rondes { get; set; }
        [JsonProperty("Time")]
        public SimpleTime Time { get; set; }

        public Measurement(int weerstand, int hartslag, int rondes, int time1, int time2)
        {
            Weerstand = weerstand;
            Hartslag = hartslag;
            Rondes = rondes;
            Time = new SimpleTime(time1, time2);
        }

        [JsonConstructor]
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

        public override string ToString()
        {
            return $"weerstand: {Weerstand} Watt, hartslag: {Hartslag} BPM, rondes: {Rondes} RPM, tijd: {Time.ToString()} Min";
        }

        public string ToRawData()
        {
            return $"{Weerstand} {Hartslag} {Rondes} {Time.ToString()}";
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

        public int Minutes { get; set; }
        public int Seconds { get; set; }

        [JsonConstructor]
        public SimpleTime(int minutes, int seconds)
        {
            Minutes = minutes;
            Seconds = seconds;
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
