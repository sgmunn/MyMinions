using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MyMinions.Domain.Data;
using MonoKit.UI;
using MonoKit.DataBinding;
using System.Threading;
using System.Linq;

namespace RewardSquirrel
{
    public partial class EarnViewController : UIViewController
    {
        private TableViewSource tableSource;
        private TableViewSection<MinionTaskDataContract> section;
        private int recipientId;

        public EarnViewController(int recipientId) : base ("EarnViewController", null)
        {
            this.tableSource = new TableViewSource();
            this.recipientId = recipientId;
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
            
            // Perform any additional setup after loading the view, typically from a nib.
            if (this.section == null)
            {
                this.section = new TableViewSection<MinionTaskDataContract>(this.tableSource);
                this.section.Header = "Header";
            }
            
            this.tableSource.TableView = this.tableView;
            this.LoadTasks();
        }
                
        public override void ViewWillUnload()
        {
            this.tableSource.TableView = null;
            this.tableSource.ClearData();
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
        
        partial void cancelButtonClicked(MonoTouch.Foundation.NSObject sender)
        {
            this.DismissModalViewControllerAnimated(true);
        }
                
        private void LoadTasks()
        {
            var loadThread = new Thread(this.LoadTasksAsync);
            loadThread.Start();
        }
        
        private void LoadTasksAsync()
        {
            // grouped or with headings ??
            
//            var repo = RewardDatabase.Main.NewRepository<RecipientTask>();
//            using (repo)
//            {
//                var startDate = DataLogic.GetStartOfWeekFromDate(DateTime.Today);
//                var endDate = startDate.AddDays(7);
//                var tasks = repo.GetAll().Where(x => x.RecipientId == this.recipientId && x.TaskDate >= startDate && x.TaskDate < endDate).
//                    OrderBy(x => x.Description).OrderBy(x => x.TaskDate).ToList();
//                
//                this.InvokeOnMainThread(() => {
//                    this.section.AddRange(tasks);
//                });                 
//            }
        }
    }
}

