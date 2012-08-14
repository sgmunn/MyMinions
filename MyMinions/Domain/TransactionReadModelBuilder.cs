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

namespace MyMinions.Domain
{
    using System;
    using MonoKit.Domain;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Events;
    using MonoKit.Domain.Data;

    public class TransactionReadModelBuilder : ReadModelBuilder
    {
        private readonly ITransactionRepository repository;

        public TransactionReadModelBuilder(ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeletedEvent evt)
        {
            this.repository.DeleteAllForMinion(evt.AggregateId.Id);
            // todo: how to notify of all deletions??
            //this.NotifyReadModelChange(trans, ReadModelChange.Updated);
        }

        public void Handle(AllowanceEarntEvent evt)
        {
            var trans = new TransactionDataContract
            {
                // todo: transaction id Id = evt.EventId,
                IsSpend = false,
                Amount = evt.Amount,
                Description = evt.Description,
                MinionId = evt.AggregateId.Id,
                TransactionDate = evt.Date,
                AsCash = evt.AsCash,
            };

            this.repository.Save(trans);
            this.NotifyReadModelChange(trans, ReadModelChange.Changed);
        }

        public void Handle(AllowanceSpentEvent evt)
        {
            var trans = new TransactionDataContract
            {
                // todo: transaction id Id = evt.EventId,
                IsSpend = true,
                Amount = evt.Amount,
                Description = evt.Description,
                MinionId = evt.AggregateId.Id,
                TransactionDate = evt.Date,
                AsCash = evt.FromCash,
            };

            this.repository.Save(trans);
            this.NotifyReadModelChange(trans, ReadModelChange.Changed);
        }
    }
}
