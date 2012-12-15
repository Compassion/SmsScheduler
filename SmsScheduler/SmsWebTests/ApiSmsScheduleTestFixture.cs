using System;
using NServiceBus;
using NUnit.Framework;
using Rhino.Mocks;
using SmsMessages.CommonData;
using SmsMessages.Scheduling.Commands;
using SmsTracking;
using SmsWeb.API;
using SmsWeb.Models;
using IRavenDocStore = SmsWeb.IRavenDocStore;
using Schedule = SmsWeb.API.Schedule;

namespace SmsWebTests
{
    [TestFixture]
    public class ApiSmsScheduleTestFixture : RavenTestBase
    {
        private Guid scheduleMessageId = Guid.NewGuid();

        [Test]
        public void PostInvalidRequest()
        {
            var scheduleModel = new Schedule { Number = "number", ScheduledTimeUtc = DateTime.Now.AddHours(1) };
            var smsScheduleService = new SmsScheduleService();
            var result = smsScheduleService.OnPost(scheduleModel) as SmsScheduleResponse;

            Assert.That(result.RequestId, Is.EqualTo(Guid.Empty));
            Assert.That(result.ResponseStatus.ErrorCode, Is.EqualTo("InvalidRequest"));
        }

        [Test]
        public void PostValidRequest()
        {
            var bus = MockRepository.GenerateMock<IBus>();
            var scheduleModel = new Schedule { Number = "number", MessageBody = "m", ScheduledTimeUtc = DateTime.Now.AddHours(1) };

            bus.Expect(b => b.Send(Arg<ScheduleSmsForSendingLater>.Is.Anything));

            var smsScheduleService = new SmsScheduleService { Bus = bus };
            var result = smsScheduleService.OnPost(scheduleModel) as SmsScheduleResponse;

            Assert.That(result.RequestId, Is.Not.EqualTo(Guid.NewGuid()));
            Assert.That(result.ResponseStatus, Is.Null);
        }        

        [Test]
        public void GetNotFound()
        {
            var ravenDocStore = MockRepository.GenerateMock<IRavenDocStore>();
            ravenDocStore.Expect(r => r.GetStore()).Return(base.DocumentStore);
            var smsScheduleService = new SmsScheduleService { RavenDocStore = ravenDocStore };

            var request = new Schedule { ScheduleMessageId = Guid.NewGuid() };
            var response = smsScheduleService.OnGet(request) as SmsScheduleResponse;

            Assert.That(response.RequestId, Is.EqualTo(request.ScheduleMessageId));
            Assert.That(response.ResponseStatus.ErrorCode, Is.EqualTo("NotFound"));
            ravenDocStore.VerifyAllExpectations();
        }

        [Test]
        public void GetFound()
        {
            var ravenDocStore = MockRepository.GenerateMock<IRavenDocStore>();
            ravenDocStore.Expect(r => r.GetStore()).Return(base.DocumentStore);
            var smsScheduleService = new SmsScheduleService { RavenDocStore = ravenDocStore };

            var request = new Schedule { ScheduleMessageId = scheduleMessageId };
            var response = smsScheduleService.OnGet(request) as ScheduleModel;

            Assert.That(response.ScheduleMessageId, Is.EqualTo(request.ScheduleMessageId));
            ravenDocStore.VerifyAllExpectations();
        }

        [SetUp]
        public void Setup()
        {
            using (var session = base.DocumentStore.OpenSession())
            {
                var scheduleTrackingData = new ScheduleTrackingData
                {
                    MessageStatus = MessageStatus.Scheduled,
                    ScheduleId = scheduleMessageId
                };
                session.Store(scheduleTrackingData, scheduleMessageId.ToString());
                session.SaveChanges();
            }
        }
    }
}