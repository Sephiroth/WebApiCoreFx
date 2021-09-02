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

        //static void Main(string[] args)
        //{
        //    OrderedGuid.Order = 256;
        //    OrderedGuid.StartDate = new DateTime(1970, 1, 1);
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        //Guid rs = Generate();
        //        //var a = GenerateUint64(rs);
        //        //Console.WriteLine($"{a} - {rs}");
        //        //Console.WriteLine(NextId());
        //    }
        //    Console.ReadLine();
        //}
    }

    public class SnowFlake
    {
        private static readonly object lockObj = new object();
        //下面两个每个5位，加起来就是10位的工作机器id
        public long WorkerId { get; private set; }    //工作id
        public long DatacenterId { get; private set; }   //数据id
        public long Sequence { get; private set; } //12位的序列号

        public SnowFlake(long workerId, long datacenterId, long sequence)
        {
            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {maxWorkerId} or less than 0");
            }
            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {maxDatacenterId} or less than 0");
            }
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;
        }

        //初始时间戳
        private static long twepoch = 537403855732986298L;
        //长度为5位
        private static byte workerIdBits = 5;
        private static byte datacenterIdBits = 5;
        //最大值
        private static long maxWorkerId = long.MaxValue;
        private static long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);
        //序列号id长度
        private static byte sequenceBits = 12;
        //序列号最大值
        private static long sequenceMask = -1L ^ (-1 << sequenceBits);
        //数据id需要左移位数 12+5=17位
        private static int datacenterIdShift = sequenceBits + workerIdBits;
        //时间戳需要左移位数 12+5+5=22位
        private static int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;
        //上次时间戳，初始值为负数
        private static long lastTimestamp = -1L;
        //获取系统时间戳
        public long GetTimestamp()
        {
            return DateTime.Now.Ticks;
        }

        //下一个ID生成算法
        public long NextId()
        {
            long timestamp;
            lock (lockObj)
            {
                timestamp = GetTimestamp();
                if (timestamp < lastTimestamp) //获取当前时间戳如果小于上次时间戳，则表示时间戳获取出现异常
                {
                    throw new ArgumentException($"当前时间戳Ticks:{timestamp}小于上次时间戳:{lastTimestamp}");
                }
                //获取当前时间戳如果等于上次时间戳（同一毫秒内），则在序列号加一；否则序列号赋值为0，从0开始。
                if (lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & sequenceMask;
                    if (Sequence == 0)
                    {
                        timestamp = NextMillis(lastTimestamp);
                    }
                }
                else { Sequence = 0; }
                //将上次时间戳值刷新
                lastTimestamp = timestamp;

                /**
                  * 返回结果：
                  * (timestamp - twepoch) << timestampLeftShift) 表示将时间戳减去初始时间戳，再左移相应位数
                  * (datacenterId << datacenterIdShift) 表示将数据id左移相应位数
                  * (workerId << workerIdShift) 表示将工作id左移相应位数
                  * | 是按位或运算符，例如：x | y，只有当x，y都为0的时候结果才为0，其它情况结果都为1。
                  * 因为个部分只有相应位上的值有意义，其它位上都是0，所以将各部分的值进行 | 运算就能得到最终拼接好的id
                */
            }
            return ((timestamp - twepoch) << timestampLeftShift) |
                    (DatacenterId << datacenterIdShift) |
                    (WorkerId << sequenceBits) |
                    Sequence;
        }

        //获取时间戳，并与上次时间戳比较
        private long NextMillis(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetTimestamp();
            }
            return timestamp;
        }

    }

}