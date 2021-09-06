using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm
{
    public static class DatabaseSeeder
    {
        public static TEntity GetRandom<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            return query.GetRandom(1).FirstOrDefault();
        }

        public static IEnumerable<TEntity> GetRandom<TEntity>(this IQueryable<TEntity> dbSet, int take)
            where TEntity : class
        {
            return dbSet.AsNoTracking().OrderBy(x => Guid.NewGuid()).Take(take);
        }

        public static void SeedGames(ApplicationDbContext context)
        {
            var gameJson = GetResource("Games.json");
            var games = JsonSerializer.Deserialize<IEnumerable<Game>>(gameJson);

            context.Games.AddRange(games);
            context.SaveChanges();
        }

        public static void SeedTeams(ApplicationDbContext context)
        {
            var teamJson = GetResource("Teams.json");
            var teams = JsonSerializer.Deserialize<IEnumerable<Team>>(teamJson);

            context.Teams.AddRange(teams);
            context.SaveChanges();
        }

        private static string GetResource(string name)
        {
            var resourceName =
                $"{typeof(DatabaseSeeder).Namespace}.data.{name}"; //.Assembly.GetManifestResourceNames();
            using var stream = typeof(DatabaseSeeder).Assembly.GetManifestResourceStream(resourceName)
                               ?? throw new InvalidOperationException($"No Stream found with name {name}");
            using var reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            return result;
        }
    }
}