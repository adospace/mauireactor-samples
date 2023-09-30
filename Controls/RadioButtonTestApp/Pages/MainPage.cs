using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;

namespace RadioButtonTestApp.Pages;

enum VehicleType
{
    Car,
    Coach,
    Motorcycle
}

class MainPageState
{
    public string SelectedValue { get; set; }
}

class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
    {
        return new ContentPage
        {
            new ScrollView
            {
                new VStack(spacing: 25)
                {
                    new VStack(spacing: 5)
                    {
                        new RadioButton("Radio 1"),
                        new RadioButton("Radio 2"),
                        new RadioButton("Radio 3"),
                        new RadioButton("Radio 4")
                            .IsChecked(true)
                    },

                    new VStack
                    {
                        new RadioButton
                        {
                            new Image("icon_email.png")
                        }
                        ,new RadioButton
                        {
                            new Image("icon_lock.png")
                        }
                    },

                    //Under Android you have to define a control template
                    //for an example look at the Styles.xaml
                    new VStack(spacing: 5)
                    {
                        new RadioButton("Radio 1")
                            .Value(State.SelectedValue == "Radio 1")
                            .OnCheckedChanged(()=>SetState(s => s.SelectedValue = "Radio 1")),
                        new RadioButton("Radio 2")
                            .Value(State.SelectedValue == "Radio 2")
                            .OnCheckedChanged(()=>SetState(s => s.SelectedValue = "Radio 2")),
                        new RadioButton("Radio 3")
                            .Value(State.SelectedValue == "Radio 3")
                            .OnCheckedChanged(()=>SetState(s => s.SelectedValue = "Radio 3")),
                        new RadioButton("Radio 4")
                            .Value(State.SelectedValue == "Radio 4")
                            .OnCheckedChanged(()=>SetState(s => s.SelectedValue = "Radio 4")),
                        new Label(State.SelectedValue)
                    },

                    RadioButtonsEnum<VehicleType>()
                }
                .VCenter()
                .Padding(30, 0)
            }
        };
    }

    public VisualNode RadioButtonsEnum<T>() where T : struct, System.Enum
    {
        return new VStack
        {
            Enum.GetValues<T>()
                .Select(_=> new RadioButton(_.ToString()))
        };
    }
}