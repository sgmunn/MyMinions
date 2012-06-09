
namespace MyMinions
{
    using System;
    using System.Drawing;
    using MonoTouch.UIKit;
    using MonoKit;
    using MonoKit.Domain;
    using MyMinions.Domain;
    using MonoKit.UI.Controls;
    using MonoKit.Reactive.Linq;

    public partial class SpendViewController : UIViewController
    {
        private readonly IDomainContext context;
        private readonly Guid minionId;
        private UIDateField dateField;

        public SpendViewController(IDomainContext context, Guid minionId) : base ("SpendViewController", null)
        {
            this.context = context;
            this.minionId = minionId;
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

            // todo: date field border for when not inside a table view cell
            this.dateField = new UIDateField(new RectangleF(20, 172, 280, 31));

            this.View.AddSubview(this.dateField);
            this.amount.KeyboardType = UIKeyboardType.DecimalPad;

            this.amount.BecomeFirstResponder();
        }
        
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            
            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;
            
            ReleaseDesignerOutlets();

            if (this.dateField != null)
            {
                this.dateField.Dispose();
                this.dateField = null;
            }
        }

        partial void doneButtonClicked(MonoTouch.Foundation.NSObject sender)
        {
            decimal amt = 0;
            try
            {
                amt = Convert.ToDecimal(this.amount.Text);
            }
            catch
            {
                amt = 0; 
            }

            var subscription = Observable.Start(
                () => 
                 {
                    var cmd = this.context.NewCommandExecutor<Minion>();
                    cmd.Execute(new SpendAllowanceCommand 
                    { 
                        AggregateId = this.minionId, 
                        Date = this.dateField.Date,
                        Amount = amt, 
                        Description = this.description.Text 
                    });
                }).Subscribe();

            this.DismissModalViewControllerAnimated(true);
        }
        
        partial void cancelButtonClicked(MonoTouch.Foundation.NSObject sender)
        {
            this.DismissModalViewControllerAnimated(true);
        }
    }
}

