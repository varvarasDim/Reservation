using Reservation.Integration.Enums;

namespace Reservation.Model
{
    //PostedPerson from the consumer of the API
    public class PostedPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TitleType? Title { get; set; }
    }
}
