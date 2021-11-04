namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            //Console.WriteLine(ExportAlbumsInfo(context, 4));
            Console.WriteLine(ExportSongsAboveDuration(context, 480));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Albums
                .Where(x => x.ProducerId == producerId)
                .ToArray()
                .Select(x => new
                {
                    Name = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    AlbumSongs = x.Songs.Select(s => new 
                    {
                        SongName = s.Name,
                        Price = s.Price,
                        WriterName = s.Writer.Name
                    })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName),
                    TotalPrice = x.Price
                })
                .ToArray()
                .OrderByDescending(x => x.TotalPrice);
            StringBuilder text = new StringBuilder();
            foreach (var album in albums)
            {
                text.AppendLine($"-AlbumName: {album.Name}");
                text.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                text.AppendLine($"-ProducerName: {album.ProducerName}");
                text.AppendLine($"-Songs:");
                int count = 1;
                foreach (var song in album.AlbumSongs)
                {
                    text.AppendLine($"---#{count}");
                    text.AppendLine($"---SongName: {song.SongName}");
                    text.AppendLine($"---Price: {song.Price:f2}");
                    text.AppendLine($"---Writer: {song.WriterName}");

                    count++;
                }
                text.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
            }
            return text.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .ToArray()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new  
                {
                    Name = x.Name,
                    WriterName = x.Writer.Name,
                    PerformerFullName = x.SongPerformers
                        .ToArray()
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .FirstOrDefault(),
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c")
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerFullName)
                .ToArray();

            StringBuilder text = new StringBuilder();
            int count = 1;
            foreach (var song in songs)
            {
                text.AppendLine($"-Song #{count++}");
                text.AppendLine($"---SongName: {song.Name}");
                text.AppendLine($"---Writer: {song.WriterName}");
                text.AppendLine($"---Performer: {song.PerformerFullName}");
                text.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                text.AppendLine($"---Duration: {song.Duration}");
            }
            return text.ToString().TrimEnd();

        }
    }
}
