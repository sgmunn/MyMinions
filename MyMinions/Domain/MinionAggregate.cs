//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionAggregate.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain
{
    using System;
    using MonoKit.Domain;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data;

    public class MinionAggregate : AggregateRoot<MinionContract>
    {
        public MinionAggregate()
        {
        }

        public override ISnapshot GetSnapshot()
        {
            var snapshot = this.InternalState;

            snapshot.Id = this.Identity.Id;
            snapshot.Version = this.Version;

            return snapshot;
        }

        //
        // basic info
        //

        public void Execute(ChangeNameCommand command)
        {
            this.RaiseEvent(command.AggregateId, new NameChangedEvent { Name = command.Name, });
        }

        public void Apply(NameChangedEvent evt)
        {
            this.InternalState.MinionName = evt.Name;
        }

        //
        // eanring and spending
        //

        public void Execute(ChangeWeeklyAllowanceCommand command)
        {
            this.RaiseEvent(command.AggregateId, new WeeklyAllowanceChangedEvent { Allowance = command.Allowance, });
        }

        public void Apply(WeeklyAllowanceChangedEvent evt)
        {
            this.InternalState.WeeklyAllowance = Math.Round(evt.Allowance, MidpointRounding.AwayFromZero);
        }

        public void Execute(DeleteCommand command)
        {
            this.RaiseEvent(command.AggregateId, new DeletedEvent());
        }

        public void Apply(DeletedEvent evt)
        {
            this.InternalState.Deleted = true;
        }

        public void Execute(EarnAllowanceCommand command)
        {
            this.RaiseEvent(command.AggregateId, new AllowanceEarntEvent
            {
                Transactionid = command.Transactionid,
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                AsCash = command.AsCash,
            });
        }

        public void Apply(AllowanceEarntEvent evt)
        {
            this.InternalState.CashBalance += evt.AsCash ? evt.Amount : 0;
            this.InternalState.StashedBalance += evt.AsCash ? 0 : evt.Amount;
        }

        public void Execute(SpendAllowanceCommand command)
        {
            this.RaiseEvent(command.AggregateId, new AllowanceSpentEvent
            {
                Transactionid = command.Transactionid,
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                FromCash = command.FromCash,
            });
        }

        public void Apply(AllowanceSpentEvent evt)
        {
            this.InternalState.CashBalance -= evt.FromCash ? evt.Amount : 0;
            this.InternalState.StashedBalance -= evt.FromCash ? 0 : evt.Amount;
        }

        //
        // deeds
        //

        public void Execute(ScheduleDeed command)
        {
            this.RaiseEvent(command.AggregateId, new DeedScheduledEvent
            {
                ScheduledDeedId = command.ScheduledDeedId,
                DeedId = command.DeedId,
                Description = command.Description,
                Monday = command.Monday,
                Tuesday = command.Tuesday,
                Wednesday = command.Wednesday,
                Thursday = command.Thursday,
                Friday = command.Friday,
                Saturday = command.Saturday,
                Sunday = command.Sunday,
            });
        }

        public void Apply(DeedScheduledEvent evt)
        {
        }

        public void Execute(UnscheduleDeed command)
        {
            this.RaiseEvent(command.AggregateId, new DeedUnscheduledEvent
            {
                ScheduledDeedId = command.ScheduledDeedId,
            });
        }

        public void Apply(DeedUnscheduledEvent evt)
        {
        }

        public void Execute(PerformDeed command)
        {
            this.RaiseEvent(command.AggregateId, new DeedPerformedEvent
            {
                PerformedDeedId = command.PerformedDeedId,
                DeedId = command.DeedId,
                Date = command.Date,
                Description = command.Description,
            });
        }

        public void Apply(DeedPerformedEvent evt)
        {
        }

        public void Execute(ResetDeed command)
        {
            this.RaiseEvent(command.AggregateId, new DeedResetEvent
            {
                PerformedDeedId = command.PerformedDeedId,
            });
        }

        public void Apply(DeedResetEvent evt)
        {
        }
    }
}

