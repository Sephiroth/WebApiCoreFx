using System;

namespace ToolSet
{
    public class OrderedGuid
    {
        /// <summary>
        /// 用于生产id的唯一锁
        /// </summary>
        private static readonly object lockObj = new object();
        /// <summary>
        /// 计算guid生成的起始日期
        /// </summary>
        public static DateTime? StartDate { get; set; }
        public static short Order { get; set; }
        private static long lastTicks;

        #region 有序唯一GUID
        /// <summary>
        /// 根据时间 + 指定Order + 随机数生成 (mysql字符串识别顺序:左->右)
        /// </summary>
        /// <param name="order">指定主机序列</param>
        /// <returns></returns>
        public static Guid Generate(short? order = null)
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

        /// <summary>
        /// guid生成UInt64
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static UInt64 GenerateUint64(Guid? guid = null)
        {
            Guid id;
            if (guid.HasValue)
                id = guid.Value;
            else
                id = Generate(Order);

            byte[] arr = id.ToByteArray();
            byte[] newArr = new byte[arr.Length];
            Array.Copy(arr, 0, newArr, 8, 8);
            newArr[0] = arr[6];
            newArr[1] = arr[7];
            newArr[2] = arr[4];
            newArr[3] = arr[5];
            newArr[4] = arr[0];
            newArr[5] = arr[1];
            newArr[6] = arr[2];
            newArr[7] = arr[3];
            return BitConverter.ToUInt64(newArr, 0);
        }

        public static byte[] TicksToBytes()
        {
            long currentTicks = GetTimeTick();
            byte[] arr = new byte[8];
            arr[0] = (byte)(currentTicks >> 0x20);
            arr[1] = (byte)(currentTicks >> 0x28);
            arr[2] = (byte)(currentTicks >> 0x30);
            arr[3] = (byte)(currentTicks >> 0x38);
            arr[4] = (byte)(currentTicks >> 0x10);
            arr[5] = (byte)(currentTicks >> 0x18);
            arr[6] = (byte)currentTicks;
            arr[7] = (byte)(currentTicks >> 8);
            return arr;
        }

        public static Int64 GetTimeTick()
        {
            long currentTicks = DateTime.Now.Ticks;
            lock (lockObj)
            {
                while (true)
                {
                    if (currentTicks > lastTicks)
                    {
                        lastTicks = currentTicks;
                        break;
                    }
                    currentTicks = DateTime.Now.Ticks;
                }
                if (StartDate != null)
                {
                    if (StartDate >= DateTime.Today)
                    {
                        throw new ArgumentException("StartDate不允许大于当前机器日期");
                    }
                    currentTicks -= StartDate.Value.Ticks;
                }
            }
            return currentTicks;
        }
        #endregion


        #region 雪花算法
        private static readonly object snowObj = new object();
        private static long twepoch = 687888001020L; //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        private static long workerId=1;
        private static readonly int workerIdBits = 4; //机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)
        private static int sequenceBits = 10; //计数器字节数，10个字节用来保存计数码
        private static int workerIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private static int timestampLeftShift = sequenceBits + workerIdBits; //时间戳左移动位数就是机器码和计数器总字节数
        private static long sequence = 0L;

        public static long NextId()
        {
            long nextId = 0;
            lock (snowObj)
            {
                long timestamp = GetTimeTick();
                nextId = (timestamp - (StartDate.HasValue ? StartDate.Value.Ticks : 0) << timestampLeftShift) | workerId << workerIdShift | sequence;
            }
            return nextId;
        }
        #endregion


        static void Main(string[] args)
        {
            OrderedGuid.Order = 256;
            OrderedGuid.StartDate = new DateTime(1970, 1, 1);

            for (int i = 0; i < 100; i++)
            {
                //Guid rs = Generate();
                //var a = GenerateUint64(rs);
                //Console.WriteLine($"{a} - {rs}");
                Console.WriteLine(NextId());
            }
            Console.ReadLine();

        }
    }
}