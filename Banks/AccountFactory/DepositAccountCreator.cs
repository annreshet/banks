using Banks.Entities;
using Banks.Models;

namespace Banks.AccountFactory;

public class DepositAccountCreator : AccountCreator
{
    public override IAccount CreateAccount(Bank bank, Client client, int id, AccountType type)
    {
        Conditions bankConditions = bank.BankConditions;

        return new DepositAccount(client, id, type, bankConditions.DepositTime, bankConditions.DepositInterests);
    }
}