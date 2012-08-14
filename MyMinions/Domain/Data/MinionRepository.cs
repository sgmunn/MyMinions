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

namespace MyMinions.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Linq;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;

    public interface IMinionRepository : IRepository<MinionDataContract>
    {
    }

    public class MinionRepository : SQLiteRepository<MinionDataContract>, IMinionRepository
    {
        public MinionRepository(SQLiteConnection connection) : base(connection)
        {
        }
    }

    public interface ITransactionRepository : IRepository<TransactionDataContract>
    {
        IEnumerable<TransactionDataContract> GetAllForMinion(Guid id);
        void DeleteAllForMinion(Guid id);
    }

    public class TransactionRepository : SQLiteRepository<TransactionDataContract>, ITransactionRepository
    {
        public TransactionRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<TransactionDataContract> GetAllForMinion(Guid id)
        {
            return GetSync(() => 
               this.Connection.Table<TransactionDataContract>().Where(x => x.MinionId == id).AsEnumerable());
        }

        public void DeleteAllForMinion(Guid id)
        {
            DoSync(() =>
                   this.Connection.Execute("delete from TransactionDataContract where MinionId = ?", id)
                   );
        }
    }

    public interface IScheduledDeedRepository : IRepository<ScheduledDeedDataContract>
    {
        IEnumerable<ScheduledDeedDataContract> GetAllForMinion(Guid id);
    }

    public class ScheduledDeedRepository : SQLiteRepository<ScheduledDeedDataContract>, IScheduledDeedRepository
    {
        public ScheduledDeedRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<ScheduledDeedDataContract> GetAllForMinion(Guid id)
        {
            return GetSync(() => 
               this.Connection.Table<ScheduledDeedDataContract>().Where(x => x.MinionId == id).AsEnumerable());
        }
    }

    public interface IPerformedDeedRepository : IRepository<PerformedDeedDataContract>
    {
        IEnumerable<PerformedDeedDataContract> GetTodaysForMinion(Guid id);
        IEnumerable<PerformedDeedDataContract> GetThisWeekForMinion(Guid id);
    }

    public class PerformedDeedRepository : SQLiteRepository<PerformedDeedDataContract>, IPerformedDeedRepository
    {
        public PerformedDeedRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<PerformedDeedDataContract> GetTodaysForMinion(Guid id)
        {
            DateTime today = DateTime.Today;

            return GetSync(() => 
               this.Connection.Table<PerformedDeedDataContract>().Where(x => x.MinionId == id && x.Date == today).AsEnumerable());
        }

        public IEnumerable<PerformedDeedDataContract> GetThisWeekForMinion(Guid id)
        {
            DateTime monday = DateTime.Today;
            DateTime sunday = DateTime.Today;

            return GetSync(() => 
               this.Connection.Table<PerformedDeedDataContract>().Where(x => x.MinionId == id && x.Date >= monday && x.Date <= sunday).AsEnumerable());
        }
    }
}

