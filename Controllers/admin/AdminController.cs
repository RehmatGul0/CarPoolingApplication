using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPoolingApp.DataModels;
using CarPoolingApp.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPoolingApp.Controllers.admin
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Route("get-user")]
        public ActionResult<ResponseDTOGet<object>> getUser()
        {
            using (var dbContext = new carpoolingContext())
            {
                try
                {

                    var results = (from client in dbContext.Client
                                   join auth in dbContext.Authdetail on client.AuthId equals auth.Id
                                   select new
                                   {
                                       Id = client.Id,
                                       Name = client.Name,
                                       Gender = client.Gender,
                                       Phone = client.Phone,
                                       Rating = client.Rating,
                                       Email = auth.Email
                                   }).ToArray();
                    return new ResponseDTOGet<object>(400, "success", results);
                }
                catch (Exception ex)
                {
                    return new ResponseDTOGet<object>(400, ex.ToString(), null);
                }
            }
        }

        [HttpGet]
        [Route("get-ride")]
        public ActionResult<ResponseDTOGet<object>> getRide()
        {
            using (var dbContext = new carpoolingContext())
            {
                try
                {
                    var results = (from ride in dbContext.Ride
                                   select new
                                   {
                                       Id = ride.Id,
                                       Time = ride.Time,
                                       Start_loc = ride.StartLoc,
                                       End_loc = ride.EndLoc,
                                       Seats = ride.Seats,
                                       Fees = ride.Fee
                                   }).ToArray();
                    return new ResponseDTOGet<object>(400, "success", results);
                }
                catch (Exception ex)
                {
                    return new ResponseDTOGet<object>(400, ex.ToString(), null);
                }
            }
        }


        [HttpGet]
        [Route("get-trip")]
        public ActionResult<ResponseDTOGet<object>> getTrip()
        {
            using (var dbContext = new carpoolingContext())
            {
                try
                {
                    var results = (from trip in dbContext.Trip
                                   join client in dbContext.Client on trip.PassengerId equals client.Id
                                   select new
                                   {
                                       Id = trip.Id,
                                       Time = trip.Time,
                                       Passenger_name = client.Name,
                                       Seats = trip.Seats,
                                   }).ToArray();
                    return new ResponseDTOGet<object>(400, "success", results);
                }
                catch (Exception ex)
                {
                    return new ResponseDTOGet<object>(400, ex.ToString(), null);
                }
            }
        }
    }
}