using Autofac;
using Entitys;
using MassTransit;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MassSub
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.AddMassTransit(m =>
            {
                m.AddConsumers(Assembly.GetExecutingAssembly());
                //m.AddConsumer<SubConsumer>();
                m.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                    });

                    cfg.ReceiveEndpoint("test_queue",es=> {
                        es.ConfigureConsumers(context);
                    });
                }));
            });
            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
            //    {

            //    });

            //    sbc.ReceiveEndpoint(host, "test_queue", ep =>
            //    {
            //        ep.Consumer<SubConsumer>();
            //        //ep.Handler<SubData>(context =>
            //        //{
            //        //    return Console.Out.WriteLineAsync($"Received: {context.Message.Name}");
            //        //});
            //    });
            //});
           var conf= builder.Build();
            var bc = conf.Resolve<IBusControl>();
            bc.Start();
            // bus.Start(); // This is important!
            Console.Read();
        }
    }
    public class SubConsumer : IConsumer<SubData>
    {
        public Task Consume(ConsumeContext<SubData> context)
        {
            Console.WriteLine(context.Message.Name);
            return Task.CompletedTask;

        }
    }
}
