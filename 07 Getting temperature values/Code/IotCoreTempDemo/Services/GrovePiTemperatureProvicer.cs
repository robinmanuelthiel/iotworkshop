using GrovePi;
using GrovePi.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreTempDemo.UWP.Services
{
    public class GrovePiTemperatureProvicer : ITemperatureProvider
    {
        private Pin pin;
        private ITemperatureAndHumiditySensor tempHumid;

        public GrovePiTemperatureProvicer(Pin pin)
        {
            tempHumid = DeviceFactory.Build.TemperatureAndHumiditySensor(pin, Model.Dht11);
        }

        public async Task InitializeAsync() {}

        public double GetTemperature()
        {
            return tempHumid.TemperatureInCelsius();
        }
    }
}
