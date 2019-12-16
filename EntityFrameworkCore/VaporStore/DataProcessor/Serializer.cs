namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ExportDtos;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                     .Where(g => genreNames.Contains(g.Name))
                     .Select(genre => new GenreDto
                     {
                         Id = genre.Id,
                         Name = genre.Name,
                         Games = genre.Games.Where(g => g.Purchases.Count > 0)
                         .Select(g => new GameDto
                         {
                             Id = g.Id,
                             Name = g.Name,
                             DeveloperName = g.Developer.Name,
                             Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name).ToArray()),
                             Players = g.Purchases.Count
                         })
                         .OrderByDescending(g => g.Players)
                         .ThenBy(g => g.Id)
                         .ToList(),
                         TotalPlayers = genre.Games.Sum(g => g.Purchases.Count)
                     })
                     .OrderByDescending(g => g.TotalPlayers)
                     .ThenBy(g => g.Id)
                     .ToList();

            
            string jsonString = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return jsonString;
           
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {

            StringBuilder sb = new StringBuilder();

            var users = context.Users
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases
                                            .Where(p => p.Type.ToString() == storeType)).OrderBy(p => p.Date)
                        .Select(p => new ExportPurchaseDto
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto
                            {
                                Name = p.Game.Name,
                                Genre = p.Game.Genre.Name.ToString(),
                                Price = p.Game.Price,
                            },
                        }).ToArray(),
                    TotalSpent = u.Cards.SelectMany(c => c.Purchases.Where(p => p.Type.ToString() == storeType).Select(per => per.Game.Price)).Sum()
                }).ToArray()
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            using (var writer = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExportUserDto[]),
                    new XmlRootAttribute("Users"));

                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();

        }
    }
}