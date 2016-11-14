using Microsoft.Azure.Devices;
using System;
using System.Text;

namespace CloudToDeviceDemo.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
