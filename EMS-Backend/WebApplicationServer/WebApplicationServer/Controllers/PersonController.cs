using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplicationServer.Models;
using Microsoft.Extensions.Configuration;
using WebApplicationServer.Models.ResponseModels;
using Microsoft.AspNetCore.Identity;
using WebApplicationServer.Services.IService;
using WebApplicationServer.Services;
using WebApplicationServer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private string connectionString = "Server=tcp:ems-server.database.windows.net,1433;Initial Catalog=emsdatabase;Persist Security Info=False;User ID=ajaykarode;Password=Emspassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private readonly IGetAllPerson _getAllPerson;
        private readonly UserManager<Person> _userManager;

        public PersonController(UserManager<Person> userManager, IGetAllPerson getAllPerson)
        {
            _getAllPerson = getAllPerson;
            _userManager = userManager;
        }

        [HttpGet, Authorize(Roles = "Organizer")]
        public async Task<GetAllPersonResponseViewModel> GetAllPersons()
        {
            var persons = await _getAllPerson.GetAllPersons();
            return persons;
        }

        [HttpGet("{Id}")]
        public async Task<GetPersonByIdResponseViewModel> GetPersonById(string Id)
        {
            var person = await _getAllPerson.GetPersonById(Id);
            return person;

        }



        [HttpDelete("{Id}")]
        public async Task<ResponseViewModel> DeletePerson(string Id)
        {
            ResponseViewModel response;
            var user = await _userManager.GetUserAsync(User);
            //if (user == null || user.Role != "Organizer")
            //{
            //    response = new ResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are either not loggedIn or You are not Orgainser.";
            //    return response;
            //}

            response = await _getAllPerson.DeletePerson(Id);
            return response;
        }




        [HttpPut("updatePerson/{id}")]
        public async Task<ResponseViewModel> UpdatePerson(string id, UpdatePersonViewModel updatePerson)
        {
            ResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please provide valid Person details.";
                return response;
            }

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null || user.Role != "Organizer")
            //{
            //    response = new ResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are either not logged in or not an organizer.";
            //    return response;
            //}

            response = await _getAllPerson.UpdatePerson(id, updatePerson);
            return response;
        }


    }
}



