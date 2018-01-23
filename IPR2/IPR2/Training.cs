using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPR2Client;
using Newtonsoft.Json;

namespace IPR2
{
    public class Training
    {
        [JsonProperty("_measurements")]
        public List<Measurement> _measurements { get; set; }
        public string _name { get; set; }
        public double _vo2 { get; set; } = -1;

        public Training(string name)
        {
            _measurements = new List<Measurement>();
            _name = name;
        }

        [JsonConstructor]
        public Training(List<Measurement> measurements, string name, double vo2)
        {
            _measurements = measurements;
            _name = name;
            _vo2 = vo2;
        }

        public void AddMeasurement(Measurement mes)
        {
            _measurements.Add(mes);
        }

        public Measurement getMeasurement(int index)
        {
            return _measurements[index];
        }

        public int getLength()
        {
            return _measurements.Count - 1;
        }
    }
}
