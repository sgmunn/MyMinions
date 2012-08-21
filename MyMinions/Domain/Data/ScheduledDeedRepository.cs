//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScheduledDeedRepository.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MonoKit.Data.SQLite;
    using MonoKit.Tasks;

    public class ScheduledDeedRepository : SqlRepository<ScheduledDeedContract>, IScheduledDeedRepository
    {
        public ScheduledDeedRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<ScheduledDeedContract> GetAllForMinion(Guid id)
        {
            return SynchronousTask.GetSync(() => 
               this.Connection.Table<ScheduledDeedContract>().Where(x => x.MinionId == id).AsEnumerable());
        }
    }
}
