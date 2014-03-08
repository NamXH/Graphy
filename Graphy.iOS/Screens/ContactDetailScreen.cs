using System;
using System.Collections.Generic;
using System.Linq;
using Graphy.Core;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Graphy.iOS
{
    public partial class ContactDetailScreen : DialogViewController
    {
        public ContactDetailScreen(Contact contact) : base(UITableViewStyle.Grouped, null, true)
        {
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, EditButtonClicked);

            Root = new RootElement("");

            // Image & name & organization
            var fullName = DatabaseHelper.GetFullName(contact);
            if (!(string.IsNullOrEmpty(fullName) && contact.ImagePath == null))
            {
                var imagePath = contact.ImagePath != null ? "Images/" + contact.ImagePath : "Images/UnknownIcon.jpg";
                Root.Add(new Section(contact.Organization)
                {
                    new BadgeElement(UIImage.FromBundle(imagePath), fullName)
                });
            }
            else if (!string.IsNullOrEmpty(contact.Organization))
            {
                Root.Add(new Section("Organization") { new StringElement(contact.Organization) });
            }

            // Birthday
            if (!DateTime.Equals(contact.Birthday, new DateTime(1, 1, 1)))
            {
                Root.Add(new Section("Birthday") { new StringElement(contact.Birthday.ToShortDateString()) });
            }

            // Phone numbers
            var numbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id).ToList();
            CreateUiList<PhoneNumber>(numbers, x => x.Type, x => x.Number);

            // Addresses
            var addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);
            if (addresses.Count > 0)
            {
                foreach (var address in addresses)
                {
                    var sec = new Section(address.Type);

                    if (!string.IsNullOrEmpty(address.StreetLine1))
                    {
                        sec.Add(new StringElement(address.StreetLine1));
                    }
                    if (!string.IsNullOrEmpty(address.StreetLine2))
                    {
                        sec.Add(new StringElement(address.StreetLine2));
                    }
                    if (!string.IsNullOrEmpty(address.City))
                    {
                        sec.Add(new StringElement(address.City));
                    }
                    if (!string.IsNullOrEmpty(address.Province))
                    {
                        sec.Add(new StringElement(address.Province));
                    }
                    if (!string.IsNullOrEmpty(address.Country))
                    {
                        sec.Add(new StringElement(address.Country));
                    }
                    if (!string.IsNullOrEmpty(address.PostalCode))
                    {
                        sec.Add(new StringElement(address.PostalCode));
                    }

                    Root.Add(sec);
                }
            }

            // Emails
            var emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id).ToList();
            CreateUiList<Email>(emails, x => x.Type, x => x.Address);

            // Urls
            var urls = DatabaseManager.GetRowsRelatedToContact<Url>(contact.Id).ToList();
            CreateUiList<Url>(urls, x => x.Type, x => x.Link);

            // Instant Message
            var instantMessages = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id).ToList();
            CreateUiList<InstantMessage>(instantMessages, x => x.Type, x => x.Nickname);

            // Special Dates
            var dates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id).ToList();
            CreateUiList<SpecialDate>(dates, x => x.Type, x => x.Date.ToShortDateString());
        }

        void CreateUiList<T>(List<T> list, Func<T, string> sectionNameFunc, Func<T, string> elementName)
        {
            foreach (var x in list)
            {
                var section = new Section(sectionNameFunc(x))
                {
                    new StringElement(elementName(x))
                };
                Root.Add(section);
            }
        }

        void EditButtonClicked(object sender, EventArgs e)
        {
        }
    }
}
