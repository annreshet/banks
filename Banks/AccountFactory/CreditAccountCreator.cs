using Banks.Entities;
using Banks.Models;

namespace Banks.AccountFactory;

public class CreditAccountCreator : AccountCreator
{
    public override IAccount CreateAccount(Bank bank, Client client, int id, AccountType type)
    {
        Conditions bankConditions = bank.BankConditions;

        return new CreditAccount(client, id, type, bankConditions.CreditLimit, bankConditions.CreditInterest);
    }
}