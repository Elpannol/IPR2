using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client
{
    public class TrainingChooser
    {
        private int _age;
        public int maxHeartRate;
        public TrainingChooser(int age)
        {
            _age = age;
            checkRate();
        }

        public void checkRate()
        {
            if(_age >= 15 && _age < 25)
            {
                maxHeartRate = 210;
            }
            else if (_age >= 25 && _age < 35)
            {
                maxHeartRate = 200;
            }
            else if (_age >= 35 && _age < 40)
            {
                maxHeartRate = 190;
            }
            else if (_age >= 40 && _age < 45)
            {
                maxHeartRate = 180;
            }
            else if (_age >= 45 && _age < 50)
            {
                maxHeartRate = 170;
            }
            else if (_age >= 50 && _age < 55)
            {
                maxHeartRate = 160;
            }
            else if (_age >= 55)
            {
                maxHeartRate = 150;
            }
        }
    }
}
