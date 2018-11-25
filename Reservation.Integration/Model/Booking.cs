using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Integration.Model
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public string RoomType { get; set; }
        public Hotel Hotel { get; set; }
    }
}
