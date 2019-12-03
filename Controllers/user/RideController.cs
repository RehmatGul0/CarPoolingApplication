using System;
using Microsoft.EntityFrameworkCore;
using CarPoolingApp.DataModels;
using CarPoolingApp.DataTransferObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using LocationObj = CarPoolingApp.DataTransferObjects.Location;
using Location = CarPoolingApp.DataModels.Location;
using System.Collections.Generic;
using System.Transactions;
using CarPoolingApp.Services;

namespace CarPoolingApp.Controllers.user
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/user/ride")]
    [ApiController]
    public class RideController : ControllerBase
    {
        public RideController()
        {
        }
        [HttpPost]
        [Route("add")]
        public ActionResult<ResponseDTO> addRide([FromBody]AddRideRequestDTO rideRequestDTO)
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

                            command.Parameters.AddWithValue("@email", rideRequestDTO.email);
                            command.Parameters.AddWithValue("@session_id", rideRequestDTO.session_id);
                            command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                            user_id = command.ExecuteScalar();
                            if (user_id == null)
                                throw new Exception();

                            var result = dbContext.Vehicle.Where(vehicle => vehicle.UserId == (long)user_id).First();
                            Ride ride = new Ride
                            {
                                VehicleId = result.Id,
                                Seats = rideRequestDTO.seats,
                                Fee = rideRequestDTO.fee,
                                StartLoc = rideRequestDTO.startLocation,
                                EndLoc = rideRequestDTO.endLocation,
                                Time = rideRequestDTO.time,
                            };
                            dbContext.Ride.Add(ride);
                            dbContext.SaveChanges();
                            IList<Location> location = new List<Location>();
                            foreach (LocationObj _location in rideRequestDTO.locations)
                            {
                                location.Add(new Location() { Lat = _location.lat, Lon = _location.lon, RideId = ride.Id });
                            }
                            dbContext.Location.AddRange(location);
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
        [Route("driver/get")]
        public ActionResult<ResponseDTOGet<object>> getDriverRide([FromBody]GetRequestDTO rideRequestDTO)
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

                        command.Parameters.AddWithValue("@email", rideRequestDTO.email);
                        command.Parameters.AddWithValue("@session_id", rideRequestDTO.session_id);
                        command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                        user_id = command.ExecuteScalar();
                        if (user_id == null)
                            throw new Exception();

                        var results = (from vehicle in dbContext.Vehicle
                                       join ride in dbContext.Ride on vehicle.Id equals ride.VehicleId
                                       where vehicle.UserId == (long)user_id
                                       select new { ride.Id, ride.Time, ride.Seats, ride.StartLoc, ride.EndLoc, ride.Fee , vehicle.Model ,vehicle.Plate }).ToList();
                        connection.Close();
                        return new ResponseDTOGet<object>(200, "success", results);
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseDTOGet<object>(400, ex.ToString(), null);
                }
            }

        }
        [HttpPost]
        [Route("get")]
        public ActionResult<ResponseDTOGet<object>> getRide([FromBody]GetRideRequestDTO rideRequestDTO)
        {
            using (var dbContext = new carpoolingContext())
            {
                try
                {
                    var results = (from ride in dbContext.Ride
                                   join vehicle in dbContext.Vehicle on ride.VehicleId equals vehicle.Id
                                   join client in dbContext.Client on vehicle.UserId equals client.Id
                                   where ride.Time >= DateTime.Now
                                   select new { ride.Id , ride.Fee,  ride.EndLoc, ride.StartLoc,  ride.Time, client.Gender, client.Phone,
                                        ride.Seats,vehicle.Plate,client.Name,vehicle.Model, Location = dbContext.Location.Where(location=>location.RideId==ride.Id).ToArray()
                                   }
                                  ).ToArray();
                    List<object> rides = new List<object>();
                    foreach (var ride in results)
                    {
                        bool pickup = false;
                        bool dropoff = false;
                        foreach (var location in ride.Location)
                        {
                            double pickUp_d = HaversineMethod.getDistance(location.Lat, location.Lon, rideRequestDTO.pickUp.lat, rideRequestDTO.pickUp.lon);
                            double dropOff_d = HaversineMethod.getDistance(location.Lat, location.Lon, rideRequestDTO.dropOff.lat, rideRequestDTO.dropOff.lon);

                            if (pickUp_d < rideRequestDTO.radius)
                                pickup = true;
                            if (dropOff_d < rideRequestDTO.radius)
                                dropoff = true;

                            if (pickup == true && dropoff == true)
                            {
                                rides.Add(new {rideId=ride.Id , fee=ride.Fee, endLoc=ride.EndLoc, startLoc=ride.StartLoc, time=ride.Time, seats=ride.Seats , gender = ride.Gender , phone = ride.Phone  });
                                break;
                            }
                        }
                    }

                    return new ResponseDTOGet<object>(200, "success",rides);
                }
                catch (Exception ex)
                {
                    return new ResponseDTOGet<object>(400, ex.ToString(),null);
                }
            }
        }
    }
}
