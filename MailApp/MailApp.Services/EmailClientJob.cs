using MailKit.Net.Imap;
using MailKit;
using Shiny.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using MailApp.Services.Data;
using ReactorData;
using System.ComponentModel.DataAnnotations;

namespace MailApp.Services;

public class EmailClientJob : IJob
{
    private readonly IQuery<Account> _accounts;
    private readonly IModelContext _modelContext;
    private readonly ISecureStorage _secureStorage;

    public EmailClientJob(IModelContext modelContext, ISecureStorage secureStorage)
    {
        _accounts = modelContext.Query<Account>();
        _accounts.CollectionChanged += OnAccountsListChanged;
        _modelContext = modelContext;
        _secureStorage = secureStorage;
    }

    private void OnAccountsListChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        
    }

    async Task IJob.Run(JobInfo jobInfo, CancellationToken cancelToken)
    {
        try
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await FetchEmails(cancelToken);

                await Task.Delay(100000, cancelToken);

            }
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
        }
    }


    private async Task FetchEmails(CancellationToken cancellationToken)
    {
        foreach (var account in _accounts)
        {
            try
            {
                await FetchEmailsFromAccount(account, cancellationToken);
            }
            catch (Exception)
            {
                account.Error = CommunicationError.Generic;
                _modelContext.Update(account);
                return;
            }
        }
    }


    private async Task FetchEmailsFromAccount(Account account, CancellationToken cancellationToken)
    {
        using var client = new ImapClient();

        var password = await _secureStorage.Get(account.Id.ToString());

        if (password == null)
        {
            account.Error = CommunicationError.PasswordMissing;
            _modelContext.Update(account);
            return;
        }

        try
        {
            await client.ConnectAsync(account.Host, account.Port, account.UseSsl, cancellationToken);
        }
        catch (Exception)
        {
            account.Error = account.Initialized ? CommunicationError.Connect : CommunicationError.Initialization;
            _modelContext.Update(account);
            return;
        }

        try
        {
            await client.AuthenticateAsync(account.Username, password, cancellationToken);
        }
        catch (Exception)
        {
            account.Error = CommunicationError.Authentication;
            _modelContext.Update(account);
            return;
        }

        // The Inbox folder is always available on all IMAP servers...
        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);

        Console.WriteLine("Total messages: {0}", inbox.Count);
        Console.WriteLine("Recent messages: {0}", inbox.Recent);

        for (int i = 0; i < inbox.Count; i++)
        {
            var message = await inbox.GetMessageAsync(i, cancellationToken);
            Console.WriteLine("Subject: {0}", message.Subject);
        }

        await client.DisconnectAsync(true, cancellationToken);
    }
}
