using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellNavTestPage.Pages;

class MainPage : Component
{
    protected override void OnMounted()
    {
        Routing.RegisterRoute<Page2>(nameof(Page2));
        Routing.RegisterRoute<PageWithArguments>(nameof(PageWithArguments));

        base.OnMounted();
    }


    public override VisualNode Render()
        => new Shell
        {
            new Page1()
        };
}

class Page1 : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Page1")
        {
            new VStack
            {
                new Button("Goto Page2")
                     .OnClicked(async ()=> await MauiControls.Shell.Current.GoToAsync(nameof(Page2)))
            }
            .HCenter()
            .VCenter()
        };
    }
}

class Page2 : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Page2")
        {
            new VStack
            {
                new Button("Goto Page3")
                    .OnClicked(async ()=> await MauiControls.Shell.Current.GoToAsync<PageWithArgumentsProps>(nameof(PageWithArguments), props => props.ParameterPassed = "Hello from Page2!"))
            }
            .HCenter()
            .VCenter()
        };
    }
}


class PageWithArgumentsState
{ }

class PageWithArgumentsProps
{
    public string ParameterPassed { get; set; }
}

class PageWithArguments : Component<PageWithArgumentsState, PageWithArgumentsProps>
{
    public override VisualNode Render()
    {
        return new ContentPage("PageWithArguments")
        {
            new VStack(spacing: 10)
            {
                new Label($"Parameter: {Props.ParameterPassed}")
                    .HCenter(),

                new Button("Open ModalPage")
                    .OnClicked(async () => await Navigation.PushModalAsync<ModalPage>())
            }
            //.HCenter()
            .VCenter()
        };
    }
}

class ModalPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Modal Page")
        {
            new VStack
            {
                new Button("Back")
                    .OnClicked(async () => await Navigation.PopModalAsync())
            }
            .HCenter()
            .VCenter()
        };
    }
}