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
            Root = new RootElement("")
            {
                new Section("First Section")
                {
                    new StringElement("Hello", () =>
                    {
                        new UIAlertView("Hola", "Thanks for tapping!", null, "Continue").Show(); 
                    }),
                    new EntryElement("Name", "Enter your name", String.Empty)
                },
                new Section("Second Section")
                {
                },
            };
        }
    }
}
