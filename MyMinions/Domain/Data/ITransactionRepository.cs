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

    public interface ITransactionRepository : IRepository<TransactionContract>
    {
        IEnumerable<TransactionContract> GetAllForMinion(Guid id);
        void DeleteAllForMinion(Guid id);
    }
}
