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
        string CellId = "TableCell";
        string[] _keys = new string[27];
        Dictionary<string, List<Contact>> _items = new Dictionary<string, List<Contact>>();

        public AllContactsTableSource()
        {
            var contactList = DatabaseManager.GetRows<Contact>();

            foreach (var contact in contactList)
            {
                var firstNotNullName = new string[]{ contact.FirstName, contact.MiddleName, contact.LastName }.FirstOrDefault(s => !string.IsNullOrEmpty(s)) ?? " ";
                var firstLetter = firstNotNullName[0];
                var firstLetterUpper = char.IsLetter(firstLetter) ? char.ToUpper(firstLetter).ToString() : "#";

                if (_items.ContainsKey(firstLetterUpper))
                {
                    _items[firstLetterUpper].Add(contact);
                }
                else
                {
                    _items.Add(firstLetterUpper, new List<Contact>() { contact });
                }
            }
            _keys = _items.Keys.ToArray();
            Array.Sort(_keys);
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return _keys;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return _items.Keys.Count;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return _items[_keys[section]].Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellId);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellId);
            }
            cell.TextLabel.Text = DatabaseHelper.GetFullName(_items[_keys[indexPath.Section]][indexPath.Row]);

            return cell;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            if (_items[_keys[section]].Count > 0)
            {
                return _keys[section];
            }
            else
            {
                return null;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ContactDetailScreen contactDetail = new ContactDetailScreen(_items[_keys[indexPath.Section]][indexPath.Row]);
            AppDelegate.RootNavigationController.PushViewController(contactDetail, true);
        }
    }
}

