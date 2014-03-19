using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Graphy.iOS
{
    //    public partial class LabelListScreen<T> : DialogViewController where T : new()
    //    {
    //        public LabelListScreen(IList<T> labels, Func<T, string> LabelName) : base(UITableViewStyle.Grouped, null)
    //        {
    //
    //        }
    //    }

    public partial class LabelListScreen : DialogViewController
    {
        public LabelListScreen(IList<string> labels) : base(UITableViewStyle.Grouped, null)
        {
            // Navigation
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
            {
                NavigationController.DismissViewController(true, null);
            });
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonClicked);

            Root = new RootElement("");
            var labelSection = new Section();
            Root.Add(labelSection);
            foreach (var label in labels)
            {
                var element = new StringElement(label);
                element.Tapped += () => {};
                labelSection.Add(element);
            }

            var customSection = new Section();
            Root.Add(customSection);
            var customElement = new StringElement("Add Custom Label");
            customElement.Tapped += () => {};
            customSection.Add(customElement);
        }

        public void DoneButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
