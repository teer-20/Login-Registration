using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegistration.CommonLayer.Model
{
    public class GetUser
    {
        public class GetUserResponse
        {
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public DateTime DoB { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }
    }
}
