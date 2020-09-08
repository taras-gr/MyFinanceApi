using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinance.Services.DataTransferObjects
{
    public class UserForCreationDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
