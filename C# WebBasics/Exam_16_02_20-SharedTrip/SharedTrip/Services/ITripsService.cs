namespace SharedTrip.Services
{
    using SharedTrip.ViewModels.Trips;
    using System.Collections.Generic;
    
    public interface ITripsService
    {
        void Add(string startPoint, string endPoint, string departureTime, string imagePath, int seats, string description);

        IEnumerable<TripViewModelForAll> GetAll();

        TripDetailsViewModel GetTrip(string tripId);

        public bool JoinUser(string tripId, string userId);
    }
}
