using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client.Simulation
{
    public class Training
    {
        public readonly List<Measurement> _measurements;
        public readonly string _name;

        public Training(List<Measurement> measurements, string name)
        {
            _measurements = measurements;
            _name = name;
        }

        public Measurement getMeasurement(int index)
        {
            return _measurements[index];
        }

        public int getLength()
        {
            return _measurements.Count-1;
        }
    }
}
