namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumInfo = context.Producers
                .Include(p => p.Albums).ThenInclude(a => a.Songs).ThenInclude(s => s.Writer)
                .First(p => p.Id == producerId)
                .Albums.Select(a => new
                {
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        SongWriterName = s.Writer.Name,
                    }).OrderByDescending(als => als.SongName).ThenBy(als => als.SongWriterName),
                    TotalAlbumPrice = a.Price
                }).OrderByDescending(a => a.TotalAlbumPrice).AsEnumerable();
            StringBuilder sb = new();
            foreach (var album in albumInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");

                int indexer = 1;
                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{indexer++}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:F2}");

            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songInfo = context.Songs
                .Include(s => s.SongPerformers)
                    .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                    .ThenInclude(a => a.Producer)                
                .Where(s => s.Duration > new TimeSpan(0, 0, duration))
                .Select(s => new
                {
                    SongName = s.Name,
                    SongWriter = s.Writer.Name,
                    Performers = s.SongPerformers
                    .Select(s => s.Performer.FirstName + " " + s.Performer.LastName).ToList(),
                     AlbumProducer = s.Album.Producer.Name,
                    SongDuration = s.Duration.ToString("c")
                }).OrderBy(s => s.SongName)
                .ThenBy(s => s.SongWriter)
                .ToList();
            StringBuilder sb = new();
            int index = 1;
            foreach (var song in songInfo)
            {                
                sb.AppendLine($"-Song #{index++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.SongWriter}");               
                if (song.Performers.Any())
                {
                    foreach ( var performer in song.Performers.OrderBy(x=>x))
                    {
                        sb.AppendLine($"---Performer: {performer}");
                    }                   
                }
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.SongDuration}");

            }

            return sb.ToString().TrimEnd();

        }
    }
}
