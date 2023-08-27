using Banks.Entities;
using Banks.Models;
using Banks.Tools;

namespace Banks.AccountFactory;

public interface IAccount
{
    AccountType GetAccountType();
    int GetId();
    void ApplyEverydayConditions();
    void ApplyConditions();
    void Withdraw(double amount);
    void Add(double amount);
    void Transfer(int recipientId, string bankName, double amount);
    void CancelLastTransaction();
    void UpdateConditions(Conditions conditions);
    void AddTransaction(Transaction transaction);
    double GetBalance();
}