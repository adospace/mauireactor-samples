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

    }
}