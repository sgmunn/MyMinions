//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionContext.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain
{
    using System;
    using MonoKit.Data.SQLite;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data.SQLite;
    using MonoKit.Domain.Data;
    using MyMinions.Domain.Builders;
    using MonoKit.Domain;

    public class MinionContext : SqlDomainContext
    {
        public MinionContext(SQLiteConnection connection, IDomainEventBus eventBus) 
            : base(connection, GetManifest(connection), null, eventBus)
        {
            this.Bootstrap();
        }

        private static IAggregateManifestRepository GetManifest(SQLiteConnection connection)
        {
            return new SqlAggregateManifestRepository(connection);
        }

        private void Bootstrap()
        {
            this.RegisterSnapshot<MinionAggregate>(c => new SnapshotRepository<MinionContract>(this.Connection));

            this.RegisterBuilder<MinionAggregate>((c) =>
            {
                return new TransactionReadModelBuilder(new TransactionRepository(this.Connection));
            });

            this.RegisterBuilder<MinionAggregate>((c) =>
            {
                return new MinionReadModelBuilder(new SqlRepository<MinionContract>(this.Connection));
            });

            this.RegisterBuilder<MinionAggregate>((c) =>
            {
                return new ScheduledDeedReadModelBuilder(new ScheduledDeedRepository(this.Connection));
            });

            this.RegisterBuilder<MinionAggregate>((c) =>
            {
                return new PerformedDeedReadModelBuilder(new PerformedDeedRepository(this.Connection));
            });
        }
    }
}

