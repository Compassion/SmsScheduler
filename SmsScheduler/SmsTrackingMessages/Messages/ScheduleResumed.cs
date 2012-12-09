using System;
using NServiceBus;

namespace SmsTrackingMessages.Messages
{
    public class ScheduleResumed : IMessage
    {
        public Guid ScheduleId { get; set; }

        public DateTime RescheduledTime { get; set; }
    }
}