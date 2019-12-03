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

namespace CarPoolingApp.Controllers.user
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/user/trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        public TripController()
        {
        }

        [HttpPost]
        [Route("get")]
        public ActionResult<ResponseDTOGet<object>> getTrip([FromBody]GetRequestDTO tripRequestDTO)
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

                        command.Parameters.AddWithValue("@email", tripRequestDTO.email);
                        command.Parameters.AddWithValue("@session_id", tripRequestDTO.session_id);
                        command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                        user_id = command.ExecuteScalar();
                        if (user_id == null)
                            throw new Exception();

                        var results = (from client in dbContext.Client
                                       join trip in dbContext.Trip on client.Id equals trip.PassengerId
                                       join ride in dbContext.Ride on trip.RideId equals ride.Id
                                       join vehicle in dbContext.Vehicle on ride.VehicleId equals vehicle.Id
                                       where client.Id == (long)user_id
                                       select new { trip.Id , trip.Time , trip.Seats , ride.StartLoc , ride.EndLoc , ride.Fee ,vehicle.Plate }).ToList();
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
        [Route("book")]
        public ActionResult<ResponseDTOGet<object>> bookRide([FromBody]BookTripRequestDTO bookRideRequestDTO)
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

                            command.Parameters.AddWithValue("@email", bookRideRequestDTO.email);
                            command.Parameters.AddWithValue("@session_id", bookRideRequestDTO.session_id);
                            command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                            user_id = command.ExecuteScalar();
                            if (user_id == null)
                                throw new Exception();

                            Ride ride = dbContext.Ride.FirstOrDefault(ride => (bookRideRequestDTO.ride_id == ride.Id && ride.Seats >= bookRideRequestDTO.seats));
                            if (ride == null)
                                throw new Exception("Invalid input");
                            ride.Seats -= bookRideRequestDTO.seats;
                            dbContext.Ride.Update(ride);
                            dbContext.SaveChanges();

                            Trip trip = new Trip()
                            {
                                PassengerId = (long)user_id,
                                RideId = bookRideRequestDTO.ride_id,
                                Time = bookRideRequestDTO.time,
                                Seats = bookRideRequestDTO.seats

                            };
                            dbContext.Trip.Add(trip);
                            dbContext.SaveChanges();

                            transaction.Complete();
                            connection.Close();
                            return new ResponseDTOGet<object>(200, "success",new { trip_id = trip.Id });
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        return new ResponseDTOGet<object>(400, ex.ToString(),null);
                    }
                }
            }
        }

        [HttpPost]
        [Route("cancel")]
        public ActionResult<ResponseDTO> cancelRide([FromBody]CancelTripRequestDTO cancelTripRequestDTO)
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

                            command.Parameters.AddWithValue("@email", cancelTripRequestDTO.email);
                            command.Parameters.AddWithValue("@session_id", cancelTripRequestDTO.session_id);
                            command.Parameters.AddWithValue("@date_time", DateTime.Now.ToString());

                            user_id = command.ExecuteScalar();
                            if (user_id == null)
                                throw new Exception();

                            Trip oldTrip = dbContext.Trip.FirstOrDefault(trip => trip.Id == cancelTripRequestDTO.tripID);
                            if (oldTrip == null)
                                throw new Exception("Invalid input");

                            Ride ride = dbContext.Ride.FirstOrDefault(ride => oldTrip.RideId == ride.Id);
                            if (ride == null)
                                throw new Exception("Invalid input");

                            ride.Seats += oldTrip.Seats;
                            dbContext.Ride.Update(ride);
                            dbContext.SaveChanges();
                            dbContext.Trip.Remove(oldTrip);
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
