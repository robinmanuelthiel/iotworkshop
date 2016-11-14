using Microsoft.Azure.Devices.Client;
using System;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;

namespace CloudToDeviceDemo.Receiver
{
    public sealed partial class MainPage : Page
    {
        private DeviceClient client;
        private GpioController gpio;
        private GpioPin pin;

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize GPIO
            gpio = GpioController.GetDefault();
            pin = gpio.OpenPin(26);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            // Initialize device client
            client = DeviceClient.Create("<IOTHUB_HOSTNAME>", new DeviceAuthenticationWithRegistrySymmetricKey("<DEVICE_ID>", "<DEVICE_KEY>"), TransportType.Amqp);

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                // Check, if new messages are available
                var message = await client.ReceiveAsync();
                if (message != null)
                {
                    // Message received
                    var content = Encoding.ASCII.GetString(message.GetBytes());
                    if (content.Equals("on"))
                    {
                        // Switch LED on
                        pin.Write(GpioPinValue.High);
                    }

                    else
                    {
                        // Switch LED off
                        pin.Write(GpioPinValue.Low);
                    }

                    // Confirm message processing
                    await client.CompleteAsync(message);
                }
            }
        }
    }
}
