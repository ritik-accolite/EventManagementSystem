using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
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
            response.Message = "All Events Fetched";
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

                //if (personToUpdate.EventOrganizerId != userId)
                //{
                //    response.Status = 403;
                //    response.Message = "You are not authorized to update this event";
                //    return response;
                //}

                // Update event properties
                personToUpdate.FirstName = updatePerson.FirstName;
                personToUpdate.LastName = updatePerson.LastName;
                personToUpdate.PhoneNumber = updatePerson.PhoneNumber;
                //personToUpdate.Email = updatePerson.Email;
                

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
    }
}