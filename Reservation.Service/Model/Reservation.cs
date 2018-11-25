using System;
using System.Collections.Generic;
using Reservation.Integration.Model;

namespace Reservation.Service
{
    //This is the object that is used in the service layer and exposed in the communication layer
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public List<Person> RegisteredPeople { get; set; }
        public string HotelName { get; set; }
        public int NumberOfBeds { get; set; }
        public Country CountryCode { get; set; }
    }
}
