using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinance.Services.DataTransferObjects
{
    public class UserForLoginDto    
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
