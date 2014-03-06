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
            Root = new RootElement("");

            var fullName = DatabaseHelper.GetFullName(contact);

            // Profile pic & name & organization
            if (!string.IsNullOrEmpty(fullName))
            {
                var imagePath = contact.ImagePath != null ? "Images/" + contact.ImagePath : "Images/UnknownIcon.jpg";
                new Section(contact.Organization)
                {
                    new BadgeElement(UIImage.FromBundle(imagePath), fullName)
                };
            }
            else
            {
                new Section(contact.Organization) { };
            }

                
            if (!DateTime.Equals(contact.Birthday, new DateTime(1, 1, 1)))
            {
                Root.Add(new Section("Birthday") { new StringElement(contact.Birthday.ToShortDateString()) });
            }

        }
    }
}
