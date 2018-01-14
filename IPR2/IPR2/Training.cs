using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPR2Client;

namespace IPR2
{
    public class Training
    {
        public readonly List<Measurement> _measurements;
        public readonly string _name;
        private TrainingState _state;

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

        public void startTraining()
        {
            _state = TrainingState.START;
        }

        public void stopTraining()
        {
            _state = TrainingState.STOP;
        }

        public void checkTraining()
        {
            switch (_state)
            {
                case TrainingState.START:
                    break;
                case TrainingState.WARMINGUP:
                    break;
                case TrainingState.REALTEST:
                    break;
                case TrainingState.COOLINGDOWN:
                    break;
                case TrainingState.STOP:
                    break;
                default:
                    break;
            }
        }
    }
}
