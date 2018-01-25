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
        public int maxHeartRate = 210;

        public TrainingChooser(int age, bool isman)
        {
            _age = age;
            _isMan = isman;
            checkRate();
        }

        public void checkRate()
        {
            if (_age >= 25 && _age < 35)
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

        public double CalculateVo2(List<int> heartRates, int wattage)
        {
            double vo2 = 0;
            int averageHeartRate = getHeartRate(heartRates);

            if(averageHeartRate < 130)
            {
                return -1;
            }

            if (_isMan)
            {
                vo2 = ((174.2 * wattage + 4020) / (103.2 * averageHeartRate - 6299));
            }
            else
            {
                vo2 = ((163.8 * wattage + 3780) / (104.4 * averageHeartRate - 7514));
            }

            if(_age > 30)
            {
                vo2 = vo2 * factor;
            }
            return vo2;
        }

        private int getHeartRate(List<int> heartRates)
        {
            int lowestHeartRate = maxHeartRate;
            int highestHeartRate = 0;
            for(int i = 2; i < heartRates.Count; i++)
            {
                if(heartRates[i] < lowestHeartRate)
                {
                    lowestHeartRate = heartRates[i];
                }
                if(heartRates[i] > highestHeartRate)
                {
                    highestHeartRate = heartRates[i];
                }
            }
            if(highestHeartRate - lowestHeartRate > 5)
            {
                return (heartRates[0] + heartRates[1]) / 2;
            }
            return (highestHeartRate + lowestHeartRate) / 2;

        }

        public void getSpoofData(string name) {
            double vo2 = ((174.2 * 150 + 4020) / (103.2 * 135 - 6299));
            Login.Handler.SendVo2(name, vo2);
        }
    }
}
