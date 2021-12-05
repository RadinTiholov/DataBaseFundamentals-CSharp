namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                .Theatres
                .ToArray()
                .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count >= 20)
                .Select(x => new
                {
                    Name = x.Name,
                    Halls = x.NumberOfHalls,
                    TotalIncome = x.Tickets.Where(x => x.RowNumber >= 1 && x.RowNumber <= 5).Sum(x => x.Price),
                    Tickets = x.Tickets
                    .Where(x => x.RowNumber >= 1 && x.RowNumber <= 5)
                    .OrderByDescending(x => x.Price)
                    .Select(t => new
                    {
                        Price = decimal.Parse(t.Price.ToString("F2")),
                        RowNumber = t.RowNumber
                    })
                })
                .OrderByDescending(x => x.Halls)
                .ThenBy(x => x.Name)
                .ToArray();

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            var theatresJson = JsonConvert.SerializeObject(theatres, settings);

            return theatresJson;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var plays = context
                .Plays
                .ToArray()
                .Where(x => x.Rating <= rating)
                .Select(x => new ExportPlayDTO
                {
                    Title = x.Title,
                    Duration = x.Duration.ToString("c"),
                    Rating = x.Rating == 0 ? "Premier" : x.Rating.ToString(),
                    Genre = x.Genre.ToString(),
                    Actors = x
                    .Casts
                    .Where(c => c.IsMainCharacter)
                    .Select(c => new ExportCastDTO
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{x.Title}'."
                    })
                    .OrderByDescending(c => c.FullName)
                    .ToArray()
                })
                .OrderBy(x => x.Title)
                .ThenByDescending(x => x.Genre)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPlayDTO[]), new XmlRootAttribute("Plays"));

            xmlSerializer.Serialize(new StringWriter(sb), plays, namespaces);

            return sb.ToString().Trim();
        }
    }
}
