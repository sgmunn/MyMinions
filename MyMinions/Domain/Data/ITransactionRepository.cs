//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ITransactionRepository.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using MonoKit.Data;

    public interface ITransactionRepository : IRepository<TransactionDataContract>
    {
        IEnumerable<TransactionDataContract> GetAllForMinion(Guid id);
        void DeleteAllForMinion(Guid id);
    }
}
