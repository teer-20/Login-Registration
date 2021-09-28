using LoginRegistration.CommonLayer.Exceptions;
using LoginRegistration.CommonLayer.Model;
using LoginRegistration.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static LoginRegistration.CommonLayer.Model.GetAdmin;
using static LoginRegistration.CommonLayer.Model.GetUser;

namespace LoginRegistration.RepositoryLayer
{
    public class EregRepositoryLayer : IEregRepositoryLayer
    {
        public readonly ILogger<EregRepositoryLayer> _logger;

        public readonly IConfiguration _configuration;
        public readonly SqlConnection sqlConnection;
        public EregRepositoryLayer(IConfiguration configuration, ILogger<EregRepositoryLayer> logger)
        {
            _configuration = configuration;
            var ConfigurationDatabase = this.GetDatabaseConfiguration();
            this.sqlConnection = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("DatabaseConnectionString").Value);
            _logger = logger;
        }

        private IConfigurationRoot GetDatabaseConfiguration()
        {
            var DatabaseConnectionBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return DatabaseConnectionBuilder.Build();
        }

        public async Task<RegisterEmployeeResponse> RegisterEmployee(RegisterEmployeeRequest request)
        {
            RegisterEmployeeResponse response = new RegisterEmployeeResponse();
            response.IsSuccess = true;
            try
            {
                    //String SqlQuery = SqlQueries.RegisterEmployee;
                    SqlCommand sqlCommand = new SqlCommand("RegisterEmployee", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Mobile", request.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Email", request.Email);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    string strkey = _configuration["SecurityKey"].ToString();
                    sqlCommand.Parameters.AddWithValue("@Password", Processor.PasswordProcessing.Encrypt(request.Password,strkey ));
                    //sqlCommand.Parameters.AddWithValue("@DoC", req.DoC);
                    sqlCommand.Parameters.AddWithValue("@DoB", request.DoB);
                    sqlCommand.Parameters.AddWithValue("@IsActive", (request.IsActive));
                sqlCommand.Parameters.AddWithValue("@Role", (request.Role));

                sqlConnection.Open();
                    int status = await sqlCommand.ExecuteNonQueryAsync();
                if (status <= 0)
                {
                    response.IsSuccess = false;
                   
                }
                else
                {
                    response.EmployeeName = request.EmployeeName;
                    response.Email = request.Email;
                    response.Mobile = request.Mobile;
                    response.Gender = request.Gender;
                    response.DoB = request.DoB;
                    response.IsActive = request.IsActive;
                    response.Role = request.Role;
                    response.Message = "Registered Successfully";
                }

                //else
                //{
                //    response.IsSuccess = false;

                //}

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                _logger.LogError($"Error Occur in register part");
            }
            finally
            {
                sqlConnection.Close();
            }
            return response;
        }


