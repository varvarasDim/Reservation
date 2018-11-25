using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Reservation.Integration;
using Reservation.Integration.Enums;
using Reservation.Integration.Model;
using Reservation.Service.Mapper;

namespace Reservation.Service.UnitTest
{
    //The names of the tests are self explanatory
    //Using xUnit and Moq to create the stub objects
    
    public class UnitTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly IMapper _mapper;

        
        public UnitTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _mapper = new Mapper.Mapper();
        }

        [Fact]
        public void GetReservation_Succeeds_NormalResult()
        {
            var sut = GetSut();
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.GetReservation(testReservationId);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetReservation_Succeeds_NoReservation()
        {
            var sut = GetSut();
            _bookingServiceMock.Setup(t => t.FetchBooking(It.IsAny<Guid>())).Returns((Booking)null);
            var result = sut.GetReservation(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public void GetReservation_Fails_ExceptionThrown()
        {
            var sut = GetSut();
            _bookingServiceMock.Setup(t => t.FetchBooking(It.IsAny<Guid>())).Throws<Exception>();
            var result = sut.GetReservation(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public void AddPersonToReservation_Fails_ExceptionThrown()
        {
            var sut = GetSut();
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //testReservationId is not important here
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            _bookingServiceMock.Setup(t => t.AddGuestToBooking(It.IsAny<Guid>(), It.IsAny<Guest>()))
                .Throws<Exception>();
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.ErrorWhileUpdating, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Succeeds()
        {
            var sut = GetSut();

            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            _bookingServiceMock.Setup(t => t.AddGuestToBooking(testReservationId, It.IsAny<Guest>()));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.True(result.IsSuccessful());

        }

        [Fact]
        public void AddPersonToReservation_Fails_NoReservation()
        {
            var sut = GetSut();
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            _bookingServiceMock.Setup(t => t.FetchBooking(It.IsAny<Guid>())).Returns((Booking)null);

            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.ReservationDoesNotExist, result.Errors);
           
        }

        [Fact]
        public void AddPersonToReservation_Fails_PersonAlreadyExists()
        {
            var sut = GetSut();
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            var booking = StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId));
            //personToBeAdded is setup as to be the same as the existing one in the booking
            var personToBeAdded = new Person() { FirstName = booking.Guests.ToList()[0].FirstName, LastName = booking.Guests.ToList()[0].LastName, Title = null };
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(booking);
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.PersonAlreadyExists, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Fails_NamingValidation()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos1", LastName = "Papadopoulos 2", Title = null };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.PersonIsNotValid, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Fails_NoTitleForGermany()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000001"); //Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.TitleNotCorrectForCountry, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Succeeds_TitleExistsForGermany()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = TitleType.Herr };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000001"); //Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.True(result.IsSuccessful());
        }

        [Fact]
        public void AddPersonToReservation_Fails_TitleForNonGermany()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = TitleType.Frau };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.TitleNotCorrectForCountry, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Succeeds_NoTitleForNonGermany()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.True(result.IsSuccessful());
        }

        [Fact]
        public void AddPersonToReservation_Fails_NoEmptyBeds()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000003"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.False(result.IsSuccessful());
            Assert.True(result.Errors.Count == 1);
            Assert.Contains(Error.NotEnoughNumberOfBeds, result.Errors);
        }

        [Fact]
        public void AddPersonToReservation_Succeeds_EmptyBedsExist()
        {
            var sut = GetSut();
            var personToBeAdded = new Person() { FirstName = "Mitsos", LastName = "Papadopoulos", Title = null };
            Guid testReservationId = new Guid("00000000-0000-0000-0000-000000000002"); //Non Germany Hotel
            _bookingServiceMock.Setup(t => t.FetchBooking(testReservationId)).Returns(StaticData.GetMockedBookings().FirstOrDefault(t => t.BookingId.Equals(testReservationId)));
            var result = sut.AddPersonToReservation(testReservationId, personToBeAdded);
            Assert.True(result.IsSuccessful());
        }

        private ReservationService GetSut()
        {
            return new ReservationService(_bookingServiceMock.Object, _mapper);
        }
    }

    public static class StaticData
    {

        public static List<Booking> GetMockedBookings()
        {
            var bookings = new List<Booking>()
            {
                new Booking()
                {
                    Guests = new List<Guest>()
                        {new Guest() {FirstName = "FirstNameA", LastName = "LastNameA", Title = TitleType.Herr}},
                    Hotel = new Hotel() {CountryCode = Country.DE, Name = "Hotel1"}, RoomType = RoomType.TWIN.ToString(),
                    BookingId = new Guid("00000000-0000-0000-0000-000000000001")
                },
                new Booking()
                {
                    Guests = new List<Guest>()
                        {new Guest() {FirstName = "FirstNameB", LastName = "LastNameB", Title = null}},
                    Hotel = new Hotel() {CountryCode = Country.DK, Name = "Hotel2"}, RoomType = RoomType.TRIPLE.ToString(),
                    BookingId = new Guid("00000000-0000-0000-0000-000000000002")
                },
                new Booking()
                {
                    Guests = new List<Guest>()
                    {
                        new Guest() {FirstName = "FirstNameC", LastName = "LastNameC", Title = null},
                        new Guest() {FirstName = "FirstNameD", LastName = "LastNameD", Title = null}
                    },
                    Hotel = new Hotel() {CountryCode = Country.SE, Name = "Hotel3"}, RoomType = RoomType.TWIN.ToString(),
                    BookingId = new Guid("00000000-0000-0000-0000-000000000003")
                }
            };

            return bookings;



        }
    }
}

