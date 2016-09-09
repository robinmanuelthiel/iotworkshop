# Managing devices
Coming soon...

## Register a new device
### With the IoT Dashboard
Coming soon...

## With code
Of course, you can also register devices via code to integrate device registration in your workflows or create your very own registration tool.

**Important:** Due to security reasons, devices should *never* gegister themselves at the IoT Hub. For the device registration, are *registry write* permissions (typyically the *iothubowner* policy) required, that should not be given to a single device. Especially in case of a stolen or corrupted device, it is important, to exclude the device without giving it the chance to re-register.

Coming soon...
- APIs and NuGet packages
- Sample registration app
- Walk through app
