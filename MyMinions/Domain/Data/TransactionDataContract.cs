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
using MyMinions.Domain.Data;

namespace MyMinions.Domain.Data
{
    using System;
    using MonoKit.Data;
    using MonoKit.Data.SQLite;
    using MonoKit.Domain;

    public class TransactionDataContract : IDataModel
    {
        public TransactionDataContract()
        {
            this.Id = Guid.NewGuid();
        }

        [Ignore]
        public IUniqueIdentity Identity 
        {
            get
            {
                return new TransactionId(this.Id);
            }
        }

        [PrimaryKey]
        public Guid Id { get; set; } 

        [Indexed]
        public Guid MinionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public bool AsCash { get; set; }

        public bool IsSpend { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Amount, Description);
        }
    }
}

