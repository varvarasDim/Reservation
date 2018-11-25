using Reservation.Integration.Enums;

namespace Reservation.Service
{
    //This is the object that is used in the service layer and exposed in the communication layer
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TitleType? Title { get; set; }
    }
}
