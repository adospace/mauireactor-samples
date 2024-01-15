using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Models;

class User
{
    public bool IsLoggedIn { get; set; } = true;

    public string? Email { get; set; }

    public string? Name { get; set; }
}