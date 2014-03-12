using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Graphy.iOS
{
    public partial class AddEditContactScreen : DialogViewController
    {
        public AddEditContactScreen() : base(UITableViewStyle.Grouped, null, true)
        {
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
            {
                NavigationController.PopViewControllerAnimated(true);
            });
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonClicked);

            Root = new RootElement("");
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
