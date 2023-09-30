using System;
namespace Chateo.Services;

public interface IKeyboardInteractionService
{
    float KeyboardHeight { get; }

    event EventHandler<float> KeyboardHeightChanged;
}

