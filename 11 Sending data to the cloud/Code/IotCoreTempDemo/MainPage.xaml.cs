using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GHIElectronics.UWP.Shields;
using System.Diagnostics;
using IotCoreTempDemo.UWP.Services;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IotCoreTempDemo.UWP
{
    public sealed partial class MainPage : Page
    {
        private ITemperatureProvider temperatureProvider;
        private DispatcherTimer timer;
        private DeviceClient deviceClient;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            // Chose a temperature provider
            //temperatureProvider = new FezHatTemperatureProvider(); // GHI FEZ HAT            
            //temperatureProvider = new FezCreamTemperatureProvider(4); // GHI FEZ Cream with TempHumidSI70 socket         
            temperatureProvider = new RandomTemperatureProvider(); // Use the random provider, if no temperature sensor is connected

            deviceClient = DeviceClient.Create("<IOTHUB_HOSTNAME>", new DeviceAuthenticationWithRegistrySymmetricKey("<DEVICE_ID>", "<PRIMARY_KEY>"));
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the provider
            await temperatureProvider.InitializeAsync();

            // Create timer that fires the Timer_Tick method every 5 seconds
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            // Take measurement
            var temp = temperatureProvider.GetTemperature();
            Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");

            // Update UI
            TemperatureText.Text = $"Temperature: {temp} \u00B0C";

            // Create a datapoint
            var telemetryDataPoint = new
            {
                id = "<DEVICE_ID>",
                temperature = temp,
                date = DateTime.Now
            };

            // Format data to a JSON message
            var json = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(json));

            // Send message to the IoT Hub
            await deviceClient.SendEventAsync(message);
        }
    }
}
