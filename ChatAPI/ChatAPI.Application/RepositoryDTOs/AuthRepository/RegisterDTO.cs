using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Application.RepositoryDTOs.AuthRepository
{
    public class RegisterDTO
    {
        public class Request
        {
            public Request()
            {
                FireToken = String.Empty;
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string FireToken { get; set; }
        }
       
    }
}
