using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoRFIDReader
{
    /// <summary>
    /// Includes methods for Arduino device detection. 
    /// </summary>
    public static class ArduinoPortDetection
    {
        /// <summary>
        /// Returns port name from Win32_SerialPort with 'Arduino' phrase in it's description.
        /// </summary>
        /// <returns>Arduino device port name.</returns>
        public static string AutodetectPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino") || desc.Contains("CH340")) 
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                throw e;
            }

            return null;
        }
    }
}
