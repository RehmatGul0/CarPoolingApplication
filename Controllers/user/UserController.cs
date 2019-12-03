using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using CarPoolingApp.DataModels;
using CarPoolingApp.DataTransferObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace CarPoolingApp.Controllers.user
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        [Route("get")]
        public ActionResult<ResponseDTOGet<GetUserResponseDTO>> Get([FromBody]GetRequestDTO getUserRequest)
        {
          
            using(var dbContext = new carpoolingContext())
            {
                try
                {
                    var result = (from client in dbContext.Client
                                join auth in dbContext.Authdetail on client.AuthId equals auth.Id
                                join session in dbContext.Sessiondetail on auth.Id equals session.AuthId
                                where session.SessionId == getUserRequest.session_id && auth.Email == getUserRequest.email
                                select new { name = client.Name, phone = client.Phone, rating = client.Rating,
                                    gender = client .Gender, isActive = session.IsActive, endTime = session.EndTime ,isDriver=client.IsDriver }).First();
                
                    if (result == null)
                        throw new System.InvalidOperationException("no data found");

                    if ((bool)result.isActive && (DateTime)result.endTime > DateTime.Now)
                    {
                        GetUserResponseDTO userData = new GetUserResponseDTO((string)result.name, (string)result.phone, (int)result.rating, (string)result.gender,(bool)result.isDriver);
                        return new ResponseDTOGet<GetUserResponseDTO>(200, "success", userData);
                    }
                    else
                        throw new System.InvalidOperationException("incorrect session id");

                }
                catch (Exception e)
                {
                    return new ResponseDTOGet<GetUserResponseDTO>(400, e.ToString(), null);
                }

            }
        }

        [HttpPost]
        [Route("driver/set")]
        public ActionResult<ResponseDTO> setDriver([FromBody]BecomeDriverRequestDTO driverRequestDTO)
        {
            using (var transaction = new TransactionScope())
            {
                using (var dbContext = new carpoolingContext())
                {
                    try
                    {
                        object user_id;
                        using (var connection = (SqlConnection)dbContext.Database.GetDbConnection())
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "getUserIdFromSession";

                            command.Parameters.AddWithValue("@email", driverRequestDTO.email);
                            command.Parameters.AddWithValue("@session_id", driverRequestDTO.session_id);
                            command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                            user_id = command.ExecuteScalar();
                            if (user_id == null)
                                throw new Exception();

                            Client client = dbContext.Client.FirstOrDefault(client => client.Id == (long)user_id);
                            client.IsDriver = true;
                            dbContext.Client.Update(client);
                            dbContext.SaveChanges();

                            Vehicle vehicle = new Vehicle
                            {
                                UserId = (long)user_id,
                                Model = driverRequestDTO.model,
                                Description = driverRequestDTO.description,
                                Plate = driverRequestDTO.plate
                            };
                            dbContext.Vehicle.Add(vehicle);
                            dbContext.SaveChanges();

                            transaction.Complete();
                            connection.Close();
                            return new ResponseDTO(200, "success");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        return new ResponseDTO(400, ex.ToString());
                    }
                }
            }
        }

        [HttpPost]
        [Route("preference/set")]
        public ActionResult<ResponseDTO> setPreference([FromBody]SetPreferenceRequestDTO preferencesRequestDTO)
        {
            using (var transaction = new TransactionScope())
            {
                using (var dbContext = new carpoolingContext())
                {
                    try
                    {
                        object user_id;
                        using (var connection = (SqlConnection)dbContext.Database.GetDbConnection())
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "getUserIdFromSession";

                            command.Parameters.AddWithValue("@email", preferencesRequestDTO.email);
                            command.Parameters.AddWithValue("@session_id", preferencesRequestDTO.session_id);
                            command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                            user_id = command.ExecuteScalar();
                            if (user_id == null)
                                throw new Exception("Invalid input");
                        
                            Preferences preferences = new Preferences
                            {
                                Gender= preferencesRequestDTO.gender,
                                Notification=preferencesRequestDTO.notification,
                                UserId= (long)user_id

                            };
                            dbContext.Preferences.Add(preferences);
                            dbContext.SaveChanges();

                            transaction.Complete();
                            connection.Close();
                            return new ResponseDTO(200, "success");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        return new ResponseDTO(400, ex.ToString());
                    }
                }
            }
        }
    }
}