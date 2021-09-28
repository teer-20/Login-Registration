using LoginRegistration.CommonLayer.Model;
using LoginRegistration.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LoginRegistration.CommonLayer.Model.GetAdmin;
using static LoginRegistration.CommonLayer.Model.GetUser;

//using static LoginRegistration.CommonLayer.Model.Getrequest;

namespace LoginRegistration.Controllers
{
        [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //private const string Roles;
        public readonly IEregServiceLayer _serviceLayer;
        public readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger, IEregServiceLayer serviceLayer)
        {
            _logger = logger;
            _serviceLayer = serviceLayer;

        }
        

        [HttpPost]
        [Route("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployeeDetails(RegisterEmployeeRequest request)
        {
            _logger.LogInformation("Enter in Register controller");
            RegisterEmployeeResponse response = null;
            bool Success = true;

            try
            {
                response = await this._serviceLayer.RegisterEmployee(request);
                if (response.IsSuccess == false)
                {
                    _logger.LogError($"Error Occur");
                    bool Status = false;
                    return BadRequest(new { Status });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Signup Error Occur=>{ex}");
                bool Status = false;
                return BadRequest(new { Status, Message = ex.Message });
            }
            return Ok(new { Success, Message = "Registered successfully", data = response });
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginEmployeeDetails(LoginEmployeeRequest request)
        {
            LoginEmployeeResponse response = null;
            bool LoginSuccess = true;
            _logger.LogInformation("Enter in Login controller");

            try
            {
                response = await this._serviceLayer.LoginEmployee(request);
                if (response.IsSuccess == false)
                {
                    _logger.LogError($"Error Occur in login part");
                    bool Status = false;
                    return BadRequest(new { Status });
                }
                response = await this._serviceLayer.LoginEmployee(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"login Error Occur => {ex}");
                bool Status = false;
                return BadRequest(new { Status, Message = ex.Message });
            }
            return Ok(new { LoginSuccess, Message = "Logged In successfully", data = response });
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        [Route("GetAllAdmin")]
        public async Task<IActionResult> GetAdminDetails()
        {

            var resultList = new List<GetAdminResponse>();
            try
            {
                resultList = await this._serviceLayer.GetAdmin();
            }
            catch (Exception ex)
            {
                _logger.LogError($" Get Admins Exception message:{ex}");
            }
            return Ok(resultList);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetUserDetails()
        {

            var resultList = new List<GetUserResponse>();
            try
            {
                resultList = await this._serviceLayer.GetUser();
            }
            catch (Exception ex)
            {
                _logger.LogError($" GetEmployee Exception message:{ex}");
            }
            return Ok(resultList);
        }
       

    }
}