        public async Task<LoginEmployeeResponse> LoginEmployee(LoginEmployeeRequest request)
        {
            LoginEmployeeResponse response = new LoginEmployeeResponse();
            response.IsSuccess = true;
            response.Email = null;

            try
            {
                
                { 
                      //String SqlQuery = SqlQueries.LoginEmployee;
                        SqlCommand sqlCommand = new SqlCommand("LoginEmployee", sqlConnection);
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandTimeout = 180;
                    //sqlCommand.Parameters.AddWithValue("@Id", request.Id);

                    sqlCommand.Parameters.AddWithValue("@Email", request.Email);
                    
                        sqlCommand.Parameters.AddWithValue("@Password", Processor.PasswordProcessing.Encrypt(request.Password, _configuration["SecurityKey"]).ToString());
                        sqlConnection.Open();
                        using (DbDataReader db = await sqlCommand.ExecuteReaderAsync()) 
                        if (db.HasRows)
                        {
                            await db.ReadAsync();
                            response.EmployeeId = db["EmployeeId"] != DBNull.Value ? Convert.ToInt32(db["EmployeeId"]) : 0;
                            response.Email = db["Email"] != DBNull.Value ? db["Email"].ToString() : null;
                            response.Role = db["Role"] != DBNull.Value ? db["Role"].ToString() : null;
                            response.IsActive = db["IsActive"] != DBNull.Value ? Convert.ToBoolean(db["IsActive"]) : true;
                            response.Message = "Login Successfully";

                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                    
                
                

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                _logger.LogError($"Error Occur in login part");

            }
            finally
            {
                sqlConnection.Close();
            }
            return response;
        }


        //Get Admin
        public async Task<List<GetAdminResponse>> GetAdmin()
        {
            var resultList = new List<GetAdminResponse>();

            try
            {
                _logger.LogInformation("Entering into GetAdmin repository layer");
                SqlCommand sqlCommand = new SqlCommand("GetAdmin", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 180;
                sqlConnection.Open();
                //using (DbDataReader db = await sqlCommand.ExecuteReaderAsync())
                DbDataReader db = await sqlCommand.ExecuteReaderAsync();

                
                    if (db.HasRows)
                    {
                        while (await db.ReadAsync())
                            resultList.Add(new GetAdminResponse()
                            {
                                IsSuccess = true,
                                EmployeeId = db["EmployeeId"] != DBNull.Value ? Convert.ToInt32(db["EmployeeId"]) : 0,
                                EmployeeName = db["EmployeeName"] != DBNull.Value ? (db["EmployeeName"]).ToString() : null,
                                Email = db["Email"] != DBNull.Value ? db["Email"].ToString() : null,
                                Mobile = db["Mobile"] != DBNull.Value ? (db["Mobile"]).ToString() : null,
                                Gender = db["Gender"] != DBNull.Value ? (db["Gender"]).ToString() : null,
                                DoB = db["DoB"] != DBNull.Value ? Convert.ToDateTime(db["DoB"]) : default,
                                Role = db["Role"] != DBNull.Value ? (db["Role"]).ToString() : null,
                                IsActive = db["IsActive"] != DBNull.Value ? Convert.ToBoolean((db["IsActive"])) : false,
                                Message = "Successful"

                            });

                    }
                   
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occur in login part");

            }
            finally
            {
                sqlConnection.Close();
            }

            return resultList;
        }

       
        public async Task<List<GetUserResponse>> GetUser()
        {
            var resultList = new List<GetUserResponse>();

            try
            {
                _logger.LogInformation("Entering into GetUser repository layer");
                SqlCommand sqlCommand = new SqlCommand("GetUser", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 180;
                sqlConnection.Open();
                //using (DbDataReader db = await sqlCommand.ExecuteReaderAsync())
                DbDataReader db = await sqlCommand.ExecuteReaderAsync();


                if (db.HasRows)
                    {
                        while (await db.ReadAsync())

                            resultList.Add(new GetUserResponse()
                        {
                            IsSuccess = true,
                            EmployeeId = db["EmployeeId"] != DBNull.Value ? Convert.ToInt32(db["EmployeeId"]) : 0,
                            EmployeeName = db["EmployeeName"] != DBNull.Value ? (db["EmployeeName"]).ToString() : null,
                            Email = db["Email"] != DBNull.Value ? db["Email"].ToString() : null,
                            Mobile = db["Mobile"] != DBNull.Value ? (db["Mobile"]).ToString() : null,
                            Gender = db["Gender"] != DBNull.Value ? (db["Gender"]).ToString() : null,
                            DoB = db["DoB"] != DBNull.Value ? Convert.ToDateTime(db["DoB"]) : default,
                            Role = db["Role"] != DBNull.Value ? (db["Role"]).ToString() : null,
                            IsActive = db["IsActive"] != DBNull.Value ? Convert.ToBoolean((db["IsActive"])) : false,
                            Message = "Successful"

                        });

                    }

                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlConnection.Close();
            }

            return resultList;
        }

        
    }
}

