using LoginRegistration.CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LoginRegistration.CommonLayer.Model.GetAdmin;
using static LoginRegistration.CommonLayer.Model.GetUser;

namespace LoginRegistration.RepositoryLayer
{
   public interface IEregRepositoryLayer
    {
        Task<RegisterEmployeeResponse> RegisterEmployee(RegisterEmployeeRequest request);
        Task<LoginEmployeeResponse> LoginEmployee(LoginEmployeeRequest request);

        Task<List<GetAdminResponse>> GetAdmin();
        Task<List<GetUserResponse>> GetUser();

    }
}
