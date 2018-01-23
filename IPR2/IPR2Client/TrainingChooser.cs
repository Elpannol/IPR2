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
        private bool _isMan;
        private double factor;
        public int maxHeartRate;

        public TrainingChooser(int age, bool isman)
        {
            _age = age;
            checkRate();
        }

        public void checkRate()
        {
            if(_age >= 15 && _age < 25)
            {
                maxHeartRate = 210;
                factor = 1.1;
            }
            else if (_age >= 25 && _age < 35)
            {
                maxHeartRate = 200;
                factor = 1;
            }
            else if (_age >= 35 && _age < 40)
            {
                maxHeartRate = 190;
                factor = 0.87;
            }
            else if (_age >= 40 && _age < 45)
            {
                maxHeartRate = 180;
                factor = 0.83;
            }
            else if (_age >= 45 && _age < 50)
            {
                maxHeartRate = 170;
                factor = 0.78;
            }
            else if (_age >= 50 && _age < 55)
            {
                maxHeartRate = 160;
                factor = 0.75;
            }
            else if (_age >= 55 && _age < 60)
            {
                maxHeartRate = 150;
                factor = 0.71;
            }
            else if (_age >= 60 && _age < 65)
            {
                maxHeartRate = 150;
                factor = 0.68;
            }
            else if (_age >= 65)
            {
                maxHeartRate = 150;
                factor = 0.65;
            }
        }

        public double CalculateVo2(List<int> heartRates)
        {
            double vo2 = 0;
            int averageHeartRate = 0;
            foreach(int i in heartRates)
            {
                averageHeartRate += i;
            }
            averageHeartRate = averageHeartRate / heartRates.Count;

            if(averageHeartRate < 130)
            {
                return -1;
            }

            if (_isMan)
            {

            }
            else
            {

            }
            return vo2;
        }
    }
}
