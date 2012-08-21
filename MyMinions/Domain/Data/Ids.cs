//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Ids.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;

    public sealed class MinionId : Identity
    {
        public MinionId(Guid id)
            : base(id)
        {
        }

        public static MinionId NewId()
        {
            return new MinionId(Guid.NewGuid());
        }

        public static implicit operator MinionId(Guid id)
        {
            return new MinionId(id);
        }
    }

    public sealed class TransactionId : Identity
    {
        public TransactionId(Guid id)
            : base(id)
        {
        }
    }

    public sealed class ScheduledDeedId : Identity
    {
        public ScheduledDeedId(Guid id)
            : base(id)
        {
        }
    }

    public sealed class PerformedDeedId : Identity
    {
        public PerformedDeedId(Guid id)
            : base(id)
        {
        }
    }
}

