namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enum;
    using VaporStore.DataProcessor.ImportDtos;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<string> developersNames = new List<string>();
            List<string> genresNames = new List<string>();
            List<string> tagsNames = new List<string>();

            foreach (var gameDto in gamesDtos)
            {

                if (!IsValid(gameDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (gameDto.Tags.Count < 1)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                if (!developersNames.Contains(gameDto.Developer))
                {
                    var developer = new Developer
                    {
                        Name = gameDto.Developer,
                    };

                    developersNames.Add(gameDto.Developer);
                    context.Developers.Add(developer);
                    context.SaveChanges();
                }

                if (!genresNames.Contains(gameDto.Genre))
                {
                    var genre = new Genre
                    {
                        Name = gameDto.Genre
                    };

                    genresNames.Add(gameDto.Genre);
                    context.Genres.Add(genre);
                    context.SaveChanges();
                }

                foreach (var tagFromDto in gameDto.Tags)
                {

                    if (!tagsNames.Contains(tagFromDto))
                    {
                        var tag = new Tag
                        {
                            Name = tagFromDto,
                        };

                        tagsNames.Add(tagFromDto);
                        context.Tags.Add(tag);

                        context.SaveChanges();
                    }

                    var gameTag = new GameTag
                    {
                        Game = game,
                        TagId = context
                            .Tags
                            .Where(t => t.Name == tagFromDto)
                            .Select(t => t.Id)
                            .FirstOrDefault()
                    };

                    game.GameTags.Add(gameTag);
                }
                game.Developer = context.Developers.Where(d => d.Name == gameDto.Developer).FirstOrDefault();
                game.Genre = context.Genres.Where(g => g.Name == gameDto.Genre).FirstOrDefault();

                context.Games.Add(game);
                context.GameTags.AddRange(game.GameTags);

                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (userDto.Cards.Count < 1)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool areAllCardsValid = true;

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine("Invalid Data");
                        areAllCardsValid = false;
                        break;
                    }
                }

                if (areAllCardsValid)
                {
                    var user = new User
                    {
                        Username = userDto.Username,
                        FullName = userDto.FullName,
                        Email = userDto.Email,
                        Age = userDto.Age
                    };

                    foreach (var cardDto in userDto.Cards)
                    {
                        var card = new Card
                        {
                            Number = cardDto.Number,
                            Cvc = cardDto.CVC,
                            Type = (CardType)Enum.Parse(typeof(CardType), cardDto.Type)
                        };

                        user.Cards.Add(card);
                        context.Cards.Add(card);
                    }

                    context.Users.Add(user);
                    context.SaveChanges();

                    sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            ImportPurchaseDto[] purchaseDtos;

            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImportPurchaseDto[]),
                    new XmlRootAttribute("Purchases"));

                purchaseDtos = (ImportPurchaseDto[])serializer.Deserialize(reader);
            }

            foreach (var purchaseDto in purchaseDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!context.Games.Any(g => g.Name == purchaseDto.GameName))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!context.Cards.Any(c => c.Number == purchaseDto.CardNumber))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isTypeValid = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType genre);

                if (!isTypeValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = Mapper.Map<Purchase>(purchaseDto);
                purchase.Card = context.Cards.Where(c => c.Number == purchaseDto.CardNumber).FirstOrDefault();
                purchase.Game = context.Games.Where(g => g.Name == purchaseDto.GameName).FirstOrDefault();
                context.Purchases.Add(purchase);
                context.SaveChanges();

                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
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