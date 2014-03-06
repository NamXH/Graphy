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
            var imagePath = contact.ImagePath != null ? "Images/" + contact.ImagePath : "Images/UnknownIcon.jpg";

            Root = new RootElement("")
            {
                new Section(contact.Organization)
                {
                    new BadgeElement(UIImage.FromBundle(imagePath), DatabaseHelper.GetFullName(contact)),
                },

            };
                
            if (!DateTime.Equals(contact.Birthday, new DateTime(1, 1, 1)))
            {
                Root.Add(new Section("Birthday") { new StringElement(contact.Birthday.ToShortDateString()) });
            }
        }
    }
}
