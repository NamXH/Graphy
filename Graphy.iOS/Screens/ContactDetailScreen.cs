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
            var fullName = contact.GetFullName();
            if (!(string.IsNullOrEmpty(fullName) && contact.ImagePath == null))
            {
                var imagePath = contact.ImagePath != null ? "Images/" + contact.ImagePath : "Images/UnknownIcon.jpg";
                Root.Add(new Section(contact.Organization)
                {
                    new BadgeElement(UIImage.FromBundle(imagePath), fullName),
                });
            }
            else if (!string.IsNullOrEmpty(contact.Organization))
            {
                Root.Add(new Section("Organization")
                { 
                    new StringElement(contact.Organization), 
                });
            }

            // Phone numbers
            var numbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id).ToList();
            CreateUiList<PhoneNumber>(numbers, x => x.Type, x => x.Number);

            // Emails
            var emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id);
            CreateUiList<Email>(emails, x => x.Type, x => x.Address);

            // Urls
            var urls = DatabaseManager.GetRowsRelatedToContact<Url>(contact.Id);
            CreateUiList<Url>(urls, x => x.Type, x => x.Link);

            // Addresses
            var addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);
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

            // Birthday
            if (!DateTime.Equals(contact.Birthday, new DateTime(1, 1, 1)))
            {
                Root.Add(new Section("Birthday") { new StringElement(contact.Birthday.ToShortDateString()) });
            }

            // Special Dates
            var dates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);
            CreateUiList<SpecialDate>(dates, x => x.Type, x => x.Date.ToShortDateString());


            // Instant Message
            var instantMessages = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);
            CreateUiList<InstantMessage>(instantMessages, x => x.Type, x => x.Nickname);

            // Tags
            var tagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            var tagIds = new List<int>();
            foreach (var tagMap in tagMaps)
            {
                tagIds.Add(tagMap.TagId);
            }
            var tags = DatabaseManager.GetRows<Tag>(tagIds);
            foreach (var tag in tags)
            {
                var section = new Section(tag.Name);
                if (!string.IsNullOrEmpty(tag.Detail))
                {
                    section.Add(new MultilineElement(tag.Detail));
                }

                Root.Add(section);
            }

            // Connections (From)
            var fromConnections = DatabaseManager.GetConnectionsFromContact(contact.Id);

            var fromConnectionTypes = new List<ConnectionType>();
            var toContacts = new List<Contact>();
            foreach (var conn in fromConnections)
            {
                fromConnectionTypes.Add(DatabaseManager.GetRow<ConnectionType>(conn.ConnectionTypeId));
                toContacts.Add(DatabaseManager.GetRow<Contact>(conn.ToContactId));
            }

            for (int i = 0; i <= fromConnections.Count - 1; i++)
            {
                var section = new Section(fromConnectionTypes[i].Name);

                var direction = new StringElement("=>", toContacts[i].GetFullName());
                section.Add(direction);

                if (!string.IsNullOrEmpty(fromConnections[i].ExtraInfo))
                {
                    var extra = new MultilineElement(fromConnections[i].ExtraInfo);
                    section.Add(extra);
                }
                Root.Add(section);
            }

            // Connection (To)
            var toConnections = DatabaseManager.GetConnectionsToContact(contact.Id);

            var toConnectionTypes = new List<ConnectionType>();
            var fromContacts = new List<Contact>();
            foreach (var conn in toConnections)
            {
                toConnectionTypes.Add(DatabaseManager.GetRow<ConnectionType>(conn.ConnectionTypeId));
                fromContacts.Add(DatabaseManager.GetRow<Contact>(conn.FromContactId));
            }

            for (int i = 0; i <= toConnections.Count - 1; i++)
            {
                var section = new Section(toConnectionTypes[i].Name);

                var direction = new StringElement("<=", fromContacts[i].GetFullName());
                section.Add(direction);

                if (!string.IsNullOrEmpty(toConnections[i].ExtraInfo))
                {
                    var extra = new MultilineElement(toConnections[i].ExtraInfo);
                    section.Add(extra);
                }
                Root.Add(section);
            }

            // Favourite
            var favourite = new BooleanElement("Favourite", contact.Favourite);
            favourite.ValueChanged += (object sender, EventArgs e) =>
            {
                contact.Favourite = favourite.Value;
            };
            var favSection = new Section();
            favSection.Add(favourite);
            Root.Add(favSection);
        }

        void CreateUiList<T>(IList<T> list, Func<T, string> SectionNameFunc, Func<T, string> ElementName)
        {
            foreach (var x in list)
            {
                var section = new Section(SectionNameFunc(x));
                var element = ElementName(x);
                if (!string.IsNullOrEmpty(element))
                {
                    section.Add(new StringElement(element));
                }

                Root.Add(section);
            }
        }

        void EditButtonClicked(object sender, EventArgs e)
        {
        }
    }
}
