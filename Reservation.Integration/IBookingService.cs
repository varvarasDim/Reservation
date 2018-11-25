using System;
using Reservation.Integration.Model;

namespace Reservation.Integration
{
    public interface IBookingService
    {
        Booking FetchBooking(Guid bookingId);
        void AddGuestToBooking(Guid bookingId, Guest guest);
    }
}