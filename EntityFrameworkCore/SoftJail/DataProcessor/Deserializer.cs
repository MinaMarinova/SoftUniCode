namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool areAllCellsValid = true;

                foreach (var cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        sb.AppendLine("Invalid Data");
                        areAllCellsValid = false;
                        break;
                    }
                }

                if (areAllCellsValid)
                {
                    var department = new Department
                    {
                        Name = departmentDto.Name,
                    };

                    foreach (var cellDto in departmentDto.Cells)
                    {
                        var cell = new Cell
                        {
                            CellNumber = cellDto.CellNumber,
                            HasWindow = cellDto.HasWindow,
                        };

                        department.Cells.Add(cell);
                        context.Cells.Add(cell);
                    }

                    context.Departments.Add(department);
                    context.SaveChanges();

                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                
                
                bool areAllMailsValid = true;

                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        sb.AppendLine("Invalid Data");
                        areAllMailsValid = false;
                        break;
                    }
                }

                if (areAllMailsValid)
                {
                    var prisoner = new Prisoner
                    {
                        FullName = prisonerDto.FullName,
                        Nickname = prisonerDto.Nickname,
                        Age = prisonerDto.Age,
                        IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Bail = prisonerDto.Bail,
                        CellId = prisonerDto.CellId
                    };
                    
                    if (String.IsNullOrWhiteSpace(prisonerDto.ReleaseDate))
                    {
                        prisoner.ReleaseDate = null;
                    }

                    else
                    {
                        prisoner.ReleaseDate = DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    foreach (var mailDto in prisonerDto.Mails)
                    {
                        var mail = new Mail
                        {
                            Description = mailDto.Description,
                            Sender = mailDto.Description,
                            Address = mailDto.Address
                        };

                        prisoner.Mails.Add(mail);
                        context.Mails.Add(mail);
                    }

                    

                    context.Prisoners.Add(prisoner);
                    context.SaveChanges();

                    sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            ImportOfficerDto[] officerDtos;

            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImportOfficerDto[]),
                    new XmlRootAttribute("Officers"));

                officerDtos = (ImportOfficerDto[])serializer.Deserialize(reader);
            }

            foreach (var officerDto in officerDtos)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isPositionValid = Enum.TryParse<Position>(officerDto.Position, out Position position);
                bool isWeaponValid = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon weapon);

                bool areAllPrisonersValid = true;

                //foreach (var prisonerDto in officerDto.Prisoners)
                //{
                //    if (!context.Prisoners.Any(p => p.Id == prisonerDto.Id))
                //    {
                //        areAllPrisonersValid = false;
                //        break;
                //    }
                //}

                if (isPositionValid == false || isWeaponValid == false || areAllPrisonersValid == false)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = Mapper.Map<Officer>(officerDto);

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    var officerPrisoner = new OfficerPrisoner
                    {
                        Officer = officer,
                        PrisonerId = prisonerDto.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                context.Officers.Add(officer);
                context.OfficersPrisoners.AddRange(officer.OfficerPrisoners);

                context.SaveChanges();

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
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