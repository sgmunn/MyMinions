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
    using MonoKit.Domain;

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
        public bool AsCash { get; set; }
    }

    public class AllowanceEarntEvent : EventBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool AsCash { get; set; }
    }

    public class SpendAllowanceCommand : CommandBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool FromCash { get; set; }
    }

    public class AllowanceSpentEvent : EventBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool FromCash { get; set; }
    }

    public class ScheduleDeed : CommandBase
    {
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
        public Guid DeedId { get; set; }
    }

    public class DeedUnscheduledEvent : EventBase
    {
        public Guid DeedId { get; set; }
    }

    public class PerformDeed : CommandBase
    {
        public Guid DeedId { get; set; }
        public DateTime Date { get; set; }
    }

    public class DeedPerformedEvent : EventBase
    {
        public Guid DeedId { get; set; }
        public DateTime Date { get; set; }
    }

    public class ResetDeed : CommandBase
    {
        public Guid Id { get; set; }
    }

    public class DeedResetEvent : EventBase
    {
        public Guid Id { get; set; }
    }
}

