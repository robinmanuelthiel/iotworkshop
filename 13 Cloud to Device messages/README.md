# Cloud to Device messages
In the previous modules, we learned how to connect devices to the Azure IoT Hub and send messages to the cloud. Let's finally take a look at the opposite way and learn the last important step of our Internet of Things journey: Sending messages form the cloud back to the device.

For this, we should focus on two different parts: The sending of messages to the IoT Hub and the receiving of messages at the device itself.

> **Hint:** If you got stuck during this module or lost the overview on where to place which code, you can always take the look at the [finished and working project](./Code) that is attached to this module.

## Create the sender
Messages that should be send to other devices might come from everywhere. Whether your device wants to talk to another one or a webserver that calculated something wants to notify a device, the process is always the same. To keep things simple, we will create a small console application, that could run anywhere.

### 1. Create a new console application
You can create a new Solution in Visual Studio or add a new project to your existing one. Just make sure, that you select the ***Console Application*** template, which you can find inside the ***Windows*** section.

![Create new Console Application in Visual Studio Screenshot](/Misc/vsnewconsoleapp.png)

### 2. Add NuGet package
The libraries for communicating with the IoT Hub are provided in the [Microsoft Azure IoT Service SDK NuGet package](https://www.nuget.org/packages/Microsoft.Azure.Devices/). Notice, that this in *not* the same package, we added in the previous modules, as this is not for connecting to the IoT Hub as a device but as a different service, that wants to access the hub directly. Once the NuGet package got added, we can start coding.

### 3. Create the logic
The logic for out Cloud to Device messaging demo is quite simple. First, we need to establish a new connection to the IoT Hub. Open the `Program.cs` file of your Console Application and create a new `ServiceClient` by providing the `iothubowner`'s connection string, that you can find in the Azure Portal.  

```csharp
static void Main(string[] args)
{
    // Establish the connection to the IoT Hub
    var client = ServiceClient.CreateFromConnectionString("<IOTHUBOWNER_CONNECTIONSTRING>");
}
```

Now, we can start sending messages to the hub using the `ServiceClient` instance. To simplify this, let's create an infinite loop that waits for any user input and wraps this into a message. This message also needs a recipient, which gets defined by providing the ***Id*** of the device that should receive the message.

```csharp
static void Main(string[] args)
{
    // Establish the connection to the IoT Hub
    var client = ServiceClient.CreateFromConnectionString("<IOTHUBOWNER_CONNECTIONSTRING>");

    while (true)
    {
        // Read command
        Console.Write("Message content: ");
        var content = Console.ReadLine();

        // Send command
        var message = new Message(Encoding.ASCII.GetBytes(content));
        client.SendAsync("<DEVICE_NAME>", message).Wait();
    }
}
```

As you can see, the message gets encoded as an array of bytes again. You can send any Type via this approach, as long as it is converted to bytes. In our demo, we just send the user input. When working in a more complex real-world scenario, I recommend parsing objects to a JSON string and encoding it then, as we saw it in the previous modules, when we sent messages from the device to the hub.

Now the message will arrive at the IoT Hub, but not at the device. So let's implement the second part and enable the device to receive messages that are sent to it via the IoT Hub in the cloud. 

## Create the receiver
To receive messages, we need to implement the process of asking the IoT Hub for new messages and react to them in our code.

### 1. Create a new project of prepare your existing one
You can either create a new UWP project for this, or extend your existing ones, if you have already written some code four your device. Make sure, that the [Microsoft Azure Devices Client NuGet package](https://www.nuget.org/packages/Microsoft.Azure.Devices.Client) has been added to your project and you are still connected to the IoT Hub using the `DeviceClient` class.

```csharp
DeviceClient client = DeviceClient.Create("<IOTHUB_HOSTNAME>", new DeviceAuthenticationWithRegistrySymmetricKey("<DEVICE_ID>", "<DEVICE_KEY>"), TransportType.Amqp);
```

### 2. Check for new messages
To ask the IoT Hub for new messages that are meant for your current device, you can simply call the `client.ReceiveAsync()` method. It automatically returns `null`, when no message could be found. If you received a message, it will be formatted in bytes of course, so you sould decode the message content back to `string`.

```csharp
// Check, if new messages are available
var message = await client.ReceiveAsync();
if (message != null)
{
    // Message received
    var content = Encoding.ASCII.GetString(message.GetBytes());    
}
```

### 3. Confirm message reception
It is very important, that you confirm the message reception to the IoT Hub. Azure, will try to send the massage to you until you did this. So make sure to call the according method, as soon as you have processed the message completely.
```csharp
await client.CompleteAsync(message);
```

### 4. Review the code
For this workshop, we could use this approach to send an LED status update to the device through the IoT Hub. To achieve this, I connected a simple LED to *Pin 25* of my RaspberryPi. You can also use any of your start kits of course. My final code that runs on the device now looks like this:

 ```csharp
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
```

As you can see, I am running the `client.ReceiveAsync();` inside an infinity loop and check, if the IoT Hub has a message for my device, that either contains `"on"` to turn the LED on or anything else to turn it off. Yes, it's that simple!

## Sending and receiving messages
To try it out, we need to start both, the Console Application, that sends the messages to the IoT Hub and the UWP App on the Raspberry Pi. If you have both projects in the same Visual Studio Solution, you need to specify them both as your startup projects. To do so, right click on the solution at the top level of your Solution Explorer in Visual Studio and select ***Properties***. Here you can select multiple startup projects.

![Select multiple startup projects in Visual Studio Screenshot](/Misc/vsmultistart.png)

When you start the solution now, both, the Console Application on your computer and the UWP App on your device will start. Now simply type *on* or *off* at the Console to send a message through the IoT Hub to the device and switch the LED on or off. You can close the Console Application with <kbd>Ctrl</kbd> + <kbd>C</kbd>.

And that's it. Now you can start creating your own cool Internet of Things projects. You have learned, how to setup a device, host a powerful backend in the cloud, manage devices, measure sensor data, send messages to the cloud, process them there and send commands back to the device. Have fun and keep tinkering!
