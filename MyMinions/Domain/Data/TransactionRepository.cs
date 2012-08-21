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

    public class TransactionRepository : SqlRepository<TransactionContract>, ITransactionRepository
    {
        public TransactionRepository(SQLiteConnection connection) : base(connection)
        {
        }

        public IEnumerable<TransactionContract> GetAllForMinion(Guid id)
        {
            return SynchronousTask.GetSync(() => 
               this.Connection.Table<TransactionContract>().Where(x => x.MinionId == id).AsEnumerable());
        }

        public void DeleteAllForMinion(Guid id)
        {
            SynchronousTask.DoSync(() =>
                   this.Connection.Execute("delete from TransactionDataContract where MinionId = ?", id)
                   );
        }
    }
}
