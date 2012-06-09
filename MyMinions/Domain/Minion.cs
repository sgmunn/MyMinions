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
using System;
using MonoKit.Domain;
using MyMinions.Domain.Data;
using MonoKit.Domain.Commands;
using MonoKit.Domain.Events;
using MonoKit.Domain.Data;


namespace MyMinions.Domain
{
    public class Minion : AggregateRoot<MinionDataContract>
    {
        public static string AggregateTypeId = "Minion";

        public Minion()
        {
        }

        public void Execute(ChangeNameCommand command)
        {
            this.NewEvent(new NameChangedEvent { Name = command.Name, });
        }

        public void Apply(NameChangedEvent @event)
        {
            this.InternalState.MinionName = @event.Name;
        }

        public void Execute(ChangeAllowanceCommand command)
        {
            this.NewEvent(new AllowanceChangedEvent { Allowance = command.Allowance, });
        }

        public void Apply(AllowanceChangedEvent @event)
        {
            this.InternalState.WeeklyAllowance = Math.Round(@event.Allowance, MidpointRounding.AwayFromZero);
        }

        public void Execute(DeleteCommand command)
        {
            this.NewEvent(new DeletedEvent());
        }

        public void Apply(DeletedEvent @event)
        {
            this.InternalState.Deleted = true;
        }

        public void Execute(EarnAllowanceCommand command)
        {
            this.NewEvent(new AllowanceEarntEvent
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
            });
        }

        public void Apply(AllowanceEarntEvent @event)
        {
            this.InternalState.CurrentBalance += @event.Amount;
        }

        public void Execute(SpendAllowanceCommand command)
        {
            this.NewEvent(new AllowanceSpentEvent
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
            });
        }

        public void Apply(AllowanceSpentEvent @event)
        {
            this.InternalState.CurrentBalance -= @event.Amount;
        }
    }

    public class ChangeNameCommand : CommandBase
    {
        public string Name { get; set; }
    }

    public class NameChangedEvent : EventBase
    {
        public string Name { get; set; }
    }

    public class ChangeAllowanceCommand : CommandBase
    {
        public decimal Allowance { get; set; }
    }

    public class AllowanceChangedEvent : EventBase
    {
        public decimal Allowance { get; set; }
    }

    public class EarnAllowanceCommand : CommandBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class AllowanceEarntEvent : EventBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class SpendAllowanceCommand : CommandBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class AllowanceSpentEvent : EventBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class MinionReadModelBuilder : ReadModelBuilder
    {
        private readonly IMinionRepository repository;

        public MinionReadModelBuilder(IMinionRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeletedEvent @event)
        {
            // todo: delete notifications
//            this.SaveReadModel();
            this.repository.DeleteId(@event.AggregateId);
        }

        protected override void DoSaveReadModel(IReadModel readModel)
        {
            //throw new System.NotImplementedException();
        }
    }

    public class TransactionReadModelBuilder : ReadModelBuilder
    {
        private readonly ITransactionRepository repository;

        public TransactionReadModelBuilder(ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DeletedEvent @event)
        {
            // todo: delete notifications
            this.repository.DeleteAllForMinion(@event.AggregateId);
        }

        public void Handle(AllowanceEarntEvent @event)
        {
            var trans = new TransactionDataContract
            {
                IsSpend = false,
                Amount = @event.Amount,
                Description = @event.Description,
                MinionId = @event.AggregateId,
                TransactionDate = @event.Date,
            };

            this.SaveReadModel(trans);
        }

        public void Handle(AllowanceSpentEvent @event)
        {
            var trans = new TransactionDataContract
            {
                IsSpend = true,
                Amount = @event.Amount,
                Description = @event.Description,
                MinionId = @event.AggregateId,
                TransactionDate = @event.Date,
            };

            this.SaveReadModel(trans);
        }

        protected override void DoSaveReadModel(IReadModel readModel)
        {
            this.repository.Save((TransactionDataContract)readModel);
        }
    }
}

