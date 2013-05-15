## Tibco EMS event streaming

Demonstrates event streaming from Tibco using two methods - the binding MessageConsumer.MessageHandler to the event 
and setting the MessageConsumer.MessageListener property.


Seemingly in both scenarios events are received regardless of whether the previously received message has been acknowledged or not.

### TIBCO.EMS.dll

Due to licensing reasons you will need to provide your own TIBCO.EMS.dll. The project is currently configured to expect it in 
a top level folder called lib alongside src.
