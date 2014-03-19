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
        Section _photoSection;
        BadgeElement _photoBadge;

        public AddEditContactScreen() : base(UITableViewStyle.Grouped, null, true)
        {
            // Navigation and root
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
            {
                NavigationController.DismissViewController(true, null);
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
                phoneNumber.KeyboardType = UIKeyboardType.NumberPad;
                phoneSection.Insert(phoneSection.Count - 1, phoneNumber);

                var deleteButton = new StyledStringElement("Delete This Number")
                {
                    TextColor = UIColor.Red,
                };

                deleteButton.Tapped += () =>
                {
                    phoneSection.Remove(phoneType);
                    phoneSection.Remove(phoneNumber);
                    phoneSection.Remove(deleteButton);
                };

                // Show/Hide Delete Button
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

                phoneType.Tapped += () =>
                {
                    var labels = new List<string>() { "Mobile", "Home", "Work", "Main", "Home Fax", "Work Fax", "Pager", "Other"};
                    var labelScreen = new LabelListScreen(labels);
                    var navigation = new UINavigationController(labelScreen);
                    NavigationController.PresentViewController(navigation, true, null);
                };
            });
            phoneSection.Add(phoneLoadMore);

            // Photo
            _photoSection = new Section();
            _photoBadge = new BadgeElement(UIImage.FromBundle("Images/UnknownIcon.jpg"), "");
            _photoBadge.Tapped += PhotoBadgeTapped;
            _photoSection.Add(_photoBadge);
            Root.Add(_photoSection);
        }

        public void PhotoBadgeTapped()
        {
            var imagePicker = new UIImagePickerController();
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                if (e.Info[UIImagePickerController.MediaType].ToString() == "public.image")
                {
                    UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                    if (originalImage != null)
                    {
                        _photoSection.Remove(_photoBadge);
                        _photoBadge = new BadgeElement(originalImage, ""); 
                        _photoBadge.Tapped += PhotoBadgeTapped;
                        _photoSection.Add(_photoBadge);
                    }

                }
                imagePicker.DismissViewController(true, null);
            }; 

            imagePicker.Canceled += (sender, e) =>
            {
                imagePicker.DismissViewController(true, null);
            };

            NavigationController.PresentViewController(imagePicker, true, null);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}