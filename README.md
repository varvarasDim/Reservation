Information

The projects is named Reservation and consists of the following subprojects

- Reservation (This is the communcation layer, the API)
	Even though it was not part of the assignment it was implemeted for real testing of the Interface
- Reservation.Service
	This is the subproject that contains the business logic and exposes the interface to the communication layer
- Reservation.Integration
	This is the subproject that container the integration to the booking service
- Reservation.Service.UnitTest
	This subproject contains the unitests that challenge the Reservation.Service logic
	
Architecture Description
The service layer is aware of the integration layer (the booking service). The communication layer (API) is not aware of the Integration layer but is aware only of the Service layer. Mappers have been used to transform the integration objects to service objects (and vice versa) that will be sent to the communication layer.

This mapping is the following:
Layer 	 	Service			Integration
			Reservation		Bookings
			Person			Guest

The service layer informs the communication layer with the use of the AddPersonResult returned object if something went wrong with the addition of the guest to the booking.
The following errors are used:

        ReservationDoesNotExist,
        PersonIsNotValid,
        TitleNotCorrectForCountry,
        PersonAlreadyExists,
        NotEnoughNumberOfBeds,
        ErrorWhileUpdating
			
Tools and frameworks used

The project was implemented in VS2017 with use of Resharper and it was implemented in .netcore 2.
For the unit tests xunit and moq was used. 
The out-of-the-box dependency injection provider of .netcore was used.

Execution instructions

In order to execute the project you may just build it and hit the F5 key of Visual studio.
The launch url is set to "api/reservation/55128ad9-1930-4445-b03d-b0aab7290680" which is the booking of an existing mocked
booking in the bookingService.

Assumptions Made
- The first assumption is that there is only one provider of Bookings. In case there were more then the Service layer should be
implemented in a way that it picks the correct provider for the specific booking.
- Secondly only one guest can be added to the booking every time. In order to support multiple guests the Service layer should be 
designed so that it receives a list of Guests/People
- People do not have problem sleeping on the same double bed. If a room has twin beds or 1 double bed then it is the application in 
both of the cases considers that 2 beds exist.
- The validation that is added to the Guests allows only customer with alphabetical characters in their Lastname and Firstname. Also 
the title can be either empty, Herr, Frau or Dr.

