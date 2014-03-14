using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Graphy.iOS
{
    public partial class LabelListScreen<T> : DialogViewController where T : new()
    {
        public LabelListScreen(IList<T> labels, Func<T, string> LabelName) : base(UITableViewStyle.Grouped, null)
        {

        }
    }
}
