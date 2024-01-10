using ReactorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models;

[Model]
partial class Todo
{
    public int Id { get; set; }

    public required string Task { get; set; }

    public bool Done {  get; set; }
}
