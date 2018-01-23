using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client.Simulation
{
    public class Training
    {
        public List<Measurement> _measurements { get; set; }
        public readonly string _name;

        public double vo2;

        public Training(List<Measurement> measurements, string name)
        {
            _measurements = measurements;
            _name = name;
        }

        public Measurement getMeasurement(int index)
        {
            if(_measurements.Count == 0)
            {
                return null;
            }
            return _measurements[index];
        }

        public int getLength()
        {
            return _measurements.Count-1;
        }
    }
}
