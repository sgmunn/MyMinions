//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TransactionReadModelBuilder.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Builders
{
    using System;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data;

    public class TransactionReadModelBuilder : ReadModelBuilder<TransactionDataContract>
    {
        public TransactionReadModelBuilder(ITransactionRepository repository) : base(repository)
        {
        }

        public void Handle(DeletedEvent evt)
        {
            // todo: improve - how to notify of all deletions ?
            ((ITransactionRepository)this.Repository).DeleteAllForMinion(evt.Identity.Id);
        }

        public void Handle(AllowanceEarntEvent evt)
        {
            var trans = new TransactionDataContract
            {
                Id = evt.Transactionid,
                IsSpend = false,
                Amount = evt.Amount,
                Description = evt.Description,
                MinionId = evt.Identity.Id,
                TransactionDate = evt.Date,
                AsCash = evt.AsCash,
            };

            this.Repository.Save(trans);
        }

        public void Handle(AllowanceSpentEvent evt)
        {
            var trans = new TransactionDataContract
            {
                Id = evt.Transactionid,
                IsSpend = true,
                Amount = evt.Amount,
                Description = evt.Description,
                MinionId = evt.Identity.Id,
                TransactionDate = evt.Date,
                AsCash = evt.FromCash,
            };

            this.Repository.Save(trans);
        }
    }
}
