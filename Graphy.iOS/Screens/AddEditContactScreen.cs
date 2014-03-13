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
            // Navigation and root
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
            {
                NavigationController.PopViewControllerAnimated(false);
            });
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonClicked);
            Root = new RootElement("");

            // Phone numbers
            var phoneSection = new Section();
            Root.Add(phoneSection);

            var phoneLoadMore = new LoadMoreElement("Add More Phone Numbers", "Loading...", lme =>
            {
                lme.Animating = false;

                var phoneType = new StyledStringElement("Mobile") { Accessory = UITableViewCellAccessory.DetailDisclosureButton };
                phoneSection.Insert(phoneSection.Count - 1, phoneType);
                var phoneNumber = new EntryElement(null, "Phone Number", null);
                phoneSection.Insert(phoneSection.Count - 1, phoneNumber);

                var deleteButton = new StyledStringElement("Delete This Number") 
                {
                    TextColor = UIColor.Red,
                };

                deleteButton.Tapped += () =>  {
                    phoneSection.Remove(phoneType);
                    phoneSection.Remove(phoneNumber);
                    phoneSection.Remove(deleteButton);
                };

                var deleteButtonOn = false;
                phoneType.AccessoryTapped += () =>
                {
                    if (!deleteButtonOn)
                    {
                        deleteButtonOn = true;
                        phoneSection.Insert(phoneType.IndexPath.Row + 2, deleteButton);
                    }
                    else
                    {
                        deleteButtonOn = false;
                        phoneSection.Remove(deleteButton);
                    }
                };
            });
            phoneSection.Add(phoneLoadMore);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}