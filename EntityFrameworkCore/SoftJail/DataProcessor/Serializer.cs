namespace SoftJail.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .Where(pr => ids.Contains(pr.Id))
                .Select(p => new ExportPrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                          .Select(op => op.Officer)
                          .Select(o => new ExportOfficerDto
                          {
                              OfficerName = o.FullName,
                              Department = o.Department.Name
                          })
                          .OrderBy(o => o.OfficerName)
                          .ToList(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                }).ToArray();

            string jsonString = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonString;

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersNameArr = prisonersNames.Split(",");


            var prisonersDtos = context.Prisoners
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .Where(p => prisonersNameArr.Contains(p.FullName))
                .Select(p => new ExportPrisonerWithMailsDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = p.Mails.Select(m => new ExportMailDto
                    {
                        Description = Reverse(m.Description)
                    }).ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExportPrisonerWithMailsDto[]),
                    new XmlRootAttribute("Prisoners"));

                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(writer, prisonersDtos, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}