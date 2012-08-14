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

namespace MyMinions.Domain.Data
{
    using System;
    using System.ComponentModel;
    using MonoKit.Data.SQLite;
    using MonoKit.Domain;

    [MonoTouch.Foundation.Preserve(AllMembers = true)]
    public class MinionDataContract : ISnapshot
    {
        public MinionDataContract()
        {
            this.MinionName = "New Minion";
        }
        
        Identity IReadModel.Id { get; set; }

        [PrimaryKey]
        public Guid Id 
        {
            get
            {
                return ((IReadModel)this).Id;
            }

            set 
            {
                ((IReadModel)this).Id = (MinionId)value;
            }
        }

        public int Version { get; set; }

        public bool Deleted { get; set; }

        public string MinionName { get; set; }

        public decimal CashBalance { get; set; }

        public decimal StashedBalance { get; set; }

        public decimal WeeklyAllowance { get; set; }
    }

    [MonoTouch.Foundation.Preserve(AllMembers = true)]
    public class ScheduledDeedDataContract : IReadModel
    {
        public ScheduledDeedDataContract()
        {
            this.Id = Guid.NewGuid();
            this.Description = "New Deed";
        }
        
        Identity IReadModel.Id { get; set; }

        [PrimaryKey]
        public Guid Id 
        {
            get
            {
                return ((IReadModel)this).Id.Id;
            }

            set 
            {
                ((IReadModel)this).Id = new ScheduledDeedId(value);
            }
        }

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

    [MonoTouch.Foundation.Preserve(AllMembers = true)]
    public class PerformedDeedDataContract : IReadModel
    {
        public PerformedDeedDataContract()
        {
            this.Id = Guid.NewGuid();
        }

        Identity IReadModel.Id { get; set; }

        [PrimaryKey]
        public Guid Id 
        {
            get
            {
                return ((IReadModel)this).Id.Id;
            }

            set 
            {
                ((IReadModel)this).Id = new PerformedDeedId(value);
            }
        }

        public int Version { get; set; }
        
        [Indexed]
        public Guid MinionId { get; set; }
        
        [Indexed]
        public Guid DeedId { get; set; }

        public DateTime Date { get; set; }
    }
}

