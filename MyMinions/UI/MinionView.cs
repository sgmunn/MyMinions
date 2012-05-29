using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Linq;
using MonoKit.UI;
using MonoKit.DataBinding;
using MyMinions.Domain.Data;

namespace MyMinions.UI
{
    [Register("MinionView")]
    public partial class MinionView : UIView
    {
        private MinionDataContract minion;
        private TableViewSource tableSource;
 //       private TableViewSection<RecipientTask> section;

        public MinionView(IntPtr handle) : base(handle)
        {
  //          this.tableSource = new TableViewSource();
        }
        
        public MinionDataContract Minion
        {
            get
            {
                return this.minion;
            }
            
            set
            {
                // todo: if we bound some properties to the recipient then we don't need to hook into view changing ...
                this.minion = value;
                //this.minionNameLabel.Text = value.RecipientName;
                
                this.minionNameLabel.ClearBindings();
                this.minionNameLabel.SetBinding("Text", this.Minion, "MinionName");
                
                //this.transactionsButton.SetTitle(value.CurrentBalance.ToString("C"), UIControlState.Normal);
                //this.LoadRecipient();
            }
        }
        
//        public UITableView TasksTable
//        {
//            get
//            {
//                return this.currentTasksTableView;
//            }
//        }

        public UIViewController Controller
        {
            get;
            set;
        }

//        partial void transactionsButtonClicked(MonoTouch.Foundation.NSObject sender)
//        {
//            var tranactions = new TransactionsViewController(this.Recipient.RecipientId);
//            tranactions.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
//            this.Controller.PresentViewController(tranactions, true, () => {});
//        }
//        
//        partial void earnButtonClicked(MonoTouch.Foundation.NSObject sender)
//        {
//            var earnView = new EarnViewController(this.Recipient.RecipientId);
//            this.Controller.PresentModalViewController(earnView, true);
//        }
//        
//        partial void spendButtonClicked(NSObject sender)
//        {
//            var spendView = new SpendViewController(this.Recipient);
//            
//            this.Controller.PresentModalViewController(spendView, true);
//            
//            //this.transactionsButton.SetTitle(this.Recipient.CurrentBalance.ToString("C"), UIControlState.Normal);
//        }
//        
//        private void LoadRecipient()
//        {
//            if (this.section == null)
//            {
//                this.section = new TableViewSection<RecipientTask>(this.tableSource);
//            
//                this.tableSource.TableView = this.TasksTable;
//                this.LoadTasks();
//            }
//
//        }
//        
//                
//        private void LoadTasks()
//        {
//            var loadThread = new Thread(this.LoadTasksAsync);
//            loadThread.Start();
//        }
//        
//        private void LoadTasksAsync()
//        {
//            DataLogic.EnsureTasksForDate(DateTime.Today, this.recipient);
//            
//            var repo = RewardDatabase.Main.NewRepository<RecipientTask>();
//            using (repo)
//            {
//                var date = DateTime.Today;
//                var tasks = repo.GetAll().Where(x => x.RecipientId == this.Recipient.RecipientId && x.TaskDate == date).
//                    OrderBy(x => x.Description).ToList();
//                
//                this.InvokeOnMainThread(() => {
//                    this.section.AddRange(tasks);
//                });                 
//            }
//        }

    }
}

