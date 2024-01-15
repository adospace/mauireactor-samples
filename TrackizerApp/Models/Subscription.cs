using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Models;

record Subscription(SubscriptionType Type, double MonthBill, DateOnly StartingDate);


enum SubscriptionType
{
    Spotify,

    [Display(Name = "You Tube Premium")]
    YouTube,

    [Display(Name = "Microsoft One Drive")]
    OneDrive,

    Netflix
}


