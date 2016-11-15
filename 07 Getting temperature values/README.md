# Getting temperature values
You are ready to create your first real world application. In this module we will use the FET Hat to read temperature values from your Raspberry Pi to be ready to process and send them to the cloud in the next module.

> **Hint:** If you got stuck during this module or lost the overview on where to place which code, you can always take the look at the [finished and working project](./Code) that is attached to this module.

## 1. Create the project
Again, we need a new UWP application first. As learned in [05 Setting up](05%20Setting%20up#5-deploy-your-first-app), open Visual Studio 2015 and select ***File*** -> ***New*** -> ***Project...*** to open the new project dialog. Navigate through the installed project templates and find the ***Blank App (Universal Windows)*** at the ***Visual C#*** -> ***Windows*** -> ***Universal*** folder. After naming your project, click on ***OK*** to get started with the empty project.

![Visual Studio 2015 new project dialog](../Misc/vsnewproject.png)

## 2. Add necessary 3rd party libraries
As most shields like  the [GHI FEZ Hat](https://www.ghielectronics.com/catalog/product/500) are from a third-party vendors, its functionality and bits are not included into Windows 10 IoT Core by default, of course. Fortunately, most manufacturers provide .NET libraries for their sensors. Most of them are available via the NuGet package management system that comes with Visual Studio and is very popular in the .NET community.

To add this library to your project via NuGet, right click on the ***References*** folder of your project in Visual Studio and select ***Manage NuGet Packages...*** to open the NuGet package manager for your project.

Head over to the ***Browse*** tab and search for your manufacturer. When the package appears in the results list, select it and hit the ***Install*** button on the right to add it to the project.

> **Hint:** If you use one of the temperature sensors that have been [recommended for this workshop](/README.md), these are their NuGet packages: [FEZ Hat](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/), [FEZ Cream](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/), [GrovePi](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/).

![Visual Studio 2015 add NuGet package](../Misc/vsaddnuget.png)

## 3. Initialize the temperature sensors
Now that all the needed bits are available, we can start coding. As there are hundrets of different temperature sensors on the market, this code can differ a bit depending on which sensor you use. In this workshop, the  recommended ones will be covered.

To isolate the sensor code, we should create a simple class for reading temperature values. As we have to consider different sensors here, I created a small and simple interface, that all sensor reading classes have to implement. You don't have to do that if you only work with one sensor but it's a nice coding style. So simply create a new interface in you project that looks like this:

```csharp
interface ITemperatureProvider
{
    Task InitializeAsync();
    double GetTemperature();    
}
```

Now we can start writing our class that implements this interface. Give it a name that represents your sensor like `FezHatTemperatureProvider` for example and let it implement the interface.

```csharp
public class MyTemperatureProvider : ITemperatureProvider
{
    public async Task InitializeAsync()
    {
        
    }

    public double GetTemperature()
    {
        
    }        
}
```
What follows now, strongly depends on your sensor. We will walk through the implementations for the recommended ones. If you don't have a sensor, you can also find a "sumulated random sensor" in the [finished code](./Code), that you can use.

### GHI FEZ Hat
First, install the [FEZ Hat NuGet package](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/). When using the GHI FEZ Hat shield, it needs to be initialized before using it. That's why we add a `FEZHAT` member to the class and initiliaze and read temperature values as follows:

```csharp
using GHIElectronics.UWP.Shields;

public class FezHatTemperatureProvider : ITemperatureProvider
{
    private FEZHAT fezHat;

    public async Task InitializeAsync()
    {
        fezHat = await FEZHAT.CreateAsync();
    }

    public double GetTemperature()
    {
        returns fezHat.GetTemperature();
    }        
}
```

### GHI FEZ Cream
First, install the [FEZ Cream NuGet package](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/). The FEZ Cream is very simiar to the FEZ Hat. The only difference is, that you can choose, to wich socket you want to connect your temperature sensor to. That is why we add a constructor to the class that takes the socket number. This will be saved in a class member beside the `FEZCream` mainboard itself and the `TempHumidSI70` sensor. Both have to be initialized (the sensor with socket number) and then can be used as follows:

```csharp
using GHIElectronics.UWP.Gadgeteer.Mainboards;
using GHIElectronics.UWP.Gadgeteer.Modules;
using GHIElectronics.UWP.GadgeteerCore;

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
```

### GrovePi
First, install the [GrovePi NuGet package](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/). GrovePi's approach is similar to the FEZ Cream. Just instanciate the sensor using the `DeviceFactory` and provide the analog port, you connected the sensor to. Once initialized, the temperature can be measured by calling the `TemperatureInCelsius()` method. 

```csharp
using GrovePi;
using GrovePi.Sensors;

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
``

## 4. Read temperature values
Now that we have implented the communication with the sensor, we just need to instanciate it. Navigate to the `MainPage.xaml.cs` file as this is the common entry point for most UWP applications and create a variable for the temperature provider.

```csharp
private ITemperatureProvider temperatureProvider;
```

In the constructor we can now instanciate the provider and initialize it soon as the `MainPage` got loaded completely. For this, we need to subscribe to the `MainPage`'s `Loaded` event and initialize the provider there.

```csharp
public sealed partial class MainPage : Page
{
    private ITemperatureProvider temperatureProvider;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += MainPage_Loaded;
        temperatureProvider = new MyTemperatureProvider(); 
    }

    private async void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        wait temperatureProvider.InitializeAsync();
    }
}
```

Once the sensor is initialized, we can start working with it. First, we need to define when we want to take a measurement, so let's create a timer that triggers a temperature measurement function every 5 seconds. Create a `DispatcherTimer` variable to store the timer.
```csharp
private DispatcherTimer timer;
````

