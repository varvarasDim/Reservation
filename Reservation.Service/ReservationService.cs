using System;
using System.Linq;
using Reservation.Integration;
using Reservation.Integration.Model;
using Reservation.Service.Mapper;
using Reservation.Service.Validator;

namespace Reservation.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public ReservationService(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        public AddPersonResult AddPersonToReservation(Guid reservationId, Person person)
        {
            //In case of validation errors the errors are send to the consumer so that it knows what is the problem
            var returnResult = new AddPersonResult();

            var reservation = GetReservation(reservationId);

            //Reservation does not exist
            if (reservation == null)
            {
                returnResult.Errors.Add(Error.ReservationDoesNotExist);
                return returnResult;
            }

            //Validate person for correct input in FirstName and LastName
            if (!PersonValidator.Validate(person))
                returnResult.Errors.Add(Error.PersonIsNotValid);

            //XOR reverse binary function because we only want to have title when and only when the country is DE
            if ((reservation.CountryCode.Equals(Country.DE) ^ person.Title != null))
                returnResult.Errors.Add(Error.TitleNotCorrectForCountry);

            //Check that the person is not registered again in the same reservation
            if (reservation.RegisteredPeople.Any(t => new PersonComparer().Equals(t, person)))
                returnResult.Errors.Add(Error.PersonAlreadyExists);


            //Check if booking has space for more by the number of guests and room type
            //Assumption made: People do not have problem sleeping on the same double bed
            //Assumption made: Adding one person per time
            var numberOfExistingPeople = reservation.RegisteredPeople.Count();

            if (numberOfExistingPeople >= reservation.NumberOfBeds)
                returnResult.Errors.Add(Error.NotEnoughNumberOfBeds);

            if (returnResult.IsSuccessful())
            {
                try
                {
                    var newGuest = _mapper.MapPersonToGuest(person);
                    _bookingService.AddGuestToBooking(reservation.ReservationId, newGuest);
                }
                catch (Exception e)
                {
                    //In a real life scenario a Logger should be used that will log the exceptions for eg elastic search to use for monitoring/tracking
                    Console.WriteLine($"Error occured while updating booking. Exception: {e.Message}");
                    returnResult.Errors.Add(Error.ErrorWhileUpdating);
                }
            }

            return returnResult;
        }

        public Reservation GetReservation(Guid reservationId)
        {
            try
            {
                var booking = _bookingService.FetchBooking(reservationId);

                if (booking == null)
                    return null;

                return _mapper.MapBookingToReservation(booking);
            }
            catch (Exception e)
            {
                //In a real life scenario a Logger should be used that will log the exceptions for eg elastic search to use for monitoring/tracking
                Console.WriteLine($"Error occured while fetching booking. Exception: {e.Message}");
            }

            return null;
        }





    }
}
