using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reservation.Integration.Enums;

namespace Reservation.Service.Validator
{
    public static class PersonValidator
    {
        public static bool Validate(Person person)
        { 
            //Assumption made: We allow only alphabetical characters for Firstname and Lastname
            return Regex.IsMatch(person.FirstName, @"^[a-zA-Z]+$") && Regex.IsMatch(person.LastName, @"^[a-zA-Z]+$") 
                   && (new List<TitleType?>() {TitleType.Herr, TitleType.Frau, TitleType.Dr, null}.Contains(person.Title));
        }
    }
}
