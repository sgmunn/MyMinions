using MyMinions.Domain.Data;

namespace MyMinions
{
    using System;
    using System.Drawing;
    
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using MonoKit;
    using MonoKit.Domain;
    using MyMinions.Domain;
    using MonoKit.Reactive.Linq;
    using MonoKit.UI.Controls;
    
    public partial class EarnViewController : UIViewController
    {
        private readonly IDomainContext context;

        private readonly MinionId minionId;

        private UIDateField dateField;

        private decimal defaultAllowance;

        public EarnViewController(IDomainContext context, MinionId minionId, decimal defaultAllowance) : base ("EarnViewController", null)
        {
            this.context = context;
            this.minionId = minionId;
            this.defaultAllowance = defaultAllowance;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.amount.Text = string.Format("{0:0.00}", this.defaultAllowance);
            this.description.Text = "Weekly Allowance";

            // todo: date field border for when not inside a table view cell
            this.dateField = new UIDateField(new RectangleF(20, 172, 280, 31));

            this.View.AddSubview(this.dateField);
            this.amount.KeyboardType = UIKeyboardType.DecimalPad;

            this.description.BecomeFirstResponder();
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

        partial void cancelButtonClicked(NSObject sender)
        {
            this.DismissModalViewControllerAnimated(true);
        }

        partial void earnButtonClicked(NSObject sender)
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
                    var cmd = this.context.NewCommandExecutor<MinionAggregate>();
                    cmd.Execute(new EarnAllowanceCommand 
                    { 
                        AggregateId = this.minionId, 
                        Date = this.dateField.Date,
                        Amount = amt, 
                        Description = this.description.Text 
                    });
                }).Subscribe();

            this.DismissModalViewControllerAnimated(true);
        }
    }
}

