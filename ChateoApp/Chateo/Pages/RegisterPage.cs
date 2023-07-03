using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using System;

namespace Chateo.Pages;

class LoginPageState
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}

class LoginPageProps
{
    public Action? OnLogged { get; set; }
}

class RegisterPage : Component<LoginPageState, LoginPageProps>
{
    public override VisualNode Render()
    {
        return new ContentPage()
        {
            new VStack
            {
                new Grid("*", "24, *")
                {
                    new Image("/images/back_icon.png")
                        .VEnd()
                        .OnTapped(OnBackClicked),

                    Theme.Current.Label("Your Profile")
                        .VEnd()
                        .Margin(8,0)
                        .FontSize(18)
                        .GridColumn(1),
                }
                .Padding(0,13),

                new Image("/images/user_plus.png")
                    .HeightRequest(100)
                    .Margin(0,46,0,32),

                Theme.Current.Entry()
                    .Margin(8,0)
                    .Placeholder("First Name (Required)")
                    .OnAfterTextChanged(text => State.FirstName = text)
                    ,

                Theme.Current.Entry()
                    .Margin(8,12,8,0)
                    .Placeholder("Last Name (Optional)")
                    .OnAfterTextChanged(text => State.LastName = text),

                Theme.Current.PrimaryButton("Save")
                    .HeightRequest(52)
                    .Margin(8,68,8,0)
                    .OnClicked(OnSaveClicked)
            }
            .Margin(16)
        }
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);

    }

    private async void OnSaveClicked()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        var id = Guid.NewGuid();
        await chatServer.CreateUser(id, State.FirstName, State.LastName);

        Preferences.Default.Set("current_user", new UserViewModel(id, State.FirstName, State.LastName));

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