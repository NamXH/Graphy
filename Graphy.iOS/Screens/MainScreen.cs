using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphy.Core;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Graphy.iOS
{
    public class MainScreen : UITabBarController
    {
        UIViewController[] _tabs;

        public MainScreen()
        {
            InitiateApplication();

            var tab1 = new UIViewController();
            tab1.Title = "one";
            tab1.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites, 0);

            var allContactsScreen = new AllContactsScreen();
            var tab2 = new UINavigationController(allContactsScreen);
            tab2.Title = "Contacts";
            tab2.TabBarItem = new UITabBarItem(UITabBarSystemItem.Contacts, 0);

            //            var tab2 = new UIViewController();
            //            tab2.Title = "Two";

            var tab3 = new UIViewController();
            tab3.Title = "three";
            tab3.TabBarItem = new UITabBarItem(UITabBarSystemItem.History, 0);

            var tab4 = new UIViewController();
            tab4.Title = "four";
            tab4.TabBarItem = new UITabBarItem(UITabBarSystemItem.More, 0);

            _tabs = new UIViewController[] { tab1, tab2, tab3, tab4 };
            this.ViewControllers = _tabs;
            this.SelectedViewController = tab2;
        }

        public void InitiateApplication()
        {
            // Copy images to Personal folder
            var unknownIcon = UIImage.FromBundle("Images/UnknownIcon.jpg");
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var imagePath = System.IO.Path.Combine(directory, "UnknownIcon.jpg");
            NSData imgData = unknownIcon.AsJPEG();
            NSError err = null;
            if (!imgData.Save(imagePath, false, out err))
            {
                throw new Exception(err.LocalizedDescription);
            }
            // ## Add Bill Gates picture for test
            var bill = UIImage.FromBundle("Images/Bill.jpg");
            var billPath = System.IO.Path.Combine(directory, "Bill.jpg");

            NSData billImgData = bill.AsJPEG();
            if (!billImgData.Save(billPath, false, out err))
            {
                throw new Exception(err.LocalizedDescription);
            }
        }
    }
}