using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegistration.CommonLayer.Model
{
    public class RegisterEmployeeRequest
    {

        public string EmployeeName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string CPassword { get; set; }
        public DateTime DoB { get; set; }

        public string Gender { get; set; }


        public bool IsActive { get; set; }
        public string Role { get; set; }

    }
    public class RegisterEmployeeResponse
{
        public string EmployeeName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DoB { get; set; }
        // public DateTime DoC { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }

    }

}
