using System;
using System.Collections.Generic;
using Reservation.Service;

namespace Reservation.Model
{
    //The Response to the consumer of the API
    public class ReservationResponse
    {
        public Guid ReservationId { get; set; }
        public List<Person> RegisteredPeople { get; set; }
        public string HotelName { get; set; }
    }
}
