namespace SharedTrip.ViewModels.Trips
{
    using System.Collections.Generic;
    
    public class AllTripsViewModel
    {
        public AllTripsViewModel()
        {
            this.Trips = new HashSet<TripViewModelForAll>();
        }
        public IEnumerable<TripViewModelForAll> Trips { get; set; }
    }
}
