using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreTempDemo.UWP.Services
{
    interface ITemperatureProvider
    {
        double GetTemperature();
        Task InitializeAsync();
    }
}
