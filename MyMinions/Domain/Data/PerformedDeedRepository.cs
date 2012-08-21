//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PerformedDeedRepository.cs" company="sgmunn">
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

    public class PerformedDeedRepository : SqlRepository<PerformedDeedContract>, IPerformedDeedRepository
    {
        public PerformedDeedRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<PerformedDeedContract> GetTodaysForMinion(Guid id)
        {
            DateTime today = DateTime.Today;

            return SynchronousTask.GetSync(() => 
               this.Connection.Table<PerformedDeedContract>().Where(x => x.MinionId == id && x.Date == today).AsEnumerable());
        }

        public IEnumerable<PerformedDeedContract> GetThisWeekForMinion(Guid id)
        {
            DateTime monday = DateTime.Today;
            DateTime sunday = DateTime.Today;

            return SynchronousTask.GetSync(() => 
               this.Connection.Table<PerformedDeedContract>().Where(x => x.MinionId == id && x.Date >= monday && x.Date <= sunday).AsEnumerable());
        }
    }
}

