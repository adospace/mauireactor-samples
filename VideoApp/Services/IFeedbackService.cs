using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.Services;

enum FeedbackType
{
    Like,

    Love,

    Face
}

interface IFeedbackService
{
    void Connect(string videoId);

    void OnFeedbackReceived(Action<FeedbackType>? onFeedbackReceivedAction);
}
