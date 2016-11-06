# Sending data to the cloud
Now that we have our code on our device running and it is registered at out cloud service, it is getting time to send your first set of data to the cloud.

If you skipped the previous modules, make sure, that you grab the [code of Module 07](/07%20RGetting%20Rtemperature%20Rvalues/Code) and deploy it to your device like shown in [Module 05](/05%20Setting%20up).

So let's get started and connect our little application with the Azure IoT Hub to send some messages.

> **Hint:** In this module, we will write code again. As last time, you can find the [finished and working code](./Code) attached to this module.

## 1. Connect to the Azure IoT Hub
For connecting with the IoT Hub, we can use a [NuGet package](https://www.nuget.org/packages/Microsoft.Azure.Devices.Client/) again. We remember: To add a NuGet package, right click on the ***References*** folder of your project in Visual Studio and select ***Manage NuGet Packages...*** to open the NuGet package manager. Head over to the ***Browse*** tab, search for "Microsoft Azure Devices Client" and install the according package.

Navigate to the `MainPage.xaml.cs` file again and add a variable for the IoT Hub connection.

```csharp
private DeviceClient deviceClient;
```

Next, let's ininitalize the connection in the constructor of the page right below the inistanciation of the temperature providers.

```csharp
deviceClient = DeviceClient.Create("<IOTHUB_HOSTNAME>", new DeviceAuthenticationWithRegistrySymmetricKey("<DEVICE_ID>", "<DEVICE_KEY>"));
```

The strings `"<IOTHUB_HOSTNAME>"`, `"<DEVICE_ID>"` and `"<PRIMARY_KEY>"` have to be replaced with your values, of course. You can find all of these values in the [Azure Portal](https://portal.azure.com) at your IoT Hub. Use the name and security key of the device that you have created and make sure, that you **only** provide the Hostname (for example "MyDemoIotHub") and not the full URL as `"<IOTHUB_HOSTNAME>"`.

Now the app is connected to your IoT Hub and can send data in the name of the device you have declared.

## 2. Send messages
Let's finally take a look at how to send messages! First, we should first define how our message should look like. For this, we can create an anonymous type with all the data we want to send.

It might be a good idea to add the Id of the sending device as well as a current timestamp of the measurement together with the temperature so we should create a telemetry point like this after every measurement in the `Timer_Tick` method.

```csharp
var telemetryDataPoint = new
{
    id = "<DEVICE_ID>",
    temperature = temp,
    date = DateTime.Now
};
```

The most compatible way to transfer objects through the borders of different technologies is JSON so let's convert our `telemetryDataPoint` into a JSON object. The beautiful JSON.NET NuGet package does that for us that gets automatically installed, when adding the *Microsoft Azure Devices Client* package to our project.

```csharp
var json = JsonConvert.SerializeObject(telemetryDataPoint);
```

The format for sending data to an Azure IoT Hub is Microsoft's `Message` class, so in the last step we need to transform the JSON into bytes and wrap it with a `Message`. Luckily, this can be done with a single line of code.

```csharp
var message = new Message(Encoding.ASCII.GetBytes(json));
```

Now we can finally send the message to the IoT Hub!

```csharp
await deviceClient.SendEventAsync(message);
```

The whole `Timer_Tick` method should now look like this:

```csharp
private async void Timer_Tick(object sender, object e)
{
    // Take measurement
    var temp = temperatureProvider.GetTemperature();
    Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");

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
```

## 3. Tracking
Of course, we want to know if everything worked as expected and if the messages really reached the IoT Hub. The easiest way to track this is the [Device Explorer](https://github.com/Azure/azure-iot-sdks/tree/master/tools/DeviceExplorer)  tool, that we might now from the previous module.

Start the ***Device Explorer***, enter your `iothubowner`'s connection string, hit the ***Update*** button and swtich to the ***Data*** tab. Here you can select a device you want to track and when activating the monitoring through the ***Monitor*** button, you should see the raw data of your temperature messages coming in every fife seconds (or whatever timespan you selected).

![Device Explorer tracking the raw messages](/Misc/deviceexplorermonitoring.png)
