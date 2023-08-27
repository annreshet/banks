using Banks.Entities;

namespace Banks.ClientBuilder;

public class ClientBuilder : IBuilder
{
    private string _name;
    private string _surname;
    private string? _address;
    private long? _passport;

    public ClientBuilder()
    {
        _name = string.Empty;
        _surname = string.Empty;
        Reset();
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetSurname(string surname)
    {
        _surname = surname;
    }

    public void SetAddress(string address)
    {
        _address = address;
    }

    public void SetPassport(long passport)
    {
        _passport = passport;
    }

    public Client GetClient()
    {
        var client = new Client(_name, _surname, _address, _passport);
        Reset();
        return client;
    }

    private void Reset()
    {
        _name = string.Empty;
        _surname = string.Empty;
        _address = null;
        _passport = null;
    }
}