using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        Task IEventBus.SendCommand<T>(T command)
        {
            return _mediator.Send(command);
        }

        void IEventBus.Publish<T>(T @event)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Get the event name
                    // Whoever uses the publish method sends a generic event
                    // Grab the name and get type via reflection
                    var eventName = @event.GetType().Name;
                    // declare a Queue on the RabbitMQ server with the exact name of the event
                    channel.QueueDeclare(eventName, false, false, false, null);
                    // get the message (the event) and serialize it
                    var message = JsonConvert.SerializeObject(@event);
                    // encode the message in a body
                    var body = Encoding.UTF8.GetBytes(message);
                    // use a channel to publish the message
                    channel.BasicPublish("", eventName, null, body);
                }
            }
        }

        

        void IEventBus.Subscribe<T, TH>()          
        {
            // takes in an event and the event handler
            // every event is a type of event which needs to be handled
            // make use of all _handlers _eventTypes so there are unique handlers
            // whenever someone subscribes to an event using the required handler

            // extract the event name
            var eventName = typeof(T).Name;
            // get handlerType
            var handlerType = typeof(TH);

            // if the event type is not already contained in the list, add it
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }
           
            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} is already registered for the '{eventName}'", nameof(handlerType));
            }
            
            _handlers[eventName].Add(handlerType);

            // start the consumption of the messages
            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            // create an asynchonous Consumer
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            // assign to a delegate - pointer to a method - assigning a method pointer with "+="
            consumer.Received += Consumer_Received;
            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            // a message has published on the Queue... pickup message and convert it
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            // process the event (kick off the event handler)
            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        // route to the correct handler
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
                    
            }
        }
    }
}
