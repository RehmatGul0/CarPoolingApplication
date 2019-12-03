using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using CarPoolingApp.DataModels;
using CarPoolingApp.DataTransferObjects;
using CarPoolingApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CarPoolingApp.Controllers.admin
{
 
    [EnableCors("AllowMyOrigin")]
    [Route("api/admin/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        PasswordManagment passwordManager = new PasswordManagment();
        SessionManagement sessioNManager = new SessionManagement();
        public AuthController()
        {
        }
        [HttpPost]
        [Route("login")]
        public ActionResult<ResponseDTO> Login([FromBody]LoginRequestDTO loginRequest)
        {
            using (var transaction = new TransactionScope())
            {
                using (var dbContext = new carpoolingContext())
                {
                    try
                    {
                        Authdetail authDetail = dbContext.Authdetail.FirstOrDefault(authDetail => authDetail.Email == loginRequest.email);
                        if (authDetail == null)
                            throw new Exception("Email not found");
                        if (!passwordManager.verifyHash(loginRequest.password, authDetail.Password, authDetail.Salt))
                            throw new Exception("Incorrect password");
                        string sessionId = sessioNManager.getSessionID(loginRequest.email, authDetail.Salt);
                        Sessiondetail oldSession = dbContext.Sessiondetail.FirstOrDefault(session => (session.AuthId == authDetail.Id && session.IsActive == true));
                        if (oldSession != null)
                        {
                            oldSession.IsActive = false;
                            dbContext.Sessiondetail.Update(oldSession);
                            dbContext.SaveChanges();
                        }
                            
                        Sessiondetail session = new Sessiondetail
                        {
                            IsActive = true,
                            StartTime = DateTime.Now,
                            EndTime = DateTime.Now.AddMonths(6),
                            AuthId = authDetail.Id,
                            SessionId = sessionId
                        };
                        dbContext.Sessiondetail.Add(session);
                        dbContext.SaveChanges();
                        return new ResponseDTO(200, "success");

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
        [Route("register")]
        public ActionResult<ResponseDTO> Register([FromBody] AdminRegisterationRequestDTO registrationRequest)
        {
            using (var transaction = new TransactionScope())
            {
                using (var dbContext = new carpoolingContext())
                {
                    try
                    {
                        PasswordManagment manager = new PasswordManagment();
                        string salt;
                        Authdetail auth = new Authdetail
                        {
                            Email=registrationRequest.email,
                            Password= manager.generateHash(registrationRequest.password, out salt),
                            Salt=salt
                        };
                        dbContext.Authdetail.Add(auth);
                        dbContext.SaveChanges();
                        Administrator admin = new Administrator
                        {
                            AuthId = auth.Id
                        };
                        dbContext.Administrator.Add(admin);
                        dbContext.SaveChanges();
                        return new ResponseDTO(200, "success");

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