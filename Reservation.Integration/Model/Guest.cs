using Reservation.Integration.Enums;

namespace Reservation.Integration.Model
{
    public class Guest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TitleType? Title { get; set; }
    }

}
