using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReactorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Services.Data;

[Model]
public partial class Account
{
    public Guid Id {  get; set; }

    public required string Email { get; set; }

    public AccountType Type { get; set; }

    public required string Host {  get; set; }

    public required int Port { get; set; } = 993;

    public bool UseSsl { get; set; } = true;

    public string? Username {  get; set; }

    public CommunicationError? Error {  get; set; }

    public bool Initialized {  get; set; }
}


public enum AccountType
{
    IMAP
}

public enum CommunicationError
{
    Initialization,

    Connect,

    PasswordMissing,

    Authentication,

    Generic
}

