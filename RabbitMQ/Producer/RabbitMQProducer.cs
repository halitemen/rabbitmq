using RabbitMQ.Client;
using RabbitMQ.Connection;
using RabbitMQ.Data.Interfaces;
using System;
using System.Text;

namespace RabbitMQ.Producer
{
    public class RabbitMQProducer : IRabbitMQPublisher
    {
        #region Singleton Section
        private static readonly Lazy<RabbitMQProducer> _instance = new Lazy<RabbitMQProducer>(() => new RabbitMQProducer());

        private RabbitMQProducer()
        {

        }

        public static RabbitMQProducer Instance => _instance.Value;

        #endregion

        #region Member

        /// <summary>
        /// Rabbitmq bağlantısı getirmek için oluşturulan class
        /// </summary>
        private RabbitMQConnection _rabbitMqConnection;

        #endregion

        #region Property

        /// <summary>
        /// Rabbitmq bağlantısı almak için
        /// </summary>
        private RabbitMQConnection RabbitMqConnection
        {
            get
            {
                if (_rabbitMqConnection == null || !_rabbitMqConnection.IsConnected)
                    _rabbitMqConnection = new RabbitMQConnection();
                return _rabbitMqConnection;
            }
            set => _rabbitMqConnection = value;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Aldığı mesajı aldığı kuyruğa yazar
        /// </summary>
        /// <param name="queueName">kuyruk adı</param>
        /// <param name="message">mesaj</param>
        public void Publish(string queueName, object message)
        {
            try
            {
                using (IModel channel = RabbitMqConnection.GetChannel(queueName))
                {
                    channel.BasicPublish(string.Empty, queueName, null, Encoding.UTF8.GetBytes(message.ToString()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{"RabbitMQPublisher" + ex.Message}");
            }
        }

        #endregion
    }
}
