## tibco-event-streaming

Demonstrates event streaming from Tibco using two methods - the binding MessageConsumer.MessageHandler to the event 
and setting the MessageConsumer.MessageListener property.


Seemingly in both scenarios events are received regardless of whether the previously received message has been acknowledged or not.
