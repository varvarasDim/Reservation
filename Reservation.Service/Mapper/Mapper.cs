using System.Collections.Generic;
using Reservation.Integration.Enums;
using Reservation.Integration.Model;

namespace Reservation.Service.Mapper
{
    public class Mapper : IMapper
    {
        //Mapper from Booking (Integration Layer) to Reservation (Service Layer) object
        public Reservation MapBookingToReservation(Booking booking)
        {
            var reservation = new Reservation();
            reservation.RegisteredPeople = new List<Person>();
            reservation.HotelName = booking.Hotel.Name;
            reservation.ReservationId = booking.BookingId;
            reservation.CountryCode = booking.Hotel.CountryCode;
            reservation.NumberOfBeds = NumberOfBeds(booking.RoomType);

            foreach (var guest in booking.Guests)
            {
                reservation.RegisteredPeople.Add(new Person() {FirstName = guest.FirstName, LastName = guest.LastName, Title = guest.Title});
            }

            return reservation;
        }

        //Mapper from Person (Service Layer) to Guest (Integration Layer) object
        public Guest MapPersonToGuest(Person person)
        {
            return new Guest() {FirstName = person.FirstName, LastName = person.LastName, Title = person.Title};
        }

        private int NumberOfBeds(string type)
        {

            if (type.Equals(RoomType.SINGLE.ToString()))
            {
                return 1;
            }
            if (type.Equals(RoomType.DOUBLE.ToString()) || type.Equals(RoomType.TWIN.ToString()))
            {
                return 2;
            }
            if (type.Equals(RoomType.TRIPLE.ToString()))
            {
                return 3;
            }

            return 0;

        }
    }
}
