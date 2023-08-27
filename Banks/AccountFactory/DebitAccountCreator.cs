using Banks.Entities;
using Banks.Models;

namespace Banks.AccountFactory;

public class DebitAccountCreator : AccountCreator
{
    public override IAccount CreateAccount(Bank bank, Client client, int id, AccountType type)
    {
        Conditions bankConditions = bank.BankConditions;

        return new DebitAccount(client, id, type, bankConditions.DebitInterest);
    }
}