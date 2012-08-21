//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IScheduledDeedRepository.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using MonoKit.Data;

    public interface IScheduledDeedRepository : IRepository<ScheduledDeedContract>
    {
        IEnumerable<ScheduledDeedContract> GetAllForMinion(Guid id);
    }
}
