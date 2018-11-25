using System;
using System.Collections.Generic;
using System.Linq;
using Reservation.Integration.Enums;
using Reservation.Integration.Model;

namespace Reservation.Integration
{
    public class BookingService : IBookingService
    {
        private readonly List<Booking> _bookings;
        //Assumption made: We have one provider for bookings
        public BookingService()
        {
            //Mocking the response from the BookingService for testing the functionality
            _bookings = new List<Booking>()
            {
                new Booking()
                {
                    Guests = new List<Guest>()
                        {new Guest() {FirstName = "Dimitrios", LastName = "Varvaras", Title = TitleType.Herr}},
                    Hotel = new Hotel() {CountryCode = Country.DE, Name = "Scandic Germany"}, RoomType = RoomType.DOUBLE.ToString(),
                    BookingId = new Guid("71a07c7b-4c3c-41f9-b014-27e01c3e9451")
                },
                new Booking()
                {
                    Guests = new List<Guest>()
                        {new Guest() {FirstName = "John", LastName = "Doe", Title = null}},
                    Hotel = new Hotel() {CountryCode = Country.DK, Name = "Scandic Denmark"}, RoomType = RoomType.TRIPLE.ToString(),
                    BookingId = new Guid("3d72640d-76c7-4314-9cc7-437bc48975e6")
                },
                new Booking()
                {
                    Guests = new List<Guest>()
                    {
                        new Guest() {FirstName = "Michael", LastName = "Jackson", Title = null},
                        new Guest() {FirstName = "Bill", LastName = "Gates", Title = null}
                    },
                    Hotel = new Hotel() {CountryCode = Country.SE, Name = "Scandic Sweden"}, RoomType = RoomType.TWIN.ToString(),
                    BookingId = new Guid("55128ad9-1930-4445-b03d-b0aab7290680")
                }
            };
        }

        public Booking FetchBooking(Guid bookingId)
        {
            return _bookings.FirstOrDefault(t => t.BookingId.Equals(bookingId));
        }

        public void AddGuestToBooking(Guid bookingId, Guest guest)
        {
            var booking = _bookings.FirstOrDefault(t => t.BookingId.Equals(bookingId));
            List<Guest> listOfGuests = booking.Guests.ToList();
            listOfGuests.Add(guest);
            booking.Guests = listOfGuests;
        }
    }
}
