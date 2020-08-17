using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ToolSet
{
    public class OrderedGuid
    {
        public static short Order { get; set; }

        /// <summary>
        /// 根据时间 + 指定Order + 随机数生成
        /// </summary>
        /// <param name="order">指定主机序列</param>
        /// <returns></returns>
        public static Guid Generate(short? order = null) //mysql(->从左向右顺序)
        {
            order ??= Order;
            byte[] arr = new byte[16];
            byte[] ticks = TicksToBytes();
            Array.Copy(ticks, 0, arr, 0, ticks.Length);
            arr[8] = (byte)(order >> 8);
            arr[9] = (byte)order;
            byte[] gArr = Guid.NewGuid().ToByteArray();
            Array.Copy(gArr, 0, arr, 10, 6);
            return new Guid(arr);
        }

        public static byte[] TicksToBytes()
        {
            long value = DateTime.Now.Ticks;
            byte[] arr = new byte[8];
            arr[0] = (byte)(value >> 0x20);
            arr[1] = (byte)(value >> 0x28);
            arr[2] = (byte)(value >> 0x30);
            arr[3] = (byte)(value >> 0x38);
            arr[4] = (byte)(value >> 0x10);
            arr[5] = (byte)(value >> 0x18);
            arr[6] = (byte)value;
            arr[7] = (byte)(value >> 8);
            return arr;
        }


        static void Main(string[] args)
        {
            OrderedGuid.Order = 256;
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int a = random.Next(50, 500);
                var rs = Generate();
                Console.WriteLine($"{rs.ToString()}     {a }");
                Thread.Sleep(a);
            }
            Console.ReadLine();
        }
    }
}