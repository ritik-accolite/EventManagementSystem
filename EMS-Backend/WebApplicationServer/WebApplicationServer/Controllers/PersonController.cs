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

        [HttpGet]
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



























//        [HttpGet]
//        public IEnumerable<Person> GetPerson()
//        {
//            List<Person> persons = new List<Person>();
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string sqlQuery = "SELECT * FROM Person";
//                SqlCommand command = new SqlCommand(sqlQuery, connection);
//                connection.Open();
//                SqlDataReader reader = command.ExecuteReader();
//                while (reader.Read())
//                {
//                    Person person = new Person
//                    {
//                        PersonId = Convert.ToInt32(reader["PersonId"]),
//                        FirstName = Convert.ToString(reader["FirstName"]),
//                        LastName = Convert.ToString(reader["LastName"]),
//                        Email = Convert.ToString(reader["Email"]),
//                        Password = Convert.ToString(reader["Password"]),
//                        PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
//                        Role = Convert.ToString(reader["Role"])
//                        // Populate other properties accordingly
//                    };
//                    persons.Add(person);
//                }
//                reader.Close();
//            }
//            return persons;
//        }




//[HttpGet("{id}")]
//public IActionResult GetPerson(int id)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        string sqlQuery = "SELECT * FROM Person WHERE PersonId = @PersonId";
//        SqlCommand command = new SqlCommand(sqlQuery, connection);
//        command.Parameters.AddWithValue("@PersonId", id); // Use the id parameter to identify the person to retrieve
//        connection.Open();
//        SqlDataReader reader = command.ExecuteReader();
//        if (reader.Read())
//        {
//            Person person = new Person
//            {
//                PersonId = (int)reader["PersonId"],
//                FirstName = reader["FirstName"].ToString(),
//                LastName = reader["LastName"].ToString(),
//                Email = reader["Email"].ToString(),
//                Password = reader["Password"].ToString(),
//                PhoneNumber = reader["PhoneNumber"].ToString(),
//                Role = reader["Role"].ToString()
//            };
//            return Ok(person); // Found the person, return it
//        }
//        else
//        {
//            return NotFound(); // No person found with the provided id
//        }
//    }
//}


//        [HttpPost]
//        public IActionResult PostPerson([FromBody] Person person)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string sqlQuery = "INSERT INTO Person (FirstName, LastName,Email,Password,PhoneNumber,Role) VALUES (@Value1,@Value2,@Value3,@Value4,@Value5,@Value6)";
//                SqlCommand command = new SqlCommand(sqlQuery, connection);
//                command.Parameters.AddWithValue("@Value1", person.FirstName); // Replace Value1 with the actual property of Admin
//                command.Parameters.AddWithValue("@Value2", person.LastName);
//                command.Parameters.AddWithValue("@Value3", person.Email);
//                command.Parameters.AddWithValue("@Value4", person.Password);
//                command.Parameters.AddWithValue("@Value5", person.PhoneNumber);
//                command.Parameters.AddWithValue("@Value6", person.Role);
//                // Set parameter values
//                connection.Open();
//                command.ExecuteNonQuery();
//                connection.Close();
//            }
//            return Ok();
//        }

//        [HttpPut("{id}")]
//        public IActionResult UpdatePerson(int id, [FromBody] Person person)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string sqlQuery = "UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, Role = @Role WHERE PersonId = @PersonId";
//                SqlCommand command = new SqlCommand(sqlQuery, connection);
//                command.Parameters.AddWithValue("@FirstName", person.FirstName);
//                command.Parameters.AddWithValue("@LastName", person.LastName);
//                command.Parameters.AddWithValue("@Email", person.Email);
//                command.Parameters.AddWithValue("@Password", person.Password);
//                command.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber);
//                command.Parameters.AddWithValue("@Role", person.Role);
//                command.Parameters.AddWithValue("@PersonId", id); // Use the id parameter to identify the person to update
//                connection.Open();
//                int rowsAffected = command.ExecuteNonQuery();

//                if (rowsAffected == 0)
//                {
//                    return NotFound(); // No rows updated, person not found
//                }
//            }
//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public IActionResult DeletePerson(int id)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string sqlQuery = "DELETE FROM Person WHERE PersonId = @PersonId";
//                SqlCommand command = new SqlCommand(sqlQuery, connection);
//                command.Parameters.AddWithValue("@PersonId", id); // Use the id parameter to identify the person to delete
//                connection.Open();
//                int rowsAffected = command.ExecuteNonQuery();
//                if (rowsAffected == 0)
//                {
//                    return NotFound(); // No rows deleted, person not found
//                }
//            }
//            return NoContent(); // Successfully deleted the person
//        }





