using GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreTempDemo.UWP.Services
{
    public class FezHatTemperatureProvider : ITemperatureProvider
    {
        private FEZHAT fezHat;

        public async Task InitializeAsync()
        {
            fezHat = await FEZHAT.CreateAsync();
        }

        public double GetTemperature()
        {
            return fezHat.GetTemperature();
        }        
    }
}
