using Banks.Tools;

namespace Banks.Entities;

public class Person
{
    private const long MinimalPassportNumber = 1000000000;
    private const long MaximalPassportNumber = 9999999999;
    private List<Client> _clients = new ();
    public Person(string name, string surname, string? address, long? passport)
    {
        Name = name;
        Surname = surname;
        Address = address;
        Passport = passport;
    }

    public Person()
    {
        Name = string.Empty;
        Surname = string.Empty;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? Address { get; private set; }
    public long? Passport { get; private set; }
    public IReadOnlyCollection<Client> BankClients => _clients;

    public void AddClient(Client client)
    {
        if (client is null)
        {
            throw new NullReferenceException(nameof(client));
        }

        _clients.Add(client);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BanksException("Name cannot be empty");
        }

        Name = name;
    }

    public void SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new BanksException("Surname cannot be empty");
        }

        Surname = surname;
    }

    public void SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BanksException("Address cannot be empty.");
        }

        Address = address;
    }

    public void SetPassport(long passport)
    {
        if (passport is < MinimalPassportNumber or > MaximalPassportNumber)
        {
            throw new BanksException("Invalid passport number");
        }

        Passport = passport;
    }
}