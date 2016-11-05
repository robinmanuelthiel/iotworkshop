using GHIElectronics.UWP.Gadgeteer.Mainboards;
using GHIElectronics.UWP.Gadgeteer.Modules;
using GHIElectronics.UWP.GadgeteerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreTempDemo.UWP.Services
{
    public class FezCreamTemperatureProvider : ITemperatureProvider
    {
        private FEZCream mainboard;
        private TempHumidSI70 tempHumid;
        private int tempHumidSocket;

        public FezCreamTemperatureProvider(int tempHumidSocket)
        {
            this.tempHumidSocket = tempHumidSocket;
        }

        public async Task InitializeAsync()
        {
            mainboard = await Module.CreateAsync<FEZCream>();
            tempHumid = await Module.CreateAsync<TempHumidSI70>(mainboard.GetProvidedSocket(tempHumidSocket));
        }

        public double GetTemperature()
        {
            var measurement = tempHumid.TakeMeasurement();
            return measurement.Temperature;
        }
    }
}
