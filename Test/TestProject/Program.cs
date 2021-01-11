using MQHelper.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
                try
                {
                    Return();
                }
                catch
                {
                }
            sw.Stop();
            Console.WriteLine("With Return " + sw.ElapsedTicks);
            sw.Restart();
            for (int i = 0; i < 100; i++)
                try
                {
                    ThrowTest();
                }
                catch
                {
                }
            sw.Stop();
            Console.WriteLine("With Exception " + sw.ElapsedTicks);
            Console.ReadLine();
        }

        public static void ThrowTest()
        {
            throw new Exception("This is exceptopn");
        }
        public static Boolean Return()
        {
            return false;
        }
    }
}
