//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PerformedDeedReadModelBuilder.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Builders
{
    using System;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data;
    using MonoKit.Data;

    public class PerformedDeedReadModelBuilder : ReadModelBuilder<PerformedDeedContract>
    {
        public PerformedDeedReadModelBuilder(IRepository<PerformedDeedContract> repository) : base(repository)
        {
        }

        public void Handle(DeedPerformedEvent evt)
        {
            var performedEvent = new PerformedDeedContract
            {
                Id = evt.PerformedDeedId,
                MinionId = evt.Identity.Id,
                DeedId = evt.DeedId,
                Date = evt.Date,
            };

            this.Repository.Save(performedEvent);
        }

        public void Handle(DeedResetEvent evt)
        {
            this.Repository.DeleteId(evt.PerformedDeedId);
        }
    }
}
