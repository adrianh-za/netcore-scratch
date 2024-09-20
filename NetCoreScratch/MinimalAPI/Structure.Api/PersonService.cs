namespace Structure.Api;

public class PersonService
{
    private readonly List<Person> _people = new()
    {
        new Person { Id = 1, Name = "John Doe", Age = 30 },
        new Person { Id = 2, Name = "Jane Doe", Age = 25 },
        new Person { Id = 3, Name = "Joe Bloggs", Age = 40 }
    };

    public List<Person> GetPeople() => _people;

    public Person? GetPerson(int id) => _people.FirstOrDefault(p => p.Id == id);

    public void AddPerson(Person person)
    {
        person.Id = _people.Max(p => p.Id) + 1;
        _people.Add(person);
    }

    public void RemovePerson(int id)
    {
        var person = _people.FirstOrDefault(p => p.Id == id);
        if (person is not null)
            _people.Remove(person);
    }

    public List<Person> GetPeople(string searchTerm)
    {
        return _people.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}