Now add the timer definition just below the provider initialization inside the `MainPage_Loaded` method.
```csharp
timer = new DispatcherTimer();
timer.Interval = TimeSpan.FromSeconds(5);
timer.Tick += Timer_Tick;
timer.Start();
````
This defines a timer, that fires the `Timer_Tick` method every five seconds from now. Make sure that this function exist by adding it below the `MainPage_Loaded` mehtod.
```csharp
private void Timer_Tick(object sender, object e)
{
    // Take measurement
}
```
Inside this method we can take the temperature measurement by simply calling the `ITemperatureProvider`'s `GetTemperature()` method. It returns the provider's current temperature. Let's store this in a local variable within the `Timer_Tick` method for later. To check if everything works as expected, we should print out the temperature together with a current timestamp in the ***Output*** window. Consider that you need to add another using statement to work with the `Debug` class.
```csharp
using System.Diagnostics;
```
```csharp
private void Timer_Tick(object sender, object e)
{
    // Take measurement
    var temp = temperatureProvider.GetTemperature();
    Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");
}
```
> **Hint:** We use `\u00B0` here to escape the degree symbol (°).

Now you can deploy the code on your Raspberry Pi and run it as [learned in Module 05](../05%20Setting%20up#run-the--app-on-your-device). As soon as the application is running, you should see a new time and temperature pair inside the ***Output*** window every five seconds.

![Visual Studio 2015 Debug console output](../Misc/vstempdebugconsole.png)

> **Hint:** Remember that you can check out the attached [finished project code](./Code) at any time you got stuck or need help on how your code should look like.

## 5. Update the UI (optional)
If your Raspberry Pi is connected to an external display via HDMI, you could also update the UI to show the current temperature value on the screen. For this, open the `MainPage.xaml` file and add the following line to create a centered text field:
```xaml
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <TextBlock x:Name="TemperatureText" HorizontalAlignment="Center" VerticalAlignment="Center" />
</Grid>
```
Using its name `TemperatureText` we can update the text everytime we take a measurement.
```csharp
private void Timer_Tick(object sender, object e)
{
    // Take measurement
    var temp = temperatureProvider.GetTemperature();
    Debug.WriteLine($"Time: {DateTime.Now}, Temperature: {temp} \u00B0C");
    
    // Update UI
    TemperatureText.Text = $"Temperature: {temp} \u00B0C";
}
```

When you run your code again and got your device attached to an external screen you should also see the temperature updates there.
