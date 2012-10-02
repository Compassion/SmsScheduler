﻿using NServiceBus;
using SmsMessages;

namespace SmsCoordinator
{
    public class SmsActioner : IHandleMessages<SendOneMessageNow>
    {
        public ISmsService SmsService { get; set; }
        public IBus Bus { get; set; }

        public void Handle(SendOneMessageNow sendOneMessageNow)
        {
            var receipt = SmsService.Send(sendOneMessageNow);
            Bus.Publish<MessageSent>(m => m.Receipt = receipt);
        }
    }
}