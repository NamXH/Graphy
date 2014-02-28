// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Graphy.iOS
{
	[Register ("AllContactsScreen")]
	partial class AllContactsScreen
	{
		[Outlet]
		MonoTouch.UIKit.UITableView contactsTable { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISearchBar searchBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (searchBar != null) {
				searchBar.Dispose ();
				searchBar = null;
			}

			if (contactsTable != null) {
				contactsTable.Dispose ();
				contactsTable = null;
			}
		}
	}
}
