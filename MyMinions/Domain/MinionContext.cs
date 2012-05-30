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
    using MonoKit.Data.SQLite;
    using MyMinions.Domain.Data;
    using MonoKit.Domain.Data.SQLite;
    using MonoKit.Domain.Data;
    using MonoKit.Domain;
    using System.IO;

    public class MinionContext : SQLiteDomainContext
    {
        public MinionContext(SQLiteConnection connection, IEventStoreRepository eventStore, IDomainEventBus eventBus) : base(connection, eventStore, eventBus)
        {
            this.Bootstrap();
        }

        private void Bootstrap()
        {
            this.RegisterSnapshot<Minion>(c => new SnapshotRepository<MinionDataContract>(this.Connection));
            // todo: register a builder to delete deleted minions
        }
    }

    public class MinionDB : SQLiteConnection
    {
        private static MinionDB mainDatabase = new MinionDB();

        public static string MinionDatabasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyMinions.db");
        }

        public static MinionDB Main
        {
            get
            {
                return mainDatabase;
            }
        }

        protected MinionDB() : base(MinionDatabasePath())
        {
            this.CreateTable<MinionDataContract>();
        }
    }
}

