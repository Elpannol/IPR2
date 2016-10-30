using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class Training
    {
        public readonly List<Measurement> _measurements;
        public readonly string _name;

        public Training(string name)
        {
            _measurements = new List<Measurement>();
            _name = name;
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
