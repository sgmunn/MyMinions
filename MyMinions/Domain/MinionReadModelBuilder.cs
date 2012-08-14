//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file=".cs" company="sgmunn">
//    (c) sgmunn 2012  
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//    documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
//    to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
//    the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
//    THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//    CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
//    IN THE SOFTWARE.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
//

namespace MyMinions.Domain
{
    using System;
    using MonoKit.Domain;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Events;
    using MonoKit.Domain.Data;

    public class MinionReadModelBuilder : ReadModelBuilder
    {
        private readonly IMinionRepository repository;

        public MinionReadModelBuilder(IMinionRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeletedEvent evt)
        {
            this.repository.DeleteId(evt.AggregateId);
            this.NotifyReadModelChange(evt.AggregateId, ReadModelChange.Deleted);
        }
    }

    public class ScheduledDeedReadModelBuilder : ReadModelBuilder
    {
        private readonly IScheduledDeedRepository repository;

        public ScheduledDeedReadModelBuilder(IScheduledDeedRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeedScheduledEvent evt)
        {
            var scheduledEvent = new ScheduledDeedDataContract
            {
                DeedId = evt.DeedId,
                Description = evt.Description,
                Monday = evt.Monday,
                Tuesday = evt.Tuesday,
                Wednesday = evt.Wednesday,
                Thursday = evt.Thursday,
                Friday = evt.Friday,
                Saturday = evt.Saturday,
                Sunday = evt.Sunday,
                MinionId = evt.AggregateId.Id,
            };

            this.repository.Save(scheduledEvent);
            this.NotifyReadModelChange(scheduledEvent, ReadModelChange.Changed);
        }

        public void Handle(DeedUnscheduledEvent evt)
        {
            this.repository.DeleteId(evt.DeedId);
 //           this.NotifyReadModelChange(evt.DeedId, ReadModelChange.Deleted);
        }
    }

    public class PerformedDeedReadModelBuilder : ReadModelBuilder
    {
        private readonly IPerformedDeedRepository repository;

        public PerformedDeedReadModelBuilder(IPerformedDeedRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeedPerformedEvent evt)
        {
            var performedEvent = new PerformedDeedDataContract
            {
                MinionId = evt.AggregateId.Id,
                DeedId = evt.DeedId,
                Date = evt.Date,
            };

            this.repository.Save(performedEvent);
            this.NotifyReadModelChange(performedEvent, ReadModelChange.Changed);
        }

        public void Handle(DeedResetEvent evt)
        {
            // todo: ??
            this.repository.DeleteId(evt.Id);
   //         this.NotifyReadModelChange(evt.Id, ReadModelChange.Deleted);
        }
    }

}
