namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var moviesDtos = context.Movies
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(m => m.Projections.Select(p => p.Tickets.Sum(t => t.Price)).Sum())
                .Where(m => m.Rating >= rating)
                .Where(m => m.Projections.Any(p => p.Tickets.Count > 0))
                .Take(10)
                .Select(m => new ExportMovieDto
                {
                    Name = m.Title,
                    Rating = m.Rating.ToString("f2"),
                    TotalIncomes = m.Projections.Select(p => p.Tickets.Sum(t => t.Price)).Sum().ToString("f2"),

                    Customers = m.Projections.SelectMany(p => p.Tickets).
                    Select(pr => new ExportCustomerDto
                    {
                        FirstName = pr.Customer.FirstName,
                        LastName = pr.Customer.LastName,
                        Balance = pr.Customer.Balance.ToString("f2")
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToArray()
                })
                .Take(10)
                .ToArray();


            string jsonString = JsonConvert.SerializeObject(moviesDtos, Formatting.Indented);

            return jsonString;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context
                .Customers
                .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
                .Where(c => c.Age >= age)
                .ProjectTo<ExportCustomerDurationDto>()
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExportCustomerDurationDto[])
                    , new XmlRootAttribute("Customers"));

                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}