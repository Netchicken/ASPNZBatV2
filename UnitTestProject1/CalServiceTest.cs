namespace MSTest
{
    using System.Collections.Generic;
    using System.Linq;
    using ASPNZBat.Business.ICal;
    using Ical.Net.CalendarComponents;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tests;

    [TestClass]
    public class CalServiceTest
    {
        //https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
        //Arrange – this is where you would typically prepare everything for the test, in other words, prepare the scene for testing(creating the objects and setting them up as necessary)

        //Act – this is where the method we are testing is executed

        //Assert – this is the final part of the test where we compare what we expect to happen with the actual result of the test method execution
        private ICalService _calService;
        private SeatBookingFake _seatBooking;
        public CalServiceTest()
        {
            _seatBooking = new SeatBookingFake();
            _calService = new CalService();
        }


        //[ClassInitialize]
        //public void SetUp()
        //{//https://stackoverflow.com/questions/44323498/unable-to-get-default-constructor-for-class-in-unit-test-project
        //    _calService = new CalService();
        //    //this created an empty ctor
        //}

        /// <summary>
        /// Testing the fake data to see its the correct format
        /// </summary>
        [TestMethod]
        public void SeatbookingFakeTest()
        {
            var output = _seatBooking.GetAllSeatBookings();
            Assert.AreEqual(4, output.Count());
        }



        [TestMethod]
        public void EventsControllerBoolTest()
        {
            //Arrange
            string result = _calService.GetBookedSeats(_seatBooking.GetAllSeatBookings(), true);

            Assert.IsNull(result);

        }

        [TestMethod]
        public void GetBookedSeatsTest()
        {

            //Arrange
            string result = _calService.GetBookedSeats(_seatBooking.GetAllSeatBookings(), true);

            //Act//Assert
            List<CalendarEvent> output = _calService.OutputEventsToIndex(_calService.CalOutput());

            Assert.AreEqual(60, output.Count);

        }

    }
}
