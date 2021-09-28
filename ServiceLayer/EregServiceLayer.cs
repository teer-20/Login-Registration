using LoginRegistration.CommonLayer.Exceptions;
using LoginRegistration.CommonLayer.Model;
using LoginRegistration.RepositoryLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LoginRegistration.CommonLayer.Model.GetAdmin;
using static LoginRegistration.CommonLayer.Model.GetUser;

namespace LoginRegistration.ServiceLayer
{
    public class EregServiceLayer : IEregServiceLayer
    {

        public readonly ILogger<EregServiceLayer> _logger;
        public readonly IConfiguration _configuration;

        public readonly IEregRepositoryLayer _EregRepositoryLayer;
        public readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public readonly string Mobilenumber = @"^[2-9]{2}[0-9]{8}$";
        public EregServiceLayer(IEregRepositoryLayer EregRepositoryLayer, ILogger<EregServiceLayer> logger, IConfiguration configuration)  //Dependency Injection /Constructor
        {
            _logger = logger;
            _EregRepositoryLayer = EregRepositoryLayer;
            _configuration = configuration;
        }


        public async Task<RegisterEmployeeResponse> RegisterEmployee(RegisterEmployeeRequest request)
        {
            RegisterEmployeeResponse response = new RegisterEmployeeResponse();

            try
            {
                if (String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Password) || String.IsNullOrEmpty(request.Mobile)  || String.IsNullOrEmpty(request.EmployeeName) || String.IsNullOrEmpty((request.DoB).ToString()) || String.IsNullOrEmpty(request.Gender))
                {
                    throw new Exception(CustomExceptions.ExceptionType.Null_Empty_String_Exception.ToString());
                }

                Regex regexMail = new Regex(strRegex);
                //RegexOptions.CultureInvariant | RegexOptions.Singleline;
                if (!(regexMail.IsMatch(request.Email)))
                {
                    response.Message = "Invalid Mail-ID";
                    response.IsSuccess = false;
                    return response;
                }

                Regex regexMobile = new Regex(Mobilenumber);
                if (!regexMobile.IsMatch(request.Mobile))
                {
                    response.Message = "Invalid Mobile";
                    response.IsSuccess = false;
                    return response;

                }
                // Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
                Regex regexPassword = new Regex(@"^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^\w\s])\S{8,}$");
                if (!regexPassword.IsMatch(request.Password))
                {
                    response.Message = "Invalid Password: Minimum 8 characters, atleast 1 uppercase, 1 lowercase, 1 number and 1 special character";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occur in Signup validation part");

                response.IsSuccess = false;
            }

            response = await _EregRepositoryLayer.RegisterEmployee(request);
            return response;


        }
        public async Task<LoginEmployeeResponse> LoginEmployee(LoginEmployeeRequest request)
        {
            LoginEmployeeResponse response = new LoginEmployeeResponse();

            try
            {
                if (String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Password))
                {
                    throw new Exception(CustomExceptions.ExceptionType.Password_Not_Match_Exception.ToString());
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                _logger.LogError($"Error Occur in login validation part");

            }

            response = await _EregRepositoryLayer.LoginEmployee(request);
            if (response.IsSuccess == true){

                string key = _configuration["Jwt:Key"];
                string Issuer = _configuration["Jwt:Issuer"];
                response.token= Processor.TokenProcessing.CreateToken(response.Role, response.Email, key, Issuer);
            }
            return response;
        }


        public async Task<List<GetAdminResponse>> GetAdmin()
        {
            var resultList = new List<GetAdminResponse>();
            resultList = await _EregRepositoryLayer.GetAdmin();
            return resultList;
        }

        public async Task<List<GetUserResponse>> GetUser()
        {
            var resultList = new List<GetUserResponse>();
            resultList = await _EregRepositoryLayer.GetUser();
            return resultList;
        }
      
    }
}
