using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
using WebApplicationServer.Models;

namespace WebApplicationServer.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _context;

        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetPerson(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task<Person> CreatePerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdatePerson(int id, Person person)
        {
            var existingPerson = await _context.People.FindAsync(id);
            if (existingPerson == null)
            {
                return null;
            }
            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.Email = person.Email;
            existingPerson.Password = person.Password;
            existingPerson.PhoneNumber = person.PhoneNumber;
            existingPerson.Role = person.Role;

            await _context.SaveChangesAsync();
            return existingPerson;
        }

        public async Task<bool> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return false;
            }
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
