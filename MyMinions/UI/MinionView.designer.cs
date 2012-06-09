// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MyMinions.UI
{
	[Register ("MinionView")]
	partial class MinionView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel minionNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView transactionTable { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton transactionButton { get; set; }

		[Action ("earnButtonClick:")]
		partial void earnButtonClick (MonoTouch.Foundation.NSObject sender);

		[Action ("spendButtonClick:")]
		partial void spendButtonClick (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (minionNameLabel != null) {
				minionNameLabel.Dispose ();
				minionNameLabel = null;
			}

			if (transactionTable != null) {
				transactionTable.Dispose ();
				transactionTable = null;
			}

			if (transactionButton != null) {
				transactionButton.Dispose ();
				transactionButton = null;
			}
		}
	}
}
