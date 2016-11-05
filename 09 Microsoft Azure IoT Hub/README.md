# Microsoft Azure IoT Hub
Coming soon...

## 1. Create a new IoT Hub in the Azure Portal
First, we need to create a new IoT Hub to work with. Navigate to the [Azure Portal](https://portal.azure.com), login and create a new IoT Hub by clicking ***New*** -> ***Internet of Things*** -> ***IoT Hub***.

![Create an Azure IoT Hub](/Misc/azureconfigureiothub.png)

Follow the wizard to configure the IoT Hub.

1. **Name:** Choose whatever you like here. The name must be unique.
2. **Pricing and scale tier:** Choose a suitable size for your purposes here. Please note, there is also a small *free* version. For details take a look at the [Azure IoT Hub pricing information](https://azure.microsoft.com/en-us/pricing/details/iot-hub/).
3. **IoT Hub units** and **Device-to-cloud partitions:** The number of instances of the IoT Hub and its Device-to-cloud partitions you want to run simultaneously. This strongly depends on the number of devices you want to manage and messages per second. For details and decision help take a look at the  [Throttling guide](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-devguide/#throttling). For our demo 1 unit with 2 partitions is fine.
4. **Subscription:** Choose the subscription, you want to pay the IoT Hub with.
5. **Resource group:** Choose an existing resource group or create a new one to deploy the IoT Hub services in. I always recommend a new one for a better overview about costs and services
6. **Enable Device Management:** Device Management is an important part of the IoT Hub services. I recommend to activate it.
7. **Location:** Choose a location for your IoT Hub deployment.

Now click on ***Create*** to let Azure create your new IoT Hub. Lean back and give the portal some time, due this can take a couple of minutes.

## 2. Explore the IoT Hub
Now let's take a look at the most important settings and insights you can find at your IoT Hub. Once the portal finished creating the hub, you can click on its tile to open the overview dashboard as seen below.

![Azure IoT Hub Overview](/Misc/azureiothuboverview.png)

### Shared access policies - a word about security
At the left panel you can click through the different settings and configurations of your IoT Hub. Here you can scale your resource if you need more performance and adjust everything to your needs. The ***Shared access policies*** section is very important at the beginning. Here you can add policies for different users of your IoT hub and manage or regenrate their access keyes.

As the IoT Hub is used from different endpoints in different scenarios, it is very important to define these carefully. The `iothubowner` for example has Registry read/write, Service connect and Device connect permissions, which let's him modify everything inside your IoT Hub. From a security perspective it makes absolutely sense to restrict these permissions. This is what the other policies are for. As you can see, the `device` policy only allows Device connection. This is exremely important, because you do not want a single device to be able to register itself or make changes in your configuration. Image your device gets corrupted or stolen. In this case it is important to not let a hacker or thief add additional devices or give himself more permissions. So make sure that all parts of your IoT ecosystem only get the policies and their regarding *Shared access keys* they need.

![Azure IoT Hub Shared access policies settings](/Misc/azureiothubsharedaccess.png)

### Messaging
Messaging might be the most interesting part of the IoT Hub for you. This is the point where you manage the messages that your devices send to the cloud. Every IoT Hub in Azure contains an [EventHub](https://azure.microsoft.com/services/event-hubs/) that  works as a Message endpoint for your devices and has the infrastructure to receive millions of messages per second.

This is also, where you can grab your data and process it. We will learn how to process these messages later in this workshop in module [12 Processing and analyzing data](/12%20Processing%20and%20analyzing%20data).

![Azure IoT Hub Messaging settings](/Misc/azureiothubmessaging.png)

### Devices
As already mentioned, the Azure IoT Hub also offers device management. Every device that gets registered, has its own securtity key that you can easily disable if you don't trust the device anymore. Especially when the device gets stolen or corrupted it is important to be able to prevent specific devices from sending messages to your backend.

![Azure IoT Hub device management](/Misc/azureiothubdevices.png)