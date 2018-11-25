using System;
using System.Collections.Generic;

namespace Reservation.Service
{
    public interface IReservationService
    {
        AddPersonResult AddPersonToReservation(Guid reservationId, Person person);
        Reservation GetReservation(Guid reservationId);
    }

    //This object will contain all the validation issues in case they exists so that the consumer has this information
    public class AddPersonResult
    {
        public AddPersonResult()
        {
            Errors = new List<Error>();
        }

        public bool IsSuccessful()
        {
            return (Errors.Count == 0);
        }

        public List<Error> Errors { get; set; }
    }

    //All the different types of errors 
    public enum Error
    {
        ReservationDoesNotExist = 1000,
        PersonIsNotValid = 2000,
        TitleNotCorrectForCountry = 3000,
        PersonAlreadyExists = 4000,
        NotEnoughNumberOfBeds = 5000,
        ErrorWhileUpdating = 6000

    }
}
