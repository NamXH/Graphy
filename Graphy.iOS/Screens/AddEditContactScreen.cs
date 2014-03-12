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

            // Phone numbers
            var phoneSection = new Section();
            Root.Add(phoneSection);



            var phoneLoadMore = new LoadMoreElement("Add More Phone Numbers", "Loading...", lme =>
            {
                lme.Animating = false;
                phoneSection.Insert(phoneSection.Count - 1, new StringElement("abc"));

                var a = new EntryElement();
            });
            phoneSection.Add(phoneLoadMore);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}