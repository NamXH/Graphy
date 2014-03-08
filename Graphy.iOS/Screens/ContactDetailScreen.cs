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
            var numbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id);
            if (numbers.Count > 0)
            {
                foreach (var number in numbers)
                {
                    var sec = new Section(number.Type)
                    {
                        new StringElement(number.Number),
                    };
                    Root.Add(sec);
                }
            }

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

            // Email
            var emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id);
            if (emails.Count > 0)
            {
//                CreateUiList<Email>(emails, x => x.Address, x => x.Type))
                foreach (var email in emails)
                {
                    var sec = new Section(email.Type)
                    {
                        new StringElement(email.Address),
                    };
                    Root.Add(sec);
                }
            }
        }

        void CreateUiList<T>(List<T> list, Func<T, string> strFunc, Func<T, string> secNameFunc)
        {
            foreach (var x in list)
            {
                var sec = new Section(secNameFunc(x))
                {
                    new StringElement(strFunc(x))
                };
                Root.Add(sec);
            }
        }

        void EditButtonClicked(object sender, EventArgs e)
        {
        }
    }
}
