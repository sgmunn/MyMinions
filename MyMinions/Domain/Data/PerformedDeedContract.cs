//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PerformedDeedContract.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;

    public class PerformedDeedContract : IDataModel
    {
        public PerformedDeedContract()
        {
            this.Id = Guid.NewGuid();
        }

        [Ignore]
        public IUniqueIdentity Identity 
        {
            get
            {
                return new PerformedDeedId(this.Id);
            }
        }

        [PrimaryKey]
        public Guid Id { get; set; } 

        public int Version { get; set; }
        
        [Indexed]
        public Guid MinionId { get; set; }
        
        [Indexed]
        public Guid DeedId { get; set; }

        public DateTime Date { get; set; }
    }
}
