//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CommandsEvents.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain
{
    using System;
    using MonoKit.Domain;
    using MonoKit.Data;
    using MyMinions.Domain.Data;

    public class CommandBase : IAggregateCommand
    {
        public CommandBase()
        {
            this.CommandId = Guid.NewGuid();
        }

        public IUniqueIdentity AggregateId { get; set; }

        public Guid CommandId { get; set; }
    }

    public class EventBase : IAggregateEvent
    {
        public EventBase()
        {
            this.EventId = Guid.NewGuid();
        }

        public IUniqueIdentity Identity { get; set; }

        public Guid IdentityId
        {
            get
            {
                return this.Identity.Id;
            }

            set
            {
                this.Identity = new Identity(value);
            }
        }

        public Guid EventId { get; set; }

        public int Version { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class DeleteCommand : CommandBase
    {
    }

    public class DeletedEvent : EventBase
    {
    }

    //
    // Basic info
    //

    public class ChangeNameCommand : CommandBase
    {
        public string Name { get; set; }
    }

    public class NameChangedEvent : EventBase
    {
        public string Name { get; set; }
    }

    public class ChangeWeeklyAllowanceCommand : CommandBase
    {
        public decimal Allowance { get; set; }
    }

    public class WeeklyAllowanceChangedEvent : EventBase
    {
        public decimal Allowance { get; set; }
    }

    //
    // deeds
    //

    public class ScheduleDeed : CommandBase
    {
        public ScheduleDeed()
        {
            this.ScheduledDeedId = new ScheduledDeedId(Guid.NewGuid());
        }

        public ScheduledDeedId ScheduledDeedId { get; set; }
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

    public class DeedScheduledEvent : EventBase
    {
        public ScheduledDeedId ScheduledDeedId { get; set; }
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

    public class UnscheduleDeed : CommandBase
    {
        // todo: improve - this should be stronly typed id ?
        public ScheduledDeedId ScheduledDeedId { get; set; }
    }

    public class DeedUnscheduledEvent : EventBase
    {
        public ScheduledDeedId ScheduledDeedId { get; set; }
    }

    public class PerformDeed : CommandBase
    {
        public PerformDeed()
        {
            this.PerformedDeedId = new PerformedDeedId(Guid.NewGuid());
        }

        public PerformedDeedId PerformedDeedId { get; set; }
        public Guid DeedId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class DeedPerformedEvent : EventBase
    {
        public PerformedDeedId PerformedDeedId { get; set; }
        public Guid DeedId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class ResetDeed : CommandBase
    {
        public PerformedDeedId PerformedDeedId { get; set; }
    }

    public class DeedResetEvent : EventBase
    {
        public PerformedDeedId PerformedDeedId { get; set; }
    }

    //
    // earning and spending
    //

    public class EarnAllowanceCommand : CommandBase
    {
        public Guid Transactionid { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool AsCash { get; set; }
    }

    public class AllowanceEarntEvent : EventBase
    {
        public Guid Transactionid { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool AsCash { get; set; }
    }

    public class SpendAllowanceCommand : CommandBase
    {
        public Guid Transactionid { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool FromCash { get; set; }
    }

    public class AllowanceSpentEvent : EventBase
    {
        public Guid Transactionid { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool FromCash { get; set; }
    }
}

