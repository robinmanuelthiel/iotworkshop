using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GHIElectronics.UWP.Shields;
using System.Diagnostics;
using IotCoreTempDemo.UWP.Services;

namespace IotCoreTempDemo.UWP
{
    public sealed partial class MainPage : Page
    {
        private ITemperatureProvider temperatureProvider;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            // Chose a temperature provider
            //temperatureProvider = new FezHatTemperatureProvider(); // GHI FEZ HAT            
            //temperatureProvider = new FezCreamTemperatureProvider(4); // GHI FEZ Cream with TempHumidSI70 socket         
            temperatureProvider = new RandomTemperatureProvider(); // Use the random provider, if no temperature sensor is connected
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

        private void Timer_Tick(object sender, object e)
        {
            // Take measurement
            var temp = temperatureProvider.GetTemperature();
            Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");

            // Update UI
            TemperatureText.Text = $"Temperature: {temp} \u00B0C";
        }
    }
}
