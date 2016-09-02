using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GHIElectronics.UWP.Shields;
using System.Diagnostics;

namespace FezHatTemp
{
    public sealed partial class MainPage : Page
    {
        private FEZHAT fezHat;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize FEZ Hat
            fezHat = await FEZHAT.CreateAsync();

            // Create timer that fires the Timer_Tick method every 5 seconds
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            // Take measurement
            var temp = fezHat.GetTemperature();
            Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");

            // Update UI
            TemperatureText.Text = $"Temperature: {temp} \u00B0C";
        }
    }
}
