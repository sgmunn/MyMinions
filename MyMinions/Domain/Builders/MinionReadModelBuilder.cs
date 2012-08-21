//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionReadModelBuilder.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
using MonoKit.Data;

namespace MyMinions.Domain.Builders
{
    using System;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data;

    public class MinionReadModelBuilder : ReadModelBuilder<MinionContract>
    {
        public MinionReadModelBuilder(IRepository<MinionContract> repository) : base(repository)
        {
        }

        public void Handle(DeletedEvent evt)
        {
            this.Repository.DeleteId(evt.Identity);
        }
    }
}
