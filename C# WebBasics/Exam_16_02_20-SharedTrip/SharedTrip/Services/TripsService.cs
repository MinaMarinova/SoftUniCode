namespace SharedTrip.Services
{
    using SharedTrip.Models;
    using SharedTrip.ViewModels.Trips;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Add(string startPoint, string endPoint, string departureTime, string imagePath, int seats, string description)
        {
            var trip = new Trip
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                DepartureTime = DateTime.ParseExact(departureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                ImagePath = imagePath,
                Seats = seats,
                Description = description
            };

            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModelForAll> GetAll()
        {
            var trips = this.db.Trips.Select(t => new TripViewModelForAll
            {
                Id = t.Id,
                StartPoint = t.StartPoint,
                EndPoint = t.EndPoint,
                DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Seats = t.Seats
            });

            return trips;
        }

        public TripDetailsViewModel GetTrip(string tripId)
        {
            var trip = this.db.Trips
                .Where(t => t.Id == tripId)
                .Select(t => new TripDetailsViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    ImagePath = t.ImagePath,
                    Description = t.Description,
                    Seats = t.Seats
                }).FirstOrDefault();

            return trip;
        }
        
        public bool JoinUser(string tripId, string userId)
        {
            var trip = this.db.Trips.FirstOrDefault(t => t.Id == tripId);

            if(this.db.UserTrips.Any(ut => ut.UserId == userId && ut.TripId == tripId) || trip.Seats < 1)
            {
                return false;
            }

            else
            {
                var tripUser = new UserTrip
                {
                    UserId = userId,
                    TripId = tripId
                };

                trip.UserTrips.Add(tripUser);
                this.db.UserTrips.Add(tripUser);
                trip.Seats -= 1;
                this.db.SaveChanges();

                return true;
            }
        }
    }
}
