using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Connection;
using RabbitMQ.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Consumer
{  /// <summary>
   /// Rabbitmq Consumer classı
   /// </summary>
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private RabbitMQConsumer()
        {
        }

        #endregion

        #region Member

        /// <summary>
        /// Bu class çağırıldığında tanımlanır
        /// </summary>
        private static readonly Lazy<RabbitMQConsumer>
            instance = new Lazy<RabbitMQConsumer>(() => new RabbitMQConsumer());

        /// <summary>
        /// Rabbitmq bağlantısı getirmek için oluşturulan class
        /// </summary>
        private RabbitMQConnection rabbitMqServices;

        /// <summary>
        /// Modeli dinlemek için kullanıclan event
        /// </summary>
        private EventingBasicConsumer eventingBasicConsumer;

        #endregion

        #region Property

        /// <summary>
        /// bu classa dışardan erişimi sağlar
        /// </summary>
        public static RabbitMQConsumer Instance => instance.Value;

        /// <summary>
        /// Rabbitmq bağlantısı getirmek için oluşturulan class
        /// </summary>
        public RabbitMQConnection RabbitMqServices
        {
            get
            {
                if (rabbitMqServices == null || !rabbitMqServices.IsConnected)
                {
                    rabbitMqServices = new RabbitMQConnection();
                }

                return rabbitMqServices;
            }
            set => rabbitMqServices = value;
        }

        #endregion

        #region Method

        /// <summary>
        /// Gelen kuyruktaki mesajları okur
        /// </summary>
        /// <param name="queue"></param>
        public void RabbitConsumer(string queue)
        {
            try
            {
                IModel channel = RabbitMqServices.GetChannel(queue);
                eventingBasicConsumer = new EventingBasicConsumer(channel);
                eventingBasicConsumer.Received += EventingBasicConsumerOnReceived;
                channel.BasicConsume(queue, true, eventingBasicConsumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex + "RabbitmqConsumer" }"
                );
            }
        }

        /// <summary>
        ///kuruktaki mesaj düştükten sonra çalışan metot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventingBasicConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            string jsonData = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(jsonData);
        }
        #endregion
    }
}
