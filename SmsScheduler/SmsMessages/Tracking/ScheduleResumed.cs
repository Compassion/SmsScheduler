using System;
using NServiceBus;

namespace SmsMessages.Tracking
{
    public class ScheduleResumed : IMessage
    {
        public Guid ScheduleId { get; set; }

        public DateTime RescheduledTime { get; set; }
    }
}