using Banks.Entities;

namespace Banks.ClientBuilder;

public interface IBuilder
{
    void SetName(string name);
    void SetSurname(string surname);
    void SetAddress(string address);
    void SetPassport(long passport);
    Client GetClient();
}