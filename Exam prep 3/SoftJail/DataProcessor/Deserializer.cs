namespace SoftJail.DataProcessor
{

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
    using System.Text;
    using System.Linq;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string InvalidData = "Invalid Data";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder text = new StringBuilder();

            var departmentDTOS = JsonConvert.DeserializeObject<IEnumerable<ImportDepartmentsCellsDTO>>(jsonString);
            HashSet<Department> departments = new HashSet<Department>();
            foreach (var departmentDTO in departmentDTOS)
            {
                if (!IsValid(departmentDTO))
                {
                    text.AppendLine(InvalidData);
                    continue;
                }
                HashSet<Cell> cells = new HashSet<Cell>();
                bool isCellValid = true;
                foreach (var cellDTO in departmentDTO.Cells)
                {
                    if (!IsValid(cellDTO))
                    {
                        text.AppendLine(InvalidData);
                        isCellValid = false;
                        break;
                    }
                    Cell cell = new Cell 
                    {
                        CellNumber = cellDTO.CellNumber,
                        HasWindow = cellDTO.HasWindow
                    };

                    cells.Add(cell);
                }
                if (!isCellValid)
                {
                    continue;
                }

                Department department = new Department 
                {
                    Name = departmentDTO.Name,
                    Cells = cells
                };
                departments.Add(department);
                text.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder text = new StringBuilder();

            var prisonerDTOs = JsonConvert.DeserializeObject<IEnumerable<ImportPrisonerDTO>>(jsonString);
            HashSet<Prisoner> prisoners = new HashSet<Prisoner>();
            foreach (var prisonerDTO in prisonerDTOs)
            {
                if (!IsValid(prisonerDTO))
                {
                    text.AppendLine(InvalidData);
                    continue;
                }

                bool isIncarcerationDateRight = DateTime.TryParseExact(prisonerDTO.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime incarcerationDate);
                if (!isIncarcerationDateRight)
                {
                    text.AppendLine(InvalidData);
                    continue;
                }

                bool isReleaseDateRight = DateTime.TryParseExact(prisonerDTO.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
                if (!isReleaseDateRight && !string.IsNullOrWhiteSpace(prisonerDTO.ReleaseDate))
                {
                    text.AppendLine(InvalidData);
                    continue;
                }
                HashSet<Mail> mails = new HashSet<Mail>();
                bool isMailValid = true;
                foreach (var mailDTO in prisonerDTO.Mails)
                {
                    if (!IsValid(mailDTO))
                    {
                        isMailValid = false;
                        text.AppendLine(InvalidData);
                        break;
                    }
                    Mail mail = new Mail 
                    {
                        Address = mailDTO.Address,
                        Sender = mailDTO.Sender,
                        Description = mailDTO.Description
                    };
                    mails.Add(mail);
                }
                if (!isMailValid)
                {
                    continue;
                }
                Prisoner prisoner = new Prisoner 
                {
                    FullName = prisonerDTO.FullName,
                    Nickname = prisonerDTO.Nickname,
                    Age = prisonerDTO.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDTO.Bail,
                    CellId = prisonerDTO.CellId,
                    Mails = mails
                };

                prisoners.Add(prisoner);
                text.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder text = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Officers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerDTO[]), root);

            StringReader reader = new StringReader(xmlString);

            HashSet<Officer> officers = new HashSet<Officer>();
            using (reader)
            {
                ImportOfficerDTO[] officerDTOs = (ImportOfficerDTO[])xmlSerializer.Deserialize(reader);

                foreach (var officerDTO in officerDTOs)
                {
                    if (!IsValid(officerDTO))
                    {
                        text.AppendLine(InvalidData);
                        continue;
                    }

                    bool isPositionValid = Enum.TryParse<Position>(officerDTO.Position , out Position position);
                    if (!isPositionValid)
                    {
                        text.AppendLine(InvalidData);
                        continue;
                    }

                    bool isWeaponValid = Enum.TryParse<Weapon>(officerDTO.Weapon, out Weapon weapon);
                    if (!isWeaponValid)
                    {
                        text.AppendLine(InvalidData);
                        continue;
                    }
                    Officer officer = new Officer
                    {
                        FullName = officerDTO.Name,
                        Salary = decimal.Parse(officerDTO.Money),
                        Position = position,
                        Weapon = weapon,
                        DepartmentId = officerDTO.DepartmentId
                    };
                    HashSet<OfficerPrisoner> officerPrisoners = new HashSet<OfficerPrisoner>();
                    foreach (var prisonerDTO in officerDTO.Prisoners)
                    {
                        if (!IsValid(prisonerDTO))
                        {
                            text.AppendLine(InvalidData);
                            continue;
                        }
                        OfficerPrisoner officerPrisoner = context.OfficersPrisoners.FirstOrDefault(x => x.PrisonerId == prisonerDTO.Id);
                        if (officerPrisoner == null)
                        {
                            officerPrisoner = new OfficerPrisoner
                            {
                                OfficerId = officer.Id,
                                PrisonerId = prisonerDTO.Id
                            };
                        }
                        officerPrisoners.Add(officerPrisoner);
                    }
                    officer.OfficerPrisoners = officerPrisoners;
                    officers.Add(officer);
                    text.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                }
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}