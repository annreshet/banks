using Banks.Entities;
using Banks.Models;

namespace Banks.AccountFactory;

public abstract class AccountCreator
{
    public abstract IAccount CreateAccount(Bank bank, Client client, int id, AccountType type);
}