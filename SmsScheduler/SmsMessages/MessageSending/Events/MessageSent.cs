﻿using System;
using SmsMessages.CommonData;

namespace SmsMessages.MessageSending.Events
{
    public class MessageSent
    {
        public SmsConfirmationData ConfirmationData { get; set; }

        public Guid CorrelationId { get; set; }

        public SmsData SmsData { get; set; }

        public SmsMetaData SmsMetaData { get; set; }

        public string ConfirmationEmailAddress { get; set; }
    }
}