//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DB.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Domain.Data
{
    using System;
    using System.IO;
    using MonoKit.Domain.Data.SQLite;
    using MonoKit.Data.SQLite;

    public sealed class DB : SQLiteConnection
    {
        public static string MinionDatabasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyMinions.db");
        }

        public static DB Main
        {
            get
            {
                // Lazy static constructor
                // http://www.yoda.arachsys.com/csharp/singleton.html
                return Constructor.Instance;
            }
        }

        private DB() : base(MinionDatabasePath())
        {
            this.CreateTable<AggregateManifest>();
            this.CreateTable<MinionContract>();
            this.CreateTable<TransactionDataContract>();
            this.CreateTable<ScheduledDeedContract>();
            this.CreateTable<PerformedDeedContract>();
        }

        private class Constructor
        {
            internal static readonly DB Instance = new DB();

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Constructor()
            {
            }
        }
    }
}

