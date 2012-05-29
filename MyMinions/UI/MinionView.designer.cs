// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MyMinions.UI
{
	partial class MinionView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel minionNameLabel { get; set; }

		[Action ("spendButtonClicked:")]
		partial void spendButtonClicked (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (minionNameLabel != null) {
				minionNameLabel.Dispose ();
				minionNameLabel = null;
			}
		}
	}
}
