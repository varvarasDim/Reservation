using System.Collections.Generic;

namespace Reservation.Service
{
    public class PersonComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            if (x == null || y == null)
                return false;

            return (x.FirstName == y.FirstName && x.LastName == y.LastName && x.Title == y.Title);
        }

        public int GetHashCode(Person obj)
        {
            return obj.GetHashCode();
        }
    }
}
