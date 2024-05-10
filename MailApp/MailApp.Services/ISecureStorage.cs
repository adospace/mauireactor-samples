using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Services;

public interface ISecureStorage
{
    Task<string?> Get(string key);

    Task Set(string key, string value);

}
