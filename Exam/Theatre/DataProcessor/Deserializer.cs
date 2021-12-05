namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder text = new StringBuilder();
 
            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer xmlSerialzer = new XmlSerializer(typeof(ImportPlayDTO[]), root);

            StringReader reader = new StringReader(xmlString);
            HashSet<Play> plays = new HashSet<Play>();
            using (reader)
            {
                ImportPlayDTO[] playDTOs = (ImportPlayDTO[])xmlSerialzer.Deserialize(reader);
                foreach (var playDTO in playDTOs)
                {
                    if (!IsValid(playDTO))
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDurationValid = TimeSpan.TryParseExact(playDTO.Duration, "c", CultureInfo.InvariantCulture,TimeSpanStyles.None, out TimeSpan duration);
                    if (!isDurationValid || duration.TotalMinutes < 60)
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isGenreValid = Enum.TryParse<Genre>(playDTO.Genre, out Genre genre);
                    if (!isGenreValid)
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }
                    Play play = new Play 
                    {
                        Title = playDTO.Title,
                        Duration = duration,
                        Rating = playDTO.Rating,
                        Genre = genre,
                        Description = playDTO.Description,
                        Screenwriter = playDTO.Screenwriter
                        
                    };
                    plays.Add(play);
                    text.AppendLine(String.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
                }
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();

            return text.ToString().TrimEnd();

        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder text = new StringBuilder();
            XmlRootAttribute root = new XmlRootAttribute("Casts");
            XmlSerializer xmlSerialzer = new XmlSerializer(typeof(ImportCastDTO[]), root);

            StringReader reader = new StringReader(xmlString);
            HashSet<Cast> casts = new HashSet<Cast>();
            using (reader)
            {
                ImportCastDTO[] castDTOs = (ImportCastDTO[])xmlSerialzer.Deserialize(reader);
                foreach (var castDTO in castDTOs)
                {
                    if (!IsValid(castDTO))
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }
                    Cast cast = new Cast 
                    {
                        FullName = castDTO.FullName,
                        IsMainCharacter = castDTO.IsMainCharacter,
                        PhoneNumber = castDTO.PhoneNumber,
                        PlayId = castDTO.PlayId
                    };
                    casts.Add(cast);
                    text.AppendLine(String.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter? "main": "lesser"));
                }
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();
            return text.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder text = new StringBuilder();

            var theatersTicketsDTOs = JsonConvert.DeserializeObject<IEnumerable<ImportTheaterDTO>>(jsonString);
            HashSet<Theatre> theatres = new HashSet<Theatre>();
            foreach (var theaterDTO in theatersTicketsDTOs)
            {
                if (!IsValid(theaterDTO))
                {
                    text.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre
                {
                    Name = theaterDTO.Name,
                    Director = theaterDTO.Director,
                    NumberOfHalls = theaterDTO.NumberOfHalls,
                };
                HashSet<Ticket> tickets = new HashSet<Ticket>();
                foreach (var ticketDTO in theaterDTO.Tickets)
                {
                    if (!IsValid(ticketDTO))
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket 
                    {
                        Price = ticketDTO.Price,
                        RowNumber = ticketDTO.RowNumber,
                        PlayId = ticketDTO.PlayId,
                        TheatreId = theatre.Id
                    };
                    tickets.Add(ticket);
                }
                theatre.Tickets = tickets;
                theatres.Add(theatre);
                text.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }
            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
