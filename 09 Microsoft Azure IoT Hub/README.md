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
