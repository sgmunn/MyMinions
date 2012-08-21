//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionContract.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;
    using MonoKit.Domain.Data;

    public class MinionContract : ISnapshot
    {
        [Ignore]
        public IUniqueIdentity Identity 
        {
            get
            {
                return new MinionId(this.Id);
            }
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        public int Version { get; set; }

        public bool Deleted { get; set; }

        public string MinionName { get; set; }

        public decimal CashBalance { get; set; }

        public decimal StashedBalance { get; set; }

        public decimal WeeklyAllowance { get; set; }
    }
}

