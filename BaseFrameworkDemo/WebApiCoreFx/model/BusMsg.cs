using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCoreFx.model
{
    public class BusMsg
    {
        public string Text { get; set; }
    }

    public class BusMsgConsumer : IConsumer<BusMsg>
    {
        public async Task Consume(ConsumeContext<BusMsg> context)
        {
            Console.WriteLine(context?.Message?.Text);
        }
    }
}