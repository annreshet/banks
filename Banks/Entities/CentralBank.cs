using Banks.AccountFactory;
using Banks.Models;
using Banks.Tools;

namespace Banks.Entities;

public sealed class CentralBank
{
    private static CentralBank? _centralBank;
    private readonly List<Bank> _banks = new ();
    private CentralBank()
    {
    }

    public static CentralBank GetCentralBank()
    {
        return _centralBank ??= new CentralBank();
    }

    public Bank CreateBank(string name, Conditions bankConditions)
    {
        var bank = new Bank(name, bankConditions);
        _banks.Add(bank);
        return bank;
    }

    public void Transfer(string senderBankName, string? recipientBankName, int senderAccountId, int recipientAccountId, double amount)
    {
        Bank senderBank = _banks.Single(bank => bank.Name == senderBankName);
        Bank? recipientBank = _banks.FirstOrDefault(bank => bank.Name == recipientBankName);

        if (recipientBank is null)
        {
            throw new BanksException("There is no recipient bank with that name");
        }

        IEnumerable<IAccount> recipientBankAccounts = recipientBank.Clients.SelectMany(client => client.Accounts);
        IEnumerable<IAccount> senderBankAccounts = senderBank.Clients.SelectMany(client => client.Accounts);
        IAccount? recipientAccount = recipientBankAccounts.FirstOrDefault(account => account.GetId() == recipientAccountId);
        IAccount? senderAccount = senderBankAccounts.FirstOrDefault(account => account.GetId() == senderAccountId);
        if (recipientAccount is null)
        {
            throw new BanksException("Recipient account with that ID doesn't exist");
        }

        if (senderAccount is null)
        {
            throw new BanksException("Recipient account with that ID doesn't exist");
        }

        senderAccount.Withdraw(amount);
        recipientAccount.Add(amount);
        senderAccount.AddTransaction(new Transaction(TransactionType.Withdraw, amount));
        recipientAccount.AddTransaction(new Transaction(TransactionType.Add, amount));
    }

    public void ApplyConditions(TimeSpan timeSpan)
    {
        IEnumerable<IAccount> accounts = _banks.SelectMany(bank => bank.Clients).SelectMany(client => client.Accounts);
        foreach (IAccount account in accounts)
        {
            int daysCounter = 0;
            for (int i = 0; i < timeSpan.Days; ++i)
            {
                account.ApplyEverydayConditions();
                daysCounter++;

                if (daysCounter != 30) continue;
                account.ApplyConditions();
                daysCounter = 0;
            }
        }
    }

    public Bank GetBank(string? bankName)
    {
        if (string.IsNullOrWhiteSpace(bankName))
        {
            throw new BanksException("Bank name cannot be empty");
        }

        Bank? bank = _banks.Find(bank => bank.Name == bankName);
        if (bank is null)
        {
            throw new BanksException("There is no bank with that name.");
        }

        return bank;
    }
}