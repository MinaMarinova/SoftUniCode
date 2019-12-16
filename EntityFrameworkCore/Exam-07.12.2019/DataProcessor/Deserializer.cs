namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.DataProcessor.ImportDto;
    using System.IO;
    using System.Xml.Serialization;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            ImportProjectDto[] projectDtos;

            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectDto[]),
                    new XmlRootAttribute("Projects"));

                projectDtos = (ImportProjectDto[])serializer.Deserialize(reader);
            }

            foreach (var projectDto in projectDtos)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                if (String.IsNullOrWhiteSpace(projectDto.DueDate))
                {
                    project.DueDate = null;
                }
                else
                {
                    project.DueDate = DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                foreach (var taskDto in projectDto.Tasks)
                {
                    if(!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };

                    if (task.OpenDate < project.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (task.DueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    project.Tasks.Add(task);
                    context.Tasks.Add(task);
                }

                context.Projects.Add(project);

                context.SaveChanges();

                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

            foreach (var employeeDto in employeeDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                Console.WriteLine(String.Join(", ", employeeDto.Tasks.Distinct().ToArray()));

                foreach (var taskId in employeeDto.Tasks.Distinct().ToArray())
                {
                    if (!context.Tasks.Any(t => t.Id == taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskId
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                    context.EmployeesTasks.Add(employeeTask);
                }

                context.Employees.Add(employee);
                context.SaveChanges();

                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
        
    }
}