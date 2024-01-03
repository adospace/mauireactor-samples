using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor.Shapes;
using SurfingApp.Services;
using SurfingApp.Models;

namespace SurfingApp.Pages;

/*
 NOTE:
 This apps is 1:1 porting of the original Maui SurfingApp
 https://github.com/jsuarezruiz/netmaui-surfing-app-challenge

 The app doesn't use any specific feature of MauiReactor and its
 main purpose is to compare MauiReactor development model with classic
 XAML based approach.

 As the original application, hasn't been adopted any performance
 optimization technique.
 */

class MainPage : Component
{
    public override VisualNode Render()
    {
        return ContentPage(
            Grid("48,Auto,*", "*",
                Grid("*", "Auto,*,Auto",
                    Grid(
                        new Path()
                            .Data("M1.230957,19.693036L30.768982,19.693036C31.506958,19.693036,32,20.185041,32,20.923019L32,22.154038C32,22.893054,31.506958,23.384999,30.768982,23.384999L1.230957,23.384999C0.49194336,23.384999,0,22.893054,0,22.154038L0,20.923019C0,20.185041,0.49194336,19.693036,1.230957,19.693036z M1.230957,9.8470059L30.768982,9.8470059C31.506958,9.8470059,32,10.339011,32,11.076989L32,12.30801C32,13.045987,31.506958,13.53903,30.768982,13.53903L1.230957,13.53903C0.49194336,13.53903,0,13.047025,0,12.30801L0,11.076989C0,10.339011,0.49194336,9.8470059,1.230957,9.8470059z M1.230957,0L30.768982,0C31.506958,-6.3337211E-08,32,0.49298194,32,1.2309594L32,2.4619804C32,3.1999579,31.506958,3.6930011,30.768982,3.6930013L1.230957,3.6930013C0.49194336,3.6930011,0,3.1999579,0,2.4619804L0,1.2309594C0,0.49298194,0.49194336,-6.3337211E-08,1.230957,0z")
                            .Style("HeaderIconStyle")
                    )
                    .Style("HeaderLayoutStyle"),

                    Label("Surfers")
                        .GridColumn(1)
                        .Style("TitleStyle"),

                    Grid(
                        new Path()
                            .Data("M11.170988,2.0000026C6.1139962,2.0000026 1.9999944,6.1120075 1.9999944,11.16603 1.9999944,16.219991 6.1139962,20.331996 11.170988,20.331996 16.227981,20.331996 20.341006,16.219991 20.341006,11.16603 20.341006,6.1120075 16.227981,2.0000026 11.170988,2.0000026z M11.170988,0C17.33003,0 22.341001,5.0089787 22.341001,11.16603 22.341001,13.76351 21.449155,16.156669 19.95551,18.055608L19.942527,18.071714 31.999898,30.615001 30.5589,32.001003 18.567029,19.525854 18.476871,19.605846C16.516895,21.303544 13.961804,22.332 11.170988,22.332 5.0119487,22.332 1.6168633E-07,17.32302 0,11.16603 1.6168633E-07,5.0089787 5.0119487,0 11.170988,0z")
                            .Style("HeaderIconStyle")
                    )
                    .Style("HeaderLayoutStyle")
                    .GridColumn (2)
                ),

                Grid(
                    CollectionView()
                        .ItemsSource(UserService.Instance.GetUsers(), RenderUser)
                        .ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(6))

                )
                .GridRow(1)
                .Padding(24, 0)
                .Margin(0, 18),

                Grid(
                    CollectionView()
                        .ItemsSource(PostService.Instance.GetPosts(), RenderPost)
                        .ItemsLayout(new VerticalLinearItemsLayout().ItemSpacing(36))

                )
                .GridRow(2)
                .Padding(24, 12, 14, 0)
            )
        );
    }

    private VisualNode RenderUser(User user)
    {
        return Grid(
            Border(
                Image(user.Image)
            )
            .Stroke(user.Color)
            .StrokeThickness(4)
            .StrokeShape(new Ellipse())
        )
        .Style("UserLayoutStyle");
    }

    private VisualNode RenderPost(Post post)
    {
        return Grid(
            Border(
                Grid(
                    Image(post.Image)
                        .Aspect(Aspect.AspectFill),
                    Grid()
                        .BackgroundColor(Colors.Black)
                        .Opacity(0.1),
                    Grid("Auto, Auto, *", "72, Auto",
                        Grid(
                            Border(
                                Image(post.User.Image)
                                    .HCenter()
                                    .VCenter()
                                    .Margin(1)
                                    .Clip(new EllipseGeometry()
                                        .Center(25, 25)
                                        .RadiusX(25)
                                        .RadiusY(25))
                            )
                            .StrokeShape(new Ellipse())
                            .Stroke(post.User.Color)
                            .Style("UserBorderStyle")
                        )
                        .HStart(),
                        Grid("Auto, Auto", "*",
                            Label(post.User.Name.ToUpper())
                                .Style("UserNameStyle"),
                            Label("4 HOURS AGO")
                                .GridRow(1)
                                .Style("CreatedAtStyle")
                        )
                        .GridColumn(1)
                        .Margin(0,18),
                        StackLayout(
                            HStack(
                                Path()
                                    .Data("M8.5319849,0C11.560988,7.3866431E-08 14.464015,1.6680007 15.99997,4.5209999 18.139016,0.55099538 22.921994,-1.1230173 27.008008,0.7949839 31.297024,2.8069787 33.185032,8.0069957 31.22604,12.411997 29.27303,16.804 15.998994,30.380001 15.998994,30.380001 15.915985,30.327022 2.7269701,16.804 0.77395964,12.411997 -1.1850933,8.0069957 0.70291448,2.8069787 4.9929701,0.7949839 6.1419612,0.25497467 7.3469826,7.3866431E-08 8.5319849,0z")
                                    .Style("HeartIconStyle"),
                                Label(post.Likes)
                                    .Style("HeartTextStyle")
                            ),
                            Grid(
                                Path()
                                    .Data("M4.2800019,0L11.127999,0C13.491008,0,15.408001,1.9349976,15.408001,4.3209839L15.408001,32 7.5820063,23.104004 0,32 0,4.3209839C2.0228617E-07,1.9349976,1.9169938,0,4.2800019,0z")
                                    .Style("MarkIconStyle")
                            )
                        )
                        .GridRow(1)
                        .Style("InfoLayoutStyle"),
                        Grid("*", "Auto, *",
                            Border(
                                Path()
                                    .Data("M0,0L15.825011,8.0009766 31.650999,15.997986 15.825011,23.998993 0,32 0,15.997986z")
                                    .Style("PlayIconStyle")
                            )
                            .StrokeShape(new Ellipse())
                            .Shadow(new Shadow().Opacity(0.5f).Offset(4,4))
                            .Style("PlayBorderStyle"),

                            StackLayout(
                                Label(post.Title)
                                    .Style("TitleTextStyle")
                                    .Shadow(new Shadow().Opacity(0.5f)),

                                Label(post.User.From.ToUpper())
                                    .Style("FromTextStyle")
                            )
                            .GridColumn(1)
                        )
                        .GridColumnSpan(2)
                        .GridRow(2)
                        .VEnd()
                        .Margin(0,12)

                    )
                )
            )
            .StrokeCornerRadius(12, 120, 12, 12)
            .Shadow(new Shadow().Opacity(0.15f).Offset(1,1))
            .Style("PostBorderStyle")
        );
    }
}