namespace SharedTrip.Controllers
{
    using SharedTrip.Services;
    using SharedTrip.ViewModels.Trips;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using System;
    using System.Globalization;

    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }
        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(inputModel.StartPoint) || string.IsNullOrWhiteSpace(inputModel.EndPoint) || string.IsNullOrWhiteSpace(inputModel.Description))
            {
                return this.View();
            }

            if (inputModel.Description.Length > 80)
            {
                return this.View();
            }

            if(inputModel.Seats < 2 || inputModel.Seats > 6)
            {
                return this.View();
            }

            DateTime dateTime;
            bool isDateTimeCorrect = DateTime.TryParseExact(inputModel.DepartureTime, "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

            if(!isDateTimeCorrect)
            {
                return this.View();
            }

            if(!string.IsNullOrWhiteSpace(inputModel.imagePath) && !inputModel.imagePath.StartsWith("https://"))
            { 
                return this.View();
            }

            this.tripsService.Add(inputModel.StartPoint, inputModel.EndPoint, inputModel.DepartureTime, inputModel.imagePath, inputModel.Seats, inputModel.Description);

            return Redirect("/Trips/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trips = new AllTripsViewModel { Trips = this.tripsService.GetAll() };

            return this.View(trips);
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trip = this.tripsService.GetTrip(tripId);
            Console.WriteLine(trip.DepartureTime);
            return this.View(trip);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (this.tripsService.JoinUser(tripId, this.User))
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            return this.Redirect("/Trips/All");
        }
    }
}
