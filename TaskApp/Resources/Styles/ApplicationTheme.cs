using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Resources.Styles;

class ApplicationTheme : Theme
{
    public static Color Black => IsLightTheme ?
        Color.FromRgba("#0E100F") : Color.FromRgba("#FFFFFF");

    public static Color White => IsLightTheme ?
        Color.FromRgba("#FFFFFF") : Color.FromRgba("#0E0F10");

    public static Color LightGray => IsLightTheme ?
        Color.FromRgba("#F1F3F3") : Color.FromRgba("#181A1B");

    public static Color Gray => IsLightTheme ?
        Color.FromRgba("#C0C3C9") : Color.FromRgba("#31343A");

    public static Color Accent => IsLightTheme ?
        Color.FromRgba("#00A86B") : Color.FromRgba("#05BD7A");

    public static Color DarkGray => IsLightTheme ?
        Color.FromRgba("#7E8491") : Color.FromRgba("#4D525B");

    public static Color MediumGray => IsLightTheme ?
        Color.FromRgba("#F4F6F6") : Color.FromRgba("#131416");

    public static Color Red => IsLightTheme ?
        Color.FromRgba("#F44725") : Color.FromRgba("#CD5037");

    public static class Selector
    {
        public const string Header = nameof(Header);

        public const string CalendarItem = nameof(CalendarItem);

        public const string Body = nameof(Body);

        public const string Primary = nameof(Primary);

        public const string Normal = nameof(Normal);
    }


    protected override void OnApply()
    {
        LabelStyles.Default = _ => _
           .FontFamily("PlusJakartaSansMedium")
           .TextColor(Black)
           .FontSize(14);

        LabelStyles.Themes[Selector.Header] = _ => _
            .FontFamily("PlusJakartaSansSemiBold")
            .FontSize(20);

        LabelStyles.Themes[Selector.CalendarItem] = _ => _
            .FontFamily("PlusJakartaSansSemiBold")
            .FontSize(18);

        LabelStyles.Themes[Selector.Body] = _ => _
            .FontFamily("PlusJakartaSansMedium")
            .FontSize(10)
            .TextColor(DarkGray)
            ;

        LabelStyles.Themes[Selector.Normal] = _ => _
            .FontFamily("PlusJakartaSansMedium")
            .FontSize(12);


        ContentPageStyles.Default = _ => _
            .BackgroundColor(White)
            .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);

        ImageButtonStyles.Default = _ => _
            .CornerRadius(10)
            .Aspect(Aspect.Center)
            .IsEnabled(true);

        ImageButtonStyles.Themes[Selector.Primary] = _ => _
            .BackgroundColor(Accent);

        ButtonStyles.Default = _ => _
            .CornerRadius(10)
            .TextColor(DarkGray)
            .BackgroundColor(MediumGray);

        ButtonStyles.Themes[Selector.Primary] = _ => _
            .BackgroundColor(Accent);

        EntryStyles.Default = _ => _
            .PlaceholderColor(Gray);

    }
}
