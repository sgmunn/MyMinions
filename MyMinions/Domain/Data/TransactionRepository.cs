//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TransactionRepository.cs" company="sgmunn">
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

    public class TransactionRepository : SqlRepository<TransactionDataContract>, ITransactionRepository
    {
        public TransactionRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<TransactionDataContract> GetAllForMinion(Guid id)
        {
            return SynchronousTask.GetSync(() => 
               this.Connection.Table<TransactionDataContract>().Where(x => x.MinionId == id).AsEnumerable());
        }

        public void DeleteAllForMinion(Guid id)
        {
            SynchronousTask.DoSync(() =>
                   this.Connection.Execute("delete from TransactionDataContract where MinionId = ?", id)
                   );
        }
    }
}
