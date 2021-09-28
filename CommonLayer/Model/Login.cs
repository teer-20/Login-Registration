using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegistration.CommonLayer.Model
{
    public class LoginEmployeeRequest
    {

        public string Email { get; set; }
        public string Password { get; set; }
        //public bool IsSuccess { get; set; }

        public string Role { get; set; }

    }


    public class LoginEmployeeResponse
    {
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string Role{ get; set; }
        public string token { get; set; }

        public bool IsActive { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }

}
