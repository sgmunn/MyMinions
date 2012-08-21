//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TransactionContract.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;
    using MyMinions.Domain.Data;

    public class TransactionContract : IDataModel
    {
        public TransactionContract()
        {
            this.Id = Guid.NewGuid();
        }

        [Ignore]
        public IUniqueIdentity Identity 
        {
            get
            {
                return new TransactionId(this.Id);
            }
        }

        [PrimaryKey]
        public Guid Id { get; set; } 

        [Indexed]
        public Guid MinionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public bool AsCash { get; set; }

        public bool IsSpend { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Amount, Description);
        }
    }
}

