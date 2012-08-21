using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoKit.UI;
using MonoKit.Data;

using MyMinions.Domain.Data;
using System.Threading;
using MonoKit.DataBinding;
using System.Collections.Generic;
using MonoKit;
using MonoKit.UI.Controls;

namespace RewardSquirrel
{
    public partial class TransactionsViewController : UIViewController
    {
        private TableViewSource tableSource;
        private TableViewSection<TransactionContract> section;
        private int recipientId;
        
        public TransactionsViewController(int recipientId) : base ("TransactionsViewController", null)
        {
            this.recipientId = recipientId;
            this.tableSource = new TableViewSource();
        }
        
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            
            // Release any cached data, images, etc that aren't in use.
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.tableSource.TableView = this.tableView;

            // todo: section to be created in constructor -- that means that we hold a reference to it and sections.clear can't dispose of the section
            
            if (this.section == null)
            {
                this.section = new TableViewSection<TransactionContract>(this.tableSource,
                    new UIViewDefinition<StringElementTableViewCell, TransactionContract>(this.Bind) { Param = UITableViewCellStyle.Value1 }           
                    );
            }
            
            this.LoadTransactions();
            
            // Perform any additional setup after loading the view, typically from a nib.
        }
        
        private void Bind(UIView view, object data)
        {
            ((StringElementTableViewCell)view).SetBinding("Text", data, "Description");
            ((StringElementTableViewCell)view).SetBinding("Value", data, new Binding("Amount") {Converter = new DecimalToStringConverter()});
        }
        
        public override void ViewWillUnload()
        {
            this.tableSource.TableView = null;
            this.tableSource.ClearData();
            this.section = null;
            base.ViewWillUnload();
        }
        
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            
            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;
            
            ReleaseDesignerOutlets();
        }
        
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
         
        partial void closeButonClicked(MonoTouch.Foundation.NSObject sender)
        {
            this.DismissViewController(true, () => {});
        }
        
        private void LoadTransactions()
        {
            var loadThread = new Thread(this.LoadTransactionsAsync);
            loadThread.Start();
        }
        
        private void LoadTransactionsAsync()
        {
//            var repo = RewardDatabase.Main.NewRepository<Transaction>();
//            using (repo)
//            {
//                var transactions = repo.GetAll().Where(x => x.RecipientId == this.recipientId).OrderByDescending(x => x.TransactionDate);
//                
//                this.InvokeOnMainThread(() =>{
//                    this.section.BeginUpdate();
//                    this.section.AddRange(transactions);
//                    this.section.EndUpdate();
//                });                 
//            }
        }
    }
}

