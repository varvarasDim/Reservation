using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reservation.Model;
using Reservation.Service;

namespace Reservation.Controllers
{
    //The API was not part of the assignment but it was implemented for transparency and testing the service Interface and how it behaves 
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //Get, REST GET the Reservation by Id
        [HttpGet("{reservationId}")]
        public ActionResult<ReservationResponse> GetReservation(Guid reservationId)
        {
            var reservation = _reservationService.GetReservation(reservationId);

            if (reservation == null)
                return NoContent();

            return Ok(new ReservationResponse()
            {
                HotelName = reservation.HotelName,
                RegisteredPeople = reservation.RegisteredPeople,
                ReservationId = reservation.ReservationId
            });
        }

        //Update, REST PUT the Reservation with another person
        [HttpPut("{reservationId}")]
        public ActionResult UpdateReservation(Guid reservationId, [FromBody] PostedPerson postedPerson)
        {
            var result = _reservationService.AddPersonToReservation(reservationId,
                new Person() { FirstName = postedPerson.FirstName, LastName = postedPerson.LastName, Title = postedPerson.Title });

            //The communication layer has the information of what went wrong
            if (!result.IsSuccessful())
                return BadRequest(result.Errors.Select(t => t.ToString()));

            return Ok();
        }


    }
}
