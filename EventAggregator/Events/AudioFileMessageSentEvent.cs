using Commons;
using Prism.Events;

namespace EventAggregator
{
    public class AudioFileMessageSentEvent : PubSubEvent<AudioFileMessage> { }
}
