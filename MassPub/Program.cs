using Autofac;
using Entitys;
using MassTransit;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MassPub
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
            //    {

            //    });

            //    sbc.ReceiveEndpoint(host, "test_queue", ep =>
            //    {

            //        //ep.Handler<SubData>(context =>
            //        //{
            //        //    return Console.Out.WriteLineAsync($"Received: {context.Message.Name}");
            //        //});
            //    });
            //});

            //bus.Start(); // This is important!
            var builder = new ContainerBuilder();
            builder.AddMassTransit(m =>
            {
                // m.AddConsumer<SubConsumer>();
                m.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                    });

                    cfg.ReceiveEndpoint("test_queue", es => {
                     //   es.ConfigureConsumers(context);
                    });
                }));
            });
           var cont= builder.Build();
           var bus= cont.Resolve<IBusControl>();
            bus.Publish<SubData>(new {
                Name="joah",
                Id="sdcsd"
            });
            Console.Read();
        }
    }
    
}
