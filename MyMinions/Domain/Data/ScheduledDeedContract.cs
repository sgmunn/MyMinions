//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScheduledDeedContract.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;

    public class ScheduledDeedContract : IDataModel
    {
        public ScheduledDeedContract()
        {
            this.Id = Guid.NewGuid();
        }
        
        [Ignore]
        public IUniqueIdentity Identity 
        {
            get
            {
                return new ScheduledDeedId(this.Id);
            }
        }

        [PrimaryKey]
        public Guid Id { get; set; } 

        [Indexed]
        public Guid MinionId { get; set; }
        
        [Indexed]
        public Guid DeedId { get; set; }

        public string Description { get; set; }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }

        public bool Sunday { get; set; }
    }
}
