
using System;
using DSMogre;

namespace Mogre.Demo.Webcam
{
    class Program
    {
        public static void Main(string[] args)
        {
            var devices = Capture.CaptureDeviceNames;
            int deviceNum = 1;

            if (devices.Count > 0)
            {
                Console.WriteLine("Select capture source:");
                Console.WriteLine("1) Sample video");

                for (int i = 0; i < devices.Count && i < 7; i++)
                {
                    Console.WriteLine((i + 2) + ") " + devices[i]);
                }

                while (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out deviceNum)
                       || deviceNum == 0
                       || deviceNum >= devices.Count + 2)
                {
                }
            }
            else
            {
                Console.WriteLine("No webcam detected ... playing video file instead.");
            }

            new Webcam(deviceNum);
        }
    }
}