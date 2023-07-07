using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using System;
using System.Linq;

namespace Chateo.Pages;

class LoginPageState
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Avatar { get; set; }
}

class LoginPageProps
{
    public Action? OnLogged { get; set; }
}

class RegisterPage : Component<LoginPageState, LoginPageProps>
{
    protected override void OnMountedOrPropsChanged()
    {
        MauiControls.Routing.UnRegisterRoute("avatar");
        Routing.RegisterRoute<AvatarPage>("avatar");

        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return new ContentPage()
        {
            new VStack
            {
                new Grid("*", "24, *")
                {
                    Theme.Current.Image(Icon.Back)
                        .VEnd()
                        .OnTapped(OnBackClicked),

                    Theme.Current.Label("Your Profile")
                        .VEnd()
                        .Margin(8,0)
                        .FontSize(18)
                        .GridColumn(1),
                }
                .Padding(0,13),

                (State.Avatar != null ? new Image($"/images/{State.Avatar}.png") : Theme.Current.Image(Icon.UserPlus))
                    .HeightRequest(100)
                    .WidthRequest(100)
                    .Margin(0,46,0,32)
                    .OnTapped(OnOpenAvatarPage),

                Theme.Current.Entry()
                    .Margin(8,0)
                    .Placeholder("First Name (Required)")
                    .OnAfterTextChanged(text => SetState(s => s.FirstName = text))
                    ,

                Theme.Current.Entry()
                    .Margin(8,12,8,0)
                    .Placeholder("Last Name (Optional)")
                    .OnAfterTextChanged(text => SetState(s => s.LastName = text)),

                Theme.Current.PrimaryButton("Save")
                    .HeightRequest(52)
                    .Margin(8,68,8,0)
                    .IsEnabled(!string.IsNullOrWhiteSpace(State.FirstName) && !string.IsNullOrWhiteSpace(State.LastName) && State.Avatar != null)
                    .OnClicked(OnSaveClicked)
            }
            .Margin(16)
        }
        .BackgroundColor(Theme.Current.Background)
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);

    }

    private async void OnOpenAvatarPage()
    {
        await MauiControls.Shell.Current.GoToAsync<AvatarPageProps>("avatar", props =>
        {
            props.OnAvatarSelected = (avatar) => SetState(s => s.Avatar = avatar);
        });
    }

    private async void OnSaveClicked()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        var id = Guid.NewGuid();
        var newUser = await chatServer.CreateUser(id, State.FirstName, State.LastName, State.Avatar ?? throw new InvalidOperationException());

        Preferences.Default.SetAsJson("current_user", newUser);

        OnBackClicked();

        Props.OnLogged?.Invoke();        
    }

    private void OnBackClicked()
    {
        if (Navigation == null)
        {
            return;
        }

        if (Navigation.NavigationStack.Count > 0)
        {
            Navigation.PopAsync();
        }
    }
}