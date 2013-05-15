using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIBCO.EMS;

namespace Tibco.EventStreaming {
    class Program {

        private static readonly bool _useEvent = false;
        private static readonly bool _ack = false;

        static void Main(string[] args) {

            var factory = new ConnectionFactory("tcp://localhost:7222");
            var connection = factory.CreateConnection("admin", "");
            var session = connection.CreateSession(false, SessionMode.ExplicitClientAcknowledge);
            var queue = session.CreateQueue("test-queue");

            var consumer = session.CreateConsumer(queue);

            Console.WriteLine("Managed thread ID is {0}.", Thread.CurrentThread.ManagedThreadId);

            if(_useEvent) {
                consumer.MessageHandler += new EMSMessageHandler(OnMessageReceived);
                Console.WriteLine("Using MessageHandler.");
            } else {
                consumer.MessageListener = new SimpleListener(_ack);
                Console.WriteLine("Using MessageListener.");
            }

            connection.Start();

            Console.WriteLine("Connected, waiting for messages. Press any key to disconnect.");
            Console.ReadLine();

            if(_useEvent) {
                consumer.MessageHandler -= new EMSMessageHandler(OnMessageReceived);
            }

            consumer.Close();
            session.Close();

            connection.Stop();
            connection.Close();

            Console.WriteLine("Disconnected. Press any key to exit.");

            Console.ReadLine();
        }

        private static void OnMessageReceived(Object source, EMSMessageEventArgs args) {
            SimpleListener.HandleMessage(args.Message, _ack);
        }

        private class SimpleListener : IMessageListener {

            private readonly bool _ack;

            public SimpleListener(bool ack) {
                _ack = ack;
            }

            public void OnMessage(Message message) {
                SimpleListener.HandleMessage(message, _ack);
            }

            public static void HandleMessage(Message message, bool ack) {

                var text = (message as TextMessage);

                if(text != null) {

                    Console.WriteLine();
                    Console.WriteLine("[Thread {0}] Message received = {1}", Thread.CurrentThread.ManagedThreadId, text.Text);

                    if(ack) {
                        text.Acknowledge();
                        Console.WriteLine("Message acknowledged");
                    }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
