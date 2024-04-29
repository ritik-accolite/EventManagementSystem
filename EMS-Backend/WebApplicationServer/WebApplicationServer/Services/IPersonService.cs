using WebApplicationServer.Models;

namespace WebApplicationServer.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(int id);
        Task<Person> CreatePerson(Person person);
        Task<Person> UpdatePerson(int id, Person person);
        Task<bool> DeletePerson(int id);
    }
}
