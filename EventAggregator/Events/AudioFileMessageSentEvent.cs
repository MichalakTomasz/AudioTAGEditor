using Commons;
using Prism.Events;

namespace EventAggregator
{
    public class AudiofileMessageSentEvent : PubSubEvent<AudiofileMessage> { }
}
