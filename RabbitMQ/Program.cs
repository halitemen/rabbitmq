using Newtonsoft.Json;
using RabbitMQ.Consumer;
using RabbitMQ.Data.Model;
using RabbitMQ.Producer;
using System;
using System.Threading;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQMessageModel rabbitMQMessageModel = new RabbitMQMessageModel()
            {
                Id = 1,
                Message = "RABBITMQEXAMPLE"
            };
            RabbitMQProducer.Instance.Publish("RABBITMQEXAMPLE", JsonConvert.SerializeObject(rabbitMQMessageModel));

            ThreadPool.QueueUserWorkItem(delegate
            {
                RabbitMQConsumer.Instance.RabbitConsumer("RABBITMQEXAMPLE");
            });
            Console.ReadLine();
        }
    }
}
