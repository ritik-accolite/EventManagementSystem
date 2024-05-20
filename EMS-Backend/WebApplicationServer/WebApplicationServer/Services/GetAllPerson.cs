using Azure;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Services
{
    public class GetAllPerson : IGetAllPerson
    {

        private readonly ApplicationDbContext _context;


        public GetAllPerson(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllPersonResponseViewModel> GetAllPersons()
        {
            GetAllPersonResponseViewModel response = new GetAllPersonResponseViewModel();
            response.Status = 200;
            response.Message = "All Persons Fetched";
            response.AllPersons= await _context.Users.ToListAsync();
            return response;
        }

        public async Task<GetPersonByIdResponseViewModel> GetPersonById(string Id)
        {
            GetPersonByIdResponseViewModel response = new GetPersonByIdResponseViewModel();
            response.Status = 200;
            response.Message = "All Events Fetched";
            response.GetPersonById = await _context.Users.FindAsync(Id);
            return response;
        }

        public async Task<List<GetAllPersonByAdminViewModel>> GetPersonByRole(string role)
        {

            var allperson = await _context.Users.Where(e => e.Role == role)
                .Select(e => new GetAllPersonByAdminViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Role = e.Role,
                    PhoneNumber = e.PhoneNumber,
                    IsBlocked = e.IsBlocked

                }).ToListAsync();

            return allperson;
            //var allperson = await _context.Users.Where(e => e.Role == role).ToListAsync();
            //return allperson;

            //GetAllPersonResponseViewModel response = new GetAllPersonResponseViewModel();
            //response.Status = 200;
            //response.Message = "All Persons Fetched";
            //response.AllPersons = await _context.Users.Where(e => e.Role == role).ToListAsync();
            //return response;
        }

    



        public async Task<ResponseViewModel> DeletePerson(string Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            try
            {
                var personToDelete = await _context.Users.FindAsync(Id);
                if (personToDelete == null)
                {
                    response.Status = 404;
                    response.Message = "Event not found";
                }
                else
                {
                    _context.Users.Remove(personToDelete);
                    await _context.SaveChangesAsync();
                    response.Status = 200;
                    response.Message = "Person deleted successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error deleting Person: {ex.Message}";
            }

            return response;
        }


        public async Task<ResponseViewModel> UpdatePerson(string id, UpdatePersonViewModel updatePerson)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var personToUpdate = await _context.Users.FindAsync(id);
                if (personToUpdate == null)
                {
                    response.Status = 404;
                    response.Message = "Event not found";
                    return response;
                }

                // Update event properties
                personToUpdate.FirstName = updatePerson.FirstName;
                personToUpdate.LastName = updatePerson.LastName;
                personToUpdate.PhoneNumber = updatePerson.PhoneNumber;

                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Person successfully updated";
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error updating Person data: {ex.Message}";
            }

            return response;
        }



        public async Task<ResponseViewModel> BlockPerson(string personId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var person = await _context.Users.FindAsync(personId);

            if (person == null)
            {
                response.Status = 400;
                response.Message = $"Person with ID '{personId}' not found.";
                return response;
            }

            person.IsBlocked = true;
            
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = $"Person with ID '{personId}' has been blocked successfully.";
                return response;        
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Failed to block person with ID '{personId}'. Error: {ex.Message}";
                return response;
            }
        }

        public async Task<ResponseViewModel> UnBlockPerson(string personId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var person = await _context.Users.FindAsync(personId);

            if (person == null)
            {
                response.Status = 400;
                response.Message = $"Person with ID '{personId}' not found.";
            }

            person.IsBlocked = false;
         
            
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = $"Person with ID '{personId}' has been unblocked successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Failed to block person with ID '{personId}'. Error: {ex.Message}";
                return response;
            }
        }
    }
}