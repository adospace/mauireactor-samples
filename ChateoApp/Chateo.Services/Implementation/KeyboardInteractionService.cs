using System;

namespace Chateo.Services.Implementation;

#if IOS

using UIKit;

/// <inheritdoc cref="IKeyboardInteractionService" />
public class KeyboardInteractionService : IKeyboardInteractionService
{
    private float _keyboardHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardInteractionService" /> class.
    /// </summary>
    public KeyboardInteractionService()
    {
        UIKeyboard.Notifications.ObserveWillShow((_, uiKeyboardEventArgs) =>
        {
            _keyboardHeight = (float)uiKeyboardEventArgs.FrameEnd.Height;
            KeyboardHeightChanged?.Invoke(this, _keyboardHeight);
        });


        UIKeyboard.Notifications.ObserveWillHide((_, uiKeyboardEventArgs) =>
        {
            _keyboardHeight = 0;
            KeyboardHeightChanged?.Invoke(this, _keyboardHeight);
        });
    }

    /// <inheritdoc cref="IKeyboardInteractionService.KeyboardHeightChanged" />
    //public Subject<float> KeyboardHeightChanged { get; } = new Subject<float>();

    public float KeyboardHeight => _keyboardHeight;

    public event EventHandler<float>? KeyboardHeightChanged;
}

#elif ANDROID

using System;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using static Android.Views.ViewTreeObserver;

/// <inheritdoc cref="IKeyboardInteractionService" />
public class KeyboardInteractionService : IKeyboardInteractionService
{
    GlobalLayoutListener _globalLayoutListener;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardInteractionService" /> class.
    /// </summary>
    public KeyboardInteractionService()
    {
        _globalLayoutListener = new GlobalLayoutListener();
        _globalLayoutListener.KeyboardHeightChanged += OnKeyboardHeightChanged;
    }

    private void OnKeyboardHeightChanged(object? sender, float e)
    {
        KeyboardHeightChanged?.Invoke(sender, e);
    }

    public float KeyboardHeight => _globalLayoutListener.KeyboardHeight;

    public event EventHandler<float>? KeyboardHeightChanged;
}

/// <inheritdoc cref="IOnGlobalLayoutListener"/>
internal class GlobalLayoutListener : Java.Lang.Object, IOnGlobalLayoutListener, IKeyboardInteractionService
{
    private readonly Activity? activity;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalLayoutListener" /> class.
    /// </summary>
    public GlobalLayoutListener()
    {
        this.activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;

        if (this.activity?.Window?.DecorView?.ViewTreeObserver == null)
        {
            throw new InvalidOperationException($"{this.GetType().FullName}.Constructor: The {nameof(this.activity)} or a follow up variable is null!");
        }

        this.activity.Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
    }

    public float KeyboardHeight { get; private set; }

    public event EventHandler<float>? KeyboardHeightChanged;

    /// <inheritdoc cref="IOnGlobalLayoutListener.OnGlobalLayout" />
    public void OnGlobalLayout()
    {
        var screenSize = new Point();
        this.activity?.WindowManager?.DefaultDisplay?.GetSize(screenSize);
        var screenHeight = screenSize.Y;

        var rootView = this.activity?.FindViewById(Android.Resource.Id.Content);

        if (rootView == null)
        {
            return;
        }

        var screenHeightWithoutKeyboard = new Rect();
        rootView.GetWindowVisibleDisplayFrame(screenHeightWithoutKeyboard);

        double keyboardHeight;

        // Android officially supports display cutouts on devices running Android 9 (API level 28) and higher.
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
        {
            var displayCutout = rootView.RootWindowInsets?.DisplayCutout;

            // Displays with a cutout need to be handled different due to the cutout type:
            //
            // Default cutout:
            // A display has no cutout. The screen height can be used used as usual.
            //
            // Corner cutout:
            // A display has a cutout in a corner on the top of the display. The screen height must add the safe area of the top to get the total screen height.
            //
            // Double cutout:
            // A display has a cutout in the middle on the top and in the middle on the bottom of the display.
            // The screen height must add the safe area of the bottom only to get the total screen height.
            // Adding the screen height of the top as well, would lead to false results.
            //
            // Tall cutout:
            // A display has a cutout in the middle on the top of the display. The screen height must add the safe area of the top to get the total screen height.
            keyboardHeight = displayCutout == null ?
                screenHeight - screenHeightWithoutKeyboard.Bottom :
                displayCutout.SafeInsetBottom <= 0 ?
                    screenHeight + displayCutout.SafeInsetTop - screenHeightWithoutKeyboard.Bottom :
                    screenHeight + displayCutout.SafeInsetBottom - screenHeightWithoutKeyboard.Bottom;
        }
        else
        {
            keyboardHeight = screenHeight - screenHeightWithoutKeyboard.Bottom;
        }

        var keyboardHeightInDip = keyboardHeight /  Resources.System?.DisplayMetrics?.Density ?? 1;

        if (keyboardHeightInDip < 0.0f)
        {
            keyboardHeightInDip = 0.0f;
        }

        this.KeyboardHeight = (float)keyboardHeightInDip;

        KeyboardHeightChanged?.Invoke(this, KeyboardHeight);
    }
}

#else



#endif
