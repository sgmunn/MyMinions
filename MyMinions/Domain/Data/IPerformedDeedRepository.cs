//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IPerformedDeedRepository.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using MonoKit.Data;

    public interface IPerformedDeedRepository : IRepository<PerformedDeedContract>
    {
        IEnumerable<PerformedDeedContract> GetTodaysForMinion(Guid id);
        IEnumerable<PerformedDeedContract> GetThisWeekForMinion(Guid id);
    }
}
