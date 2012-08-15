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
    using MonoKit.Data;
    using MonoKit.Domain;
    using MyMinions.Domain.Data;

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

        public static TransactionId NewId()
        {
            return new TransactionId(Guid.NewGuid());
        }
    }

    public sealed class ScheduledDeedId : Identity
    {
        public ScheduledDeedId(Guid id)
            : base(id)
        {
        }

        public static ScheduledDeedId NewId()
        {
            return new ScheduledDeedId(Guid.NewGuid());
        }
    }

    public sealed class PerformedDeedId : Identity
    {
        public PerformedDeedId(Guid id)
            : base(id)
        {
        }

        public static PerformedDeedId NewId()
        {
            return new PerformedDeedId(Guid.NewGuid());
        }
    }






    public class Minion : AggregateRoot<MinionDataContract>
    {
        public static string AggregateTypeId = "Minion";

        public Minion()
        {
        }

        public override ISnapshot GetSnapshot()
        {
            var snapshot = this.InternalState;

            snapshot.Id = this.Identity.Id;
            snapshot.Version = this.Version;

            return snapshot;
        }

        public void Execute(ChangeNameCommand command)
        {
            this.RaiseEvent(new NameChangedEvent { Name = command.Name, });
        }

        public void Apply(NameChangedEvent evt)
        {
            this.InternalState.MinionName = evt.Name;
        }

        public void Execute(ChangeAllowanceCommand command)
        {
            this.RaiseEvent(new AllowanceChangedEvent { Allowance = command.Allowance, });
        }

        public void Apply(AllowanceChangedEvent evt)
        {
            this.InternalState.WeeklyAllowance = Math.Round(evt.Allowance, MidpointRounding.AwayFromZero);
        }

        public void Execute(DeleteCommand command)
        {
            this.RaiseEvent(new DeletedEvent());
        }

        public void Apply(DeletedEvent evt)
        {
            this.InternalState.Deleted = true;
        }

        public void Execute(EarnAllowanceCommand command)
        {
            this.RaiseEvent(new AllowanceEarntEvent
            {
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
            this.RaiseEvent(new AllowanceSpentEvent
            {
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

        public void Execute(ScheduleDeed command)
        {
            this.RaiseEvent(new DeedScheduledEvent
            {
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
            this.RaiseEvent(new DeedUnscheduledEvent
            {
                DeedId = command.DeedId,
            });
        }

        public void Apply(DeedUnscheduledEvent evt)
        {
        }

        public void Execute(PerformDeed command)
        {
            this.RaiseEvent(new DeedPerformedEvent
            {
                DeedId = command.DeedId,
                Date = command.Date,
            });
        }

        public void Apply(DeedPerformedEvent evt)
        {
        }

        public void Execute(ResetDeed command)
        {
            this.RaiseEvent(new DeedResetEvent
            {
                Id = command.Id,
            });
        }

        public void Apply(DeedResetEvent evt)
        {
        }
    }
}

