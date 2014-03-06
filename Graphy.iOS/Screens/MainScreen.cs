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
        public MainScreen()
        {
        }

        public void CreateTabs()
        {
            var tab1 = new UIViewController();
            tab1.Title = "one";

            var allContactsScreen = new AllContactsScreen(this.NavigationController);
            var tab2 = new UINavigationController(allContactsScreen);
            tab2.Title = "Contacts";
            tab2.TabBarItem = new UITabBarItem(UITabBarSystemItem.Contacts, 0);

//            var tab2 = new UIViewController();
//            tab2.Title = "Two";

            var tab3 = new UIViewController();
            tab3.Title = "three";

            var tab4 = new UIViewController();
            tab4.Title = "four";

            var tabs = new UIViewController[] { tab1, tab2, tab3, tab4 };
            this.ViewControllers = tabs;
            this.SelectedViewController = tab2;
        }
    }
}