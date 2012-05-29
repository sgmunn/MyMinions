// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace RewardSquirrel
{
	[Register ("TransactionsViewController")]
	partial class TransactionsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Action ("closeButonClicked:")]
		partial void closeButonClicked (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}
