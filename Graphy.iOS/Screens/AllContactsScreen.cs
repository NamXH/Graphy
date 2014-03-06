using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;
using Graphy.Core;

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
            var leftButton = new UIBarButtonItem(UIImage.FromBundle("Icons/TagsIcon.png"), UIBarButtonItemStyle.Plain, LeftButtonClicked);
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
        string m_cellId = "TableCell";
        string[] m_keys = new string[26];
        Dictionary<string, List<Contact>> m_items = new Dictionary<string, List<Contact>>();

        public AllContactsTableSource()
        {
            // Init index keys
            var i = 0;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                m_keys[i] = c.ToString();
                i++;
            }

            var contactList = DatabaseManager.GetRowsRelatedToContact<Contact>();

            foreach (var contact in contactList)
            {
                var firstNotNullName = new string[]{ contact.FirstName, contact.MiddleName, contact.LastName }.FirstOrDefault(s => !string.IsNullOrEmpty(s)) ?? " ";
                var firstLetter = firstNotNullName[0];
                var firstLetterUpper = Char.ToUpper(firstLetter).ToString();

                if (m_items.ContainsKey(firstLetterUpper))
                {
                    m_items[firstLetterUpper].Add(contact);
                }
                else
                {
                    m_items.Add(firstLetterUpper, new List<Contact>() { contact });
                }
            }
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return m_keys;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return m_items.Keys.Count;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return m_items[m_keys[section]].Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(m_cellId);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, m_cellId);
            }
            cell.TextLabel.Text = DatabaseHelper.GetFullName(m_items[m_keys[indexPath.Section]][indexPath.Row]);

            return cell;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            if (m_items[m_keys[section]].Count > 0)
            {
                return m_keys[section];
            }
            else
            {
                return null;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ContactDetailScreen contactDetail = new ContactDetailScreen(m_items[m_keys[indexPath.Section]][indexPath.Row]);
            AppDelegate.RootNavigationController.PushViewController(contactDetail, true);
        }
    }
}

