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
                var number = new EntryElement(null, "Phone Number", null);
                phoneSection.Insert(phoneSection.Count - 1, number);

                var deleteButtonOn = false;
                phoneType.AccessoryTapped += () =>
                {
                    if (!deleteButtonOn)
                    {
                        deleteButtonOn = true;
                        var deleteButton = new StyledStringElement("Delete This Number")
                        {
                            BackgroundColor = UIColor.Red,
                        };
                        phoneSection.Insert(phoneType.IndexPath.Row + 2, deleteButton);
                    }
                };
            });
            phoneSection.Add(phoneLoadMore);

//            phoneSection.Add(new StyledStringElement("abc")
//            {
////                BackgroundColor = UIColor.Green,
//            });

//            var btn = new StyledStringElement("Delete This Number")
//            {
//                BackgroundColor = UIColor.Red,
//            };
//            phoneSection.Insert(phoneSection.Count - 1, btn);

            // Connections
//            var connectionSection = new Section();
//            Root.Add(connectionSection);

//            Root.Add(new Section() { new StyledStringElement("abc") });
//            TableView.SetEditing(true, true);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
        //        public override Source CreateSizingSource(bool unevenRows)
        //        {
        //            //                if (unevenRows)
        //            //                    throw new NotImplementedException("You need to create a new SourceSizing subclass, this sample does not have it");
        //            return new EditingSource(this);
        //        }
    }
    //    public class EditingSource : DialogViewController.Source
    //    {
    //        public EditingSource(DialogViewController dvc) : base(dvc)
    //        {
    //        }
    //
    //        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
    //        {
    //            // Trivial implementation: we let all rows be editable, regardless of section or row
    //            return true;
    //        }
    //
    //        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
    //        {
    //            var section = Container.Root[indexPath.Section];
    //            var element = section[indexPath.Row];
    //
    //            if (element is StyledStringElement)
    //            {
    //                return UITableViewCellEditingStyle.Delete;
    //            }
    //            else
    //            {
    //                return UITableViewCellEditingStyle.None;
    //            }
    //        }
    //
    //        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
    //        {
    //            //
    //            // In this method, we need to actually carry out the request
    //            //
    //            var section = Container.Root[indexPath.Section];
    //            var element = section[indexPath.Row];
    //            section.Remove(element);
    //        }
    //    }
}