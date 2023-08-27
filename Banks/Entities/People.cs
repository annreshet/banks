using Banks.Tools;

namespace Banks.Entities;

public sealed class People
{
    private static People? _people;
    private List<Person> _peopleList;
    private People()
    {
        _peopleList = new List<Person>();
    }

    public static People GetPeople()
    {
        return _people ??= new People();
    }

    public void AddPerson(Person person)
    {
        _peopleList.Add(person);
    }

    public Person GetPerson(string name, string surname)
    {
        Person? person = _peopleList.SingleOrDefault(person => person.Name == name && person.Surname == surname);
        if (person is null)
        {
            throw new BanksException("There is no person registered with that name");
        }

        return person;
    }
}