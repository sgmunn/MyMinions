// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MyMinions
{
	[Register ("SpendViewController")]
	partial class SpendViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField amount { get; set; }

		[Action ("doneButtonClicked:")]
		partial void doneButtonClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("cancelButtonClicked:")]
		partial void cancelButtonClicked (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (amount != null) {
				amount.Dispose ();
				amount = null;
			}
		}
	}
}
