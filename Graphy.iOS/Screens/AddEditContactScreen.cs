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

                var phoneType = new StyledStringElement ("Mobile") { Accessory = UITableViewCellAccessory.DetailDisclosureButton };
                phoneSection.Insert(phoneSection.Count - 1, phoneType);
                var number = new EntryElement(null, "Phone Number", null);
                phoneSection.Insert(phoneSection.Count - 1, number);
            });
            phoneSection.Add(phoneLoadMore);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}