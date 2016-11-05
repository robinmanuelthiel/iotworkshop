using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreTempDemo.UWP.Services
{
    public class RandomTemperatureProvider : ITemperatureProvider
    {
        Random random;

        public RandomTemperatureProvider()
        {
            random = new Random();
        }

        public async Task InitializeAsync() {}

        public double GetTemperature()
        {
            return random.Next(10, 40) + random.NextDouble();
        }        
    }
}
