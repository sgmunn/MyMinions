// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MyMinions
{
	[Register ("EarnViewController")]
	partial class EarnViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField amount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel testLabel { get; set; }

		[Action ("cancelButtonClicked:")]
		partial void cancelButtonClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("earnButtonClicked:")]
		partial void earnButtonClicked (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (amount != null) {
				amount.Dispose ();
				amount = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (testLabel != null) {
				testLabel.Dispose ();
				testLabel = null;
			}
		}
	}
}
