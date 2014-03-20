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
        Section _photoSection, _phoneSection, _nameSection;
        BadgeElement _photoBadge;
        UIImage _profilePhoto;

        public AddEditContactScreen() : base(UITableViewStyle.Grouped, null, true)
        {
            // Navigation
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
            {
                NavigationController.DismissViewController(true, null);
            });
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonClicked);
            NavigationItem.Title = "New Contact";

            Root = new RootElement("");

            // Photo
            _photoSection = new Section();
            _photoBadge = new BadgeElement(UIImage.FromBundle("Images/UnknownIcon.jpg"), "Add Photo");
            _photoBadge.Tapped += PhotoBadgeTapped;
            _photoSection.Add(_photoBadge);
            Root.Add(_photoSection);

            // Name
            _nameSection = new Section();
            var firstName = new EntryElement(null, "First", null);
            var middleName = new EntryElement(null, "Middle", null);
            var lastName = new EntryElement(null, "Last", null);
            var org = new EntryElement(null, "Organization", null);
            _nameSection.Add(firstName);
            _nameSection.Add(middleName);
            _nameSection.Add(lastName);
            _nameSection.Add(org);
            Root.Add(_nameSection);

            // Phone numbers
            _phoneSection = new Section("Phone Numbers");
            Root.Add(_phoneSection);
            var phoneLoadMore = new LoadMoreElement("Add More Phone Numbers", "Loading...", PhoneLoadMoreTapped);
            _phoneSection.Add(phoneLoadMore);
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
                    _profilePhoto = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                    if (_profilePhoto != null)
                    {
                        _photoSection.Remove(_photoBadge);
                        _photoBadge = new BadgeElement(_profilePhoto, ""); 
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

        public void PhoneLoadMoreTapped(LoadMoreElement lme)
        {
            lme.Animating = false;

            var phoneType = new StyledStringElement("Mobile") { Accessory = UITableViewCellAccessory.DetailDisclosureButton };
            _phoneSection.Insert(_phoneSection.Count - 1, phoneType);
            var phoneNumber = new EntryElement(null, "Phone Number", null);
            phoneNumber.KeyboardType = UIKeyboardType.NumberPad;
            _phoneSection.Insert(_phoneSection.Count - 1, phoneNumber);

            var deleteButton = new StyledStringElement("Delete This Number")
            {
                TextColor = UIColor.Red,
            };

            deleteButton.Tapped += () =>
            {
                _phoneSection.Remove(phoneType);
                _phoneSection.Remove(phoneNumber);
                _phoneSection.Remove(deleteButton);
            };

            // Show/Hide Delete Button
            var deleteButtonOn = false;
            phoneType.AccessoryTapped += () =>
            {
                if (!deleteButtonOn)
                {
                    deleteButtonOn = true;
                    _phoneSection.Insert(phoneType.IndexPath.Row + 2, UITableViewRowAnimation.Bottom, deleteButton);
                }
                else
                {
                    deleteButtonOn = false;
                    _phoneSection.Remove(deleteButton);
                }
            };

            phoneType.Tapped += () =>
            {
                var labels = new List<string>() { "Mobile", "Home", "Work", "Main", "Home Fax", "Work Fax", "Pager", "Other" };
                var labelScreen = new LabelListScreen(labels);
                var navigation = new UINavigationController(labelScreen);
                NavigationController.PresentViewController(navigation, true, null);
            };
        }

        public void DoneButtonClicked(object sender, EventArgs args)
        {
            var a = (EntryElement)_nameSection[0];
            Console.WriteLine(a.Value);
            var b = _phoneSection[0];
            Console.WriteLine(b.GetType().FullName);
            var c = (StyledStringElement)b;
            Console.WriteLine(c.Caption);
        }
    }
}