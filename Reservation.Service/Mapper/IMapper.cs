using Reservation.Integration.Model;

namespace Reservation.Service.Mapper
{
    public interface IMapper
    {
        Reservation MapBookingToReservation(Booking booking);
        Guest MapPersonToGuest(Person person);
    }
}
