using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.Services.Implementation;

class DummyFeedbackService : IFeedbackService
{
    Action<FeedbackType>? _onFeedbackReceivedAction;
    System.Threading.Timer? _timer;
    readonly Random _random = new();

    public void Connect(string videoId)
    {
        _timer ??= new System.Threading.Timer(OnTimeTick, null, 500, 500);
    }

    private void OnTimeTick(object? state)
    {
        var feedbackTypes = Enum.GetValues<FeedbackType>();
        _onFeedbackReceivedAction?.Invoke(feedbackTypes[_random.Next(0, feedbackTypes.Length)]);
    }

    public void OnFeedbackReceived(Action<FeedbackType>? onFeedbackReceivedAction)
    {
        _onFeedbackReceivedAction = onFeedbackReceivedAction;
    }
}
