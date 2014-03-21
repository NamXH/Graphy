using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Graphy.Core;

namespace Graphy.iOS
{
    public partial class AddEditContactScreen : DialogViewController
    {
        UINavigationController _rootContainerNavigationController;
        Section _photoSection, _phoneSection, _nameSection, _emailSection, _urlSection, _instantMsgSection;
        BadgeElement _photoBadge;
        UIImage _profilePhoto;

        public AddEditContactScreen(UINavigationController nav) : base(UITableViewStyle.Grouped, null, true)
        {
            _rootContainerNavigationController = nav;

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

            // Emails
            _emailSection = new Section("Emails");
            Root.Add(_emailSection);
            var emailLoadMore = new LoadMoreElement("Add More Emails", "Loading...", EmailLoadMoreTapped);
            _emailSection.Add(emailLoadMore);

            // Urls
            _urlSection = new Section("Urls");
            Root.Add(_urlSection);
            var urlLoadMore = new LoadMoreElement("Add More Urls", "Loading...", UrlLoadMoreTapped);
            _urlSection.Add(urlLoadMore);

            // IMs
            _instantMsgSection = new Section("Instant Messages");
            Root.Add(_instantMsgSection);
            var instantMsgLoadMore = new LoadMoreElement("Add More Instant Messages", "Loading...", InstantMsgLoadMoreTapped);
            _instantMsgSection.Add(instantMsgLoadMore);
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
                else
                {
                    new UIAlertView("Warning", "Please Choose An Image", null, "OK", null).Show();
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
            var labels = new List<string>() { "Mobile", "Home", "Work", "Main", "Home Fax", "Work Fax", "Pager", "Other" };
            PopulateElements(lme, _phoneSection, "Mobile", "Phone Number", UIKeyboardType.NumberPad, "Delete This Number", labels);
        }

        public void EmailLoadMoreTapped(LoadMoreElement lme)
        {
            var labels = new List<string>() { "Home", "Work", "Oher" };
            PopulateElements(lme, _emailSection, "Home", "Email Address", UIKeyboardType.EmailAddress, "Delete This Email", labels);
        }

        public void UrlLoadMoreTapped(LoadMoreElement lme)
        {
            var labels = new List<string>() { "Home Page", "Home", "Work" };
            PopulateElements(lme, _urlSection, "Home Page", "Url", UIKeyboardType.Url, "Delete This Url", labels);
        }

        public void InstantMsgLoadMoreTapped(LoadMoreElement lme)
        {
            var labels = new List<string>() { "Skype", "Hangouts", "Facebook Messenger", "MSN", "Yahoo", "AIM", "QQ" };
            PopulateElements(lme, _instantMsgSection, "Skype", "IM", UIKeyboardType.ASCIICapable, "Delete This IM", labels);
        }

        public void PopulateElements(LoadMoreElement lme, Section section, string typeLable, string valueLabel, UIKeyboardType entryKeyboardType, string deleteLabel, IList<string> labelList)
        {
            lme.Animating = false;

            var type = new StyledStringElement(typeLable) { Accessory = UITableViewCellAccessory.DetailDisclosureButton };
            section.Insert(section.Count - 1, type);
            var value = new EntryElement(null, valueLabel, null);
            value.KeyboardType = entryKeyboardType;
            section.Insert(section.Count - 1, value);

            var deleteButton = new StyledStringElement(deleteLabel)
            {
                TextColor = UIColor.Red,
            };

            deleteButton.Tapped += () =>
            {
                section.Remove(type);
                section.Remove(value);
                section.Remove(deleteButton);
            };

            // Show/Hide Delete Button
            var deleteButtonOn = false;
            type.AccessoryTapped += () =>
            {
                if (!deleteButtonOn)
                {
                    deleteButtonOn = true;
                    section.Insert(type.IndexPath.Row + 2, UITableViewRowAnimation.Bottom, deleteButton);
                }
                else
                {
                    deleteButtonOn = false;
                    section.Remove(deleteButton);
                }
            };

            type.Tapped += () =>
            {
                var labelScreen = new LabelListScreen(labelList);
                var navigation = new UINavigationController(labelScreen);
                NavigationController.PresentViewController(navigation, true, null);
            };
        }

        public void DoneButtonClicked(object sender, EventArgs args)
        {
            var contact = new Contact();

            // Photo
            if (_profilePhoto != null)
            {
                contact.ImageName = Guid.NewGuid().ToString() + ".jpg";
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var imagePath = System.IO.Path.Combine(directory, contact.ImageName);

                NSData imgData = _profilePhoto.AsJPEG();
                NSError err = null;
                if (!imgData.Save(imagePath, false, out err))
                {
                    throw new Exception(err.LocalizedDescription);
                }
            }

            // Name
            contact.FirstName = ((EntryElement)_nameSection[0]).Value ?? null;
            contact.MiddleName = ((EntryElement)_nameSection[1]).Value ?? null;
            contact.LastName = ((EntryElement)_nameSection[2]).Value ?? null;
            contact.Organization = ((EntryElement)_nameSection[3]).Value ?? null;

            DatabaseManager.AddNewContact(contact);
            _rootContainerNavigationController.PopViewControllerAnimated(false);
            _rootContainerNavigationController.PushViewController(new AllContactsScreen(), false);
            NavigationController.DismissViewController(true, null);

            // Phone numbers
            for (int i = 0; i <= _phoneSection.Count - 1; i++)
            {
                if (_phoneSection[i].GetType() == typeof(EntryElement))
                {
                    if (i >= 1)
                    {
                        var phoneType = (StyledStringElement)_phoneSection[i - 1];
                        var phoneNumber = (EntryElement)_phoneSection[i];

                        if (!string.IsNullOrEmpty(phoneNumber.Value))
                        {
                            Console.WriteLine("type:" + phoneType.Caption + " number:" + phoneNumber.Value);
                        }
                    }
                }
            }
        }
    }
}