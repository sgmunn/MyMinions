
using MonoKit.Data;
using MyMinions.Domain;

namespace MyMinions.UI
{
    using System;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using System.Linq;
    using MonoKit;
    using MonoKit.UI;
    using MyMinions.Domain.Data;
    using MonoKit.Domain;
    using MonoKit.Reactive.Disposables;
    using MonoKit.Reactive.Linq;
    using System.Collections.Generic;
    using MonoKit.Reactive;

    public partial class MinionView : UIView
    {
        private readonly CompositeDisposable lifetime;
        private readonly TableViewSource tableSource;
        private TableViewSection<TransactionDataContract> section;

        private MinionContract minion;

        public MinionView(IntPtr handle) : base(handle)
        {
            this.lifetime = new CompositeDisposable();
            this.tableSource = new TableViewSource();
        }
        
        public MinionContract Minion
        {
            get
            {
                return this.minion;
            }
            
            set
            {
                this.SetupTable();

                if (value != this.minion)
                {
                    this.lifetime.Clear();

                    var bus = this.Context.EventBus;
                    this.lifetime.Add(bus.ObserveOnMainThread().Subscribe(this.OnNextReadModel));
                }

                this.minion = value;
                this.MinionUpdated(value);
                this.LoadTransactionsAsync();
            }
        }

        public IDomainContext Context
        {
            get;
            set;
        }

        public ITransactionRepository Repository
        {
            get;
            set;
        }

        public UIViewController Controller
        {
            get;
            set;
        }

        partial void earnButtonClick(NSObject sender)
        {
            var earnView = new EarnViewController(this.Context, this.Minion.Id, this.Minion.WeeklyAllowance);
            this.Controller.PresentModalViewController(earnView, true);
        }

        partial void spendButtonClick(NSObject sender)
        {
            var spendView = new SpendViewController(this.Context, this.Minion.Id);
            this.Controller.PresentModalViewController(spendView, true);
        }

        private void OnNextReadModel(IDataModelEvent readModelChange)
        {
            if (readModelChange.Identity is TransactionId)
            {
               // this.TransactionUpdated((TransactionDataContract)readModel);
            }

            if (readModelChange.Identity is MinionId)
            {
              //  this.MinionUpdated((MinionDataContract)readModel);
            }
        }

        private void TransactionUpdated(TransactionDataContract transaction)
        {
            this.section.Insert(0, transaction);
        }

        private void MinionUpdated(MinionContract minion)
        {
            if (minion.Id == this.Minion.Id)
            {
                // by-pass property, bit of a hack
                this.minion = minion;
                this.minionNameLabel.Text = minion.MinionName;
                this.transactionButton.SetTitle(string.Format("{0}", minion.CashBalance), UIControlState.Normal);
            }
        }

        private void SetupTable()
        {
            if (this.section == null)
            {
                this.section = new TableViewSection<TransactionDataContract>(this.tableSource);
            
                this.tableSource.TableView = this.transactionTable;
            }
        }

        private void LoadTransactionsAsync()
        {
            var subscription = Observable.Start<IEnumerable<TransactionDataContract>>(
                () =>
                {
                    return this.Repository.GetAllForMinion(this.Minion.Id).OrderByDescending(x => x.TransactionDate);
                })
                .ObserveOnMainThread().Subscribe((transactions) =>
                    {
                        this.LoadTransactions(transactions);
                    });

            // make sure to add to lifetime so that if we navigate away 
            this.lifetime.Add(subscription);
        }

        private void LoadTransactions(IEnumerable<TransactionDataContract> transactions)
        {
            this.section.BeginUpdate();
            this.section.Clear();
            foreach (var transaction in transactions)
            {
                this.section.Add(transaction);
            }
                
            this.section.EndUpdate();
        }
    }
}

