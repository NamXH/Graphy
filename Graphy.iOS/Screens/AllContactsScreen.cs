using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;

namespace Graphy.iOS
{
    public partial class AllContactsScreen : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public AllContactsScreen()
            : base(UserInterfaceIdiomIsPhone ? "AllContactsScreen_iPhone" : "AllContactsScreen_iPad", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Add buttons & name to Navigation Bar
            var leftButton = new UIBarButtonItem(UIImage.FromBundle("Images/TagsIcon.png"), UIBarButtonItemStyle.Plain, LeftButtonClicked);
            NavigationItem.LeftBarButtonItem = leftButton;
            var rightButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, RightButtonClicked);
            NavigationItem.RightBarButtonItem = rightButton;
            NavigationItem.Title = "All Contacts";

            // Search bar
            searchBar.SearchButtonClicked += SearchButtonClicked;

            // Contacts table
            contactsTable.Source = new AllContactsTableSource();
        }

        void LeftButtonClicked(object sender, EventArgs e)
        {
        }

        void RightButtonClicked(object sender, EventArgs e)
        {
        }

        void SearchButtonClicked(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// View source of the table.
    /// </summary>
    public class AllContactsTableSource : UITableViewSource
    {
        string cellIdentifier = "TableCell";
        string[] keys = new string[26];

        List<String> m_items = new List<string>();
        Dictionary<string, List<String>> m_idxItems = new Dictionary<string, List<string>>();

        public AllContactsTableSource()
        {
	    // Init keys
            int count = 0;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                keys[count] = c.ToString();
                count++;
            }
	    
            m_items.Add("foo foo");
            m_items.Add("bar bar");
            m_items.Add("baz baz");

            for (char c = 'a'; c <= 'z'; c++)
            {
                m_idxItems.Add(c.ToString(), new List<string>());
            }

            foreach (var t in m_items)
            {
                if (m_idxItems.ContainsKey(t[0].ToString()))
                {
                    m_idxItems[t[0].ToString()].Add(t);
                }
                else
                {
                    //m_idxItems.Add(t[0].ToString(), new List<string>() { t });
                }
            }

            keys = m_idxItems.Keys.ToArray();
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return keys.Length;
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return keys;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return m_idxItems[keys[section]].Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }
            cell.TextLabel.Text = m_idxItems[keys[indexPath.Section]][indexPath.Row];

            return cell;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            if (m_idxItems[keys[section]].Count > 0)
            {
                return keys[section].ToUpper();
            }
            else
                return null;
        }


        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    new UIAlertView("Row Selected", m_items[indexPath.Row], null, "OK", null).Show();
        //    tableView.DeselectRow(indexPath, true);
        //}
    }
}

