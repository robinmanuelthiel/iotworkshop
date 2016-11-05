# Managing devices
As the Azure IoT Hub also has device management implemented, we should register our device to the cloudservice. In terms of security, registering devices is super important. Every registered device, has its own securtity key that you can easily disable if you don't trust the device anymore. Especially when the device gets stolen or corrupted it is important to be able to prevent specific devices from sending messages to your backend.

## Register a new device
There are several ways to register a device at the IoT Hub. We will take a look at a couple, so you can decide, which option fits best for you.

### With the IoT Dashboard
We already worked with the IoT Dashboard in some of the previous modules and it can also register devices at an IoT Hub. 

> **Hint:** As I write this workshop, this option does not work with the German datacenters yet. I recommend the Device Explorer option, if you host inside the German Azure datacenters. 

Navigate to the ***Connect to Azure*** section. You might be asked to log into your Azure account. Now you can select an IoT Hub from the drop down menu and create a new device by clicking the ***Create new device*** button. You will be asked for a name and that's it, your device has been created. Unfortunately, the IoT Dashboard does not reveal us the security key that has been gerenated. We can find this in Azure at your IoT Hub's ***Devices*** section.

![IoT Dashboard device registrationr screenshots](/Misc/iotdashboarddeviceregistration.png)

> **Hint:** The IoT Dashboard also offers device provisioning. In this context, provisioning means to store the security key securely on the device. Typyically a good place to store such a key is a TPM chip, which the Raspberry Pi does not have. In this workshop, we need to store the key inside our code. If you wish to get more information about secure key storing and device provisioning, [check out this article](https://blogs.windows.com/buildingapps/2016/07/20/building-secure-apps-for-windows-iot-core/#M23T9JEcYFsVmS3r.97). 

### With the Device Explorer
The easiest and at the same time most flexible way to register a device might be the [Device Explorer](https://github.com/Azure/azure-iot-sdks/tree/master/tools/DeviceExplorer) which is an open source tool that manages IoT Hub connected devices simply with the respective connection string.

Open the tool and enter the connection string to your Azure IoT Hub into the according field. You can find the connection string in the Azure Portal at your IoT Hub's ***Shared access policies*** section, when clicking on one of the policies. Please make sure, that you provide the connection string from the  `iothubowner` policy, as it has the rights to manage devices.

Once you entered the connection string into the Device Explorer, hit the ***Update*** button and move on to the ***Management*** tab. Here you can easily manage and create new devices and see their security keys.

![Devcice Explorer screenshots](/Misc/deviceexplorer.png)

### With code or command line
Of course, you can also register devices via code to integrate device registration in your workflows or create your very own registration tool or use a command line tool if you want to script device registration or if you are not working with windows.

**Important:** Due to security reasons, devices should *never* gegister themselves at the IoT Hub. For the device registration, are *registry write* permissions (typyically the *iothubowner* policy) required, that should not be given to a single device. Especially in case of a stolen or corrupted device, it is important, to exclude the device without giving it the chance to re-register.

If you are interested in alternate ways to register devices, you can [check this article](http://pumpingco.de/different-ways-to-register-devices-to-the-azure-iot-hub/) I wrote about different ways of registering devices at an Azure IoT Hub.