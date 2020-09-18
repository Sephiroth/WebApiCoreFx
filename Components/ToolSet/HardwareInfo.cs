using System.Management;

namespace ToolSet
{
    public class HardwareInfo
    {
        private static string macAddress;
        private static string cpuSerialNumber;
        private static string boardSerialNumber;
        private static string hardDiskNumber;

        /// <summary>
        /// 获取网卡mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacinfo()
        {
            if (string.IsNullOrEmpty(macAddress))
            {
                string mac = string.Empty;
                try
                {
                    using ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    using ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        bool? ipEnabled = mo["IPEnabled"] as bool?;
                        if (ipEnabled.HasValue && ipEnabled.Value)
                        {
                            mac = $"{mac}{mo["MacAddress"]}";
                            break;
                        }
                    }
                    macAddress = mac;
                }
                catch { }
            }
            return macAddress;
        }

        /// <summary>
        /// 获取Cpu序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuSerialNumber()
        {
            if (string.IsNullOrEmpty(cpuSerialNumber))
            {
                try
                {
                    using ManagementClass mc = new ManagementClass("Win32_Processor");
                    using ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        cpuSerialNumber = mo.Properties["ProcessorId"].Value?.ToString();
                        break;
                    }
                }
                catch { }
            }
            return cpuSerialNumber;
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string GetBoardSerialNumber()
        {
            if (string.IsNullOrEmpty(boardSerialNumber))
            {
                try
                {
                    using ManagementClass mc = new ManagementClass("Win32_BaseBoard");
                    using ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        boardSerialNumber = mo.Properties["SerialNumber"].Value?.ToString();
                        break;
                    }
                }
                catch { }
            }
            return boardSerialNumber;
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskSerialNumber()
        {
            if (string.IsNullOrEmpty(hardDiskNumber))
            {
                try
                {
                    using ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
                    using ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        hardDiskNumber = mo.Properties["SerialNumber"].Value?.ToString();
                        break;
                    }
                }
                catch { }
            }
            return hardDiskNumber;
        }

    }
}