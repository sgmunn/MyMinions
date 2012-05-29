using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using MyMinions.Domain.Data;

namespace RewardSquirrel
{
    [Register("UITextFieldNoPaste")]
    public class UITextFieldNoPaste : UITextField
    {
        public UITextFieldNoPaste(IntPtr handle): base(handle)
        {
            
        }
        
        public override bool CanPerform(MonoTouch.ObjCRuntime.Selector action, NSObject withSender)
        {
            if (action == new Selector("paste:"))
            {
                return false;
            }
            
            return base.CanPerform(action, withSender);
        }
    }
    
    public partial class SpendViewController : UIViewController
    {
        private MinionDataContract recipient;
        private DateTime transactionDate;
        
        public SpendViewController(MinionDataContract recipient) : base ("SpendViewController", null)
        {
            this.recipient = recipient;
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
            
            this.amount.KeyboardType = UIKeyboardType.DecimalPad;
            
            // Perform any additional setup after loading the view, typically from a nib.
            
            this.transactionDate = DateTime.Today;
            
            var datePicker = new UIDatePicker();
            datePicker.Mode = UIDatePickerMode.Date;
            datePicker.TimeZone = NSTimeZone.FromAbbreviation("GMT");
            datePicker.Date = DateTime.SpecifyKind(this.transactionDate, DateTimeKind.Utc);
            
            datePicker.ValueChanged += (sender, e) => 
            {
                this.transactionDate = DateTime.SpecifyKind(datePicker.Date, DateTimeKind.Unspecified);
                this.datePlaceholder.Text = this.transactionDate.ToString("D");
            }
            ;
            
            this.date.InputView = datePicker;
            this.amount.BecomeFirstResponder();

            var dt = DateTime.SpecifyKind(datePicker.Date, DateTimeKind.Unspecified);
            this.datePlaceholder.Text = dt.ToString("D");
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
        
        partial void doneButtonClicked(MonoTouch.Foundation.NSObject sender)
        {
            // todo: support IEditableObject
//            var oldBalance = this.recipient.CurrentBalance;
//            
//            RewardDatabase.Main.BeginTransaction();
//            try
//            {
//                var repo = RewardDatabase.Main.NewRepository<Transaction>();
//                using (repo)
//                {
//                    var transaction = new Transaction();
//                    transaction.RecipientId = this.recipient.RecipientId;
//                    transaction.Amount = Convert.ToDecimal(this.amount.Text);
//                    transaction.Description = this.description.Text;
//                    transaction.TransactionDate = this.transactionDate;
//                    
//                    repo.Insert(transaction);
//                    
//                    this.recipient.CurrentBalance -= transaction.Amount;
//                    
//                    using (var recipientRepo = RewardDatabase.Main.NewRepository<Recipient>())
//                    {
//                        recipientRepo.Update(this.recipient);
//                    }
//                }
//                
//                RewardDatabase.Main.Commit();
//            }
//            catch
//            {
//                this.recipient.CurrentBalance = oldBalance;
//                RewardDatabase.Main.Rollback();
//            }
            
            this.DismissModalViewControllerAnimated(true);
        }
        
        partial void cancelButtonClicked(MonoTouch.Foundation.NSObject sender)
        {
            this.DismissModalViewControllerAnimated(true);
        }
    }
}

