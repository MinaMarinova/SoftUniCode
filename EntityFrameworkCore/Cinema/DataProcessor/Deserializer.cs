namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDtos = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);
            List<Movie> movies = new List<Movie>();

            StringBuilder sb = new StringBuilder();

            foreach (var movieDto in moviesDtos)
            {
                if (!IsValid(movieDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (movies.Any(m => m.Title == movieDto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = Mapper.Map<Movie>(movieDto);

                movies.Add(movie);

                sb.AppendLine(String.Format(SuccessfulImportMovie, movie.Title, movie.Genre.ToString(), movie.Rating.ToString("f2")));

                context.Movies.Add(movie);
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallDtos = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var hallDto in hallDtos)
            {
                if (!IsValid(hallDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (hallDto.Seats <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall
                {
                    Name = hallDto.Name,
                    Is3D = hallDto.Is3D,
                    Is4Dx = hallDto.Is4Dx
                };

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    var seat = new Seat
                    {
                        Hall = hall
                    };

                    hall.Seats.Add(seat);

                    context.Seats.Add(seat);
                }

                string projectionType = "";

                if (hall.Is3D == false && hall.Is4Dx == false)
                {
                    projectionType = "Normal";
                }

                else if (hall.Is3D == true && hall.Is4Dx == true)
                {
                    projectionType = "4Dx/3D";
                }

                else if (hall.Is3D == true)
                {
                    projectionType = "3D";
                }
                else if (hall.Is4Dx == true)
                {
                    projectionType = "4Dx";
                }

                sb.AppendLine(String.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));

                context.Halls.Add(hall);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            ImportProjectionDto[] projectionDtos;

            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectionDto[]),
                    new XmlRootAttribute("Projections"));

                projectionDtos = (ImportProjectionDto[])serializer.Deserialize(reader);
            }

            foreach (var projectionDto in projectionDtos)
            {
                if (!IsValid(projectionDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Movies.Any(m => m.Id == projectionDto.MovieId) ||
                    !context.Halls.Any(h => h.Id == projectionDto.HallId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = Mapper.Map<Projection>(projectionDto);

                context.Projections.Add(projection);
                context.SaveChanges();

                sb.AppendLine(String.Format(SuccessfulImportProjection, projection.Movie.Title, projection.DateTime.ToString("MM/dd/yyyy")));

            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            ImportCustomerDto[] customerDtos;
            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomerDto[]),
                    new XmlRootAttribute("Customers"));

                customerDtos = (ImportCustomerDto[])serializer.Deserialize(reader);
            }

            foreach (var customerDto in customerDtos)
            {
                var areAllTicketsValid = true;

                foreach (var ticketDto in customerDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        areAllTicketsValid = false;
                        break;
                    }

                    if (!context.Projections.Any(p => p.Id == ticketDto.ProjectionId))
                    {
                        sb.AppendLine(ErrorMessage);
                        areAllTicketsValid = false;
                        break;
                    }
                }

                if (areAllTicketsValid)
                {
                    var customer = Mapper.Map<Customer>(customerDto);

                    if (!IsValid(customerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    foreach (var ticketDto in customerDto.Tickets)
                    {
                        var ticket = Mapper.Map<Ticket>(ticketDto);
                    }

                    context.Tickets.AddRange(customer.Tickets);
                    context.Customers.Add(customer);

                    context.SaveChanges();
                    sb.AppendLine(String.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }

    }
}