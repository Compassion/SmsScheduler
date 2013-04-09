using System;
using System.Collections.Generic;
using NUnit.Framework;
using SmsWeb.API;

namespace SmsWebTests
{
    [TestFixture]
    public class ApiCoordinatorModelToMessageMappingTestFixture
    {
        [Test]
        public void MapToTrickleSpacedByTimePeriod()
        {
            var model = new Coordinator
                {
                    Numbers = new List<string> {"04040404040","11111111111" },
                    Message = "Message",
                    StartTimeUtc = DateTime.UtcNow.AddHours(2),
                    TimeSeparator = new TimeSpan(0, 0,0, 90),
                    Tags = new List<string> {"tag1", "tag2" },
                    Topic = "Dance Dance Revolution!",
                    OlsenTimeZone = "Australia/Sydney"
                };

            var requestId = Guid.NewGuid();
            var mapper = new CoordinatorApiModelToMessageMapping();
            var message = mapper.MapToTrickleSpacedByPeriod(model, requestId);

            Assert.That(message.Messages.Count, Is.EqualTo(2));
            Assert.That(message.Messages[0].Mobile, Is.EqualTo(model.Numbers[0]));
            Assert.That(message.Messages[0].Message, Is.EqualTo(model.Message));
            Assert.That(message.Messages[1].Mobile, Is.EqualTo(model.Numbers[1]));
            Assert.That(message.Messages[1].Message, Is.EqualTo(model.Message));
            Assert.That(message.MetaData.Tags, Is.EqualTo(model.Tags));
            Assert.That(message.MetaData.Topic, Is.EqualTo(model.Topic));
            Assert.That(message.StartTimeUtc, Is.EqualTo(model.StartTimeUtc));
            Assert.That(message.TimeSpacing, Is.EqualTo(model.TimeSeparator));
            Assert.IsNull(message.ConfirmationEmail);
            Assert.That(message.UserOlsenTimeZone, Is.EqualTo(model.OlsenTimeZone));
        }

        [Test]
        public void MapToTrickleOverTimePeriod()
        {
            var model = new Coordinator
                {
                    Numbers = new List<string> { "04040404040", "11111111111" },
                    Message = "Message",
                    StartTimeUtc = DateTime.UtcNow.AddHours(2),
                    SendAllByUtc = DateTime.UtcNow.AddHours(3),
                    Tags = new List<string> { "tag1", "tag2" },
                    Topic = "Dance Dance Revolution!",
                    OlsenTimeZone = string.Empty
                };

            var requestId = Guid.NewGuid();
            var mapper = new CoordinatorApiModelToMessageMapping();
            var message = mapper.MapToTrickleOverPeriod(model, requestId);

            var coordinationDuration = model.SendAllByUtc.Value.Subtract(model.StartTimeUtc);
            Assert.That(message.Messages.Count, Is.EqualTo(2));
            Assert.That(message.Messages[0].Mobile, Is.EqualTo(model.Numbers[0]));
            Assert.That(message.Messages[0].Message, Is.EqualTo(model.Message));
            Assert.That(message.Messages[1].Mobile, Is.EqualTo(model.Numbers[1]));
            Assert.That(message.Messages[1].Message, Is.EqualTo(model.Message));
            Assert.That(message.MetaData.Tags, Is.EqualTo(model.Tags));
            Assert.That(message.MetaData.Topic, Is.EqualTo(model.Topic));
            Assert.That(message.StartTimeUtc, Is.EqualTo(model.StartTimeUtc));
            Assert.That(message.Duration, Is.EqualTo(coordinationDuration));
            Assert.IsNull(message.ConfirmationEmail);
            Assert.That(message.UserOlsenTimeZone, Is.EqualTo("UTC"));
        }

        [Test]
        public void MapToSendAllAtOnce()
        {
            var model = new Coordinator
                {
                    Numbers = new List<string> { "04040404040", "11111111111" },
                    Message = "Message",
                    StartTimeUtc = DateTime.UtcNow.AddHours(2),
                    SendAllAtOnce = true,
                    Tags = new List<string> { "tag1", "tag2" },
                    Topic = "Dance Dance Revolution!",
                    OlsenTimeZone = string.Empty
                };

            var requestId = Guid.NewGuid();
            var mapper = new CoordinatorApiModelToMessageMapping();
            var message = mapper.MapToSendAllAtOnce(model, requestId);

            Assert.That(message.Messages.Count, Is.EqualTo(2));
            Assert.That(message.Messages[0].Mobile, Is.EqualTo(model.Numbers[0]));
            Assert.That(message.Messages[0].Message, Is.EqualTo(model.Message));
            Assert.That(message.Messages[1].Mobile, Is.EqualTo(model.Numbers[1]));
            Assert.That(message.Messages[1].Message, Is.EqualTo(model.Message));
            Assert.That(message.MetaData.Tags, Is.EqualTo(model.Tags));
            Assert.That(message.MetaData.Topic, Is.EqualTo(model.Topic));
            Assert.That(message.SendTimeUtc, Is.EqualTo(model.StartTimeUtc));
            Assert.IsNull(message.ConfirmationEmail);
            Assert.That(message.UserOlsenTimeZone, Is.EqualTo("UTC"));
        }
    }
}