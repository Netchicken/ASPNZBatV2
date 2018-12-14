using System;
using Xunit;
using ASPNZBat;

namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using ASPNZBat.Business.ICal;
    using Ical.Net.CalendarComponents;

    public class CalServiceTest
    {
        //https://code-maze.com/unit-testing-aspnetcore-web-api/
        //Arrange – this is where you would typically prepare everything for the test, in other words, prepare the scene for testing(creating the objects and setting them up as necessary)

        //Act – this is where the method we are testing is executed

        //Assert – this is the final part of the test where we compare what we expect to happen with the actual result of the test method execution
        private ICalService _calService;
        private SeatBookingFake _seatBooking;
        public CalServiceTest(ICalService calService)
        {
            _seatBooking = new SeatBookingFake();
            _calService = calService;
        }

        [Fact]
        public void seatbookingFakeTest()
        {
            var output = _seatBooking.GetAllSeatBookings();
            Assert.Equal(3, output.Count());
        }



        [Fact]
        public void outputToIndexTest()
        {
            //Arrange
            _calService.CalendarBooking(_seatBooking.GetAllSeatBookings(), true);

            //Act//Assert
            List<CalendarEvent> output = _calService.OutputEventsToIndex(_calService.CalOutput());
            Assert.Equal(3, output.Count());

        }
    }
}
