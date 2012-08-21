//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScheduledDeedReadModelBuilder.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Builders
{
    using System;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data;
    using MonoKit.Data;

    public class ScheduledDeedReadModelBuilder : ReadModelBuilder<ScheduledDeedContract>
    {
        public ScheduledDeedReadModelBuilder(IRepository<ScheduledDeedContract> repository) : base(repository)
        {
        }

        public void Handle(DeedScheduledEvent evt)
        {
            var scheduledEvent = new ScheduledDeedContract
            {
                Id = evt.ScheduledDeedId,
                DeedId = evt.DeedId,
                Description = evt.Description,
                Monday = evt.Monday,
                Tuesday = evt.Tuesday,
                Wednesday = evt.Wednesday,
                Thursday = evt.Thursday,
                Friday = evt.Friday,
                Saturday = evt.Saturday,
                Sunday = evt.Sunday,
                MinionId = evt.Identity.Id,
            };

            this.Repository.Save(scheduledEvent);
        }

        public void Handle(DeedUnscheduledEvent evt)
        {
            this.Repository.DeleteId(evt.ScheduledDeedId);
        }
    }
}
