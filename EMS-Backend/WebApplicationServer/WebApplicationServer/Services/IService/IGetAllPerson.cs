using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IGetAllPerson
    {
        public Task<GetAllPersonResponseViewModel> GetAllPersons();

        public Task<GetPersonByIdResponseViewModel> GetPersonById(string id);
        public Task<ResponseViewModel> DeletePerson(string id);

        public Task<ResponseViewModel> UpdatePerson(string id, UpdatePersonViewModel updatePerson);
    }
}


