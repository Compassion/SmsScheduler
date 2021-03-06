using System;
using System.Collections.Generic;
using SmsMessages.CommonData;

namespace SmsMessages.Coordinator.Commands
{
    public class TrickleSmsWithDefinedTimeBetweenEachMessage
    {
        public TrickleSmsWithDefinedTimeBetweenEachMessage()
        {
            ConfirmationEmails = new List<string>();
        }

        public Guid CoordinatorId { get; set; }

        public List<SmsData> Messages { get; set; }

        public DateTime StartTimeUtc { get; set; }

        public TimeSpan TimeSpacing { get; set; }

        public SmsMetaData MetaData { get; set; }

        [Obsolete]
        public string ConfirmationEmail { get; set; }

        public List<string> ConfirmationEmails { get; set; }

        public string UserOlsenTimeZone { get; set; }
    }
}