// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace RewardSquirrel
{
	[Register ("SpendViewController")]
	partial class SpendViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField amount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField date { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField datePlaceholder { get; set; }

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

			if (date != null) {
				date.Dispose ();
				date = null;
			}

			if (datePlaceholder != null) {
				datePlaceholder.Dispose ();
				datePlaceholder = null;
			}
		}
	}
}
