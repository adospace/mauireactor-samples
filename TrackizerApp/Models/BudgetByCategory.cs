using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Models;

record BudgetByCategory(Category Category, double MonthBills, double MonthBudget);

enum Category
{
    [Display(Name = "Auto & Transport")]
    AutoTransport,

    Entertainment,

    Security
}
