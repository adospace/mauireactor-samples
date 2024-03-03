using MauiReactor.Internals;
using MauiReactor;
using ShellTestPage.Pages;
using Shouldly;
using Microsoft.Maui.Controls;
using System.Linq;

namespace SampleTests
{
    public class Tests
    {
        [Test]
        public void TestNavigationMainPageIssue218()
        {
            var mainPageNode = TemplateHost.Create(new MainPageIssue218());

            var shell = mainPageNode.Find<MauiControls.Shell>("MainShell");

            var shellItem1 = shell.Find<MauiControls.FlyoutItem>("FlyoutItem_Page1");
            var shellItem2 = shell.Find<MauiControls.FlyoutItem>("FlyoutItem_Page2");

            shell.CurrentItem.ShouldBe(shellItem1);

            shell.CurrentItem = shellItem2;

            shell.CurrentItem.ShouldBe(shellItem2);

            var gotoPage1Button = mainPageNode.Find<MauiControls.Button>("GotoPage1Button");
            gotoPage1Button.SendClicked();

            shell.CurrentItem.ShouldBe(shellItem1);
        }

        [Test]
        public void TestNavigationMainPage8()
        {
            var mainPageNode = TemplateHost.Create(new MainPage8());

            var shell = mainPageNode.Find<MauiControls.Shell>("MainShell");
            var tabBar = shell.FindAll<MauiControls.TabBar>().First();

            var shellItem1 = shell.Find<MauiControls.Tab>("NotificationsItem");
            var shellItem2 = shell.Find<MauiControls.Tab>("DatabaseItem");

            //Shell should show the Notifications as the first page
            tabBar.CurrentItem.ShouldBe(shellItem1);

            //Navigate to Database page
            tabBar.CurrentItem = shellItem2;

            //Database page should be visible now
            tabBar.CurrentItem.ShouldBe(shellItem2);
        }

        [Test]
        public void TestNavigationMainPage2()
        {
            var mainPageNode = TemplateHost.Create(new MainPage2());

            var shell = mainPageNode.Find<MauiControls.Shell>("MainShell");
            var tab = shell.Find<MauiControls.Tab>("tab");

            var notifications_item = shell.Find<MauiControls.ShellContent>("notifications_item");
            var home_item = shell.Find<MauiControls.ShellContent>("home_item");
            var settings_item = shell.Find<MauiControls.ShellContent>("settings_item");
            var flyout_item = shell.Find<MauiControls.FlyoutItem>("flyout_item");
            var database_item = shell.Find<MauiControls.ShellContent>("database_item");


            shell.CurrentItem.ShouldBe(flyout_item);

            flyout_item.CurrentItem.ShouldBe(tab);

            tab.CurrentItem.ShouldBe(home_item);

            //Navigate to database page
            flyout_item.CurrentItem = database_item;

            flyout_item.CurrentItem.ShouldBe(database_item);
        }

    }
}