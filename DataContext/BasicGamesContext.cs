using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BasicGamesEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using NuGet.Protocol;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace BasicGames.Data
{
    public class BasicGamesContext : DbContext
    {
        public BasicGamesContext(DbContextOptions<BasicGamesContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the value converter for the Animal
            modelBuilder.Entity<Games>()
                .Property(x => x.Platforms)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<string>>(v))); // Convert to List<String> for use
        }

        public DbSet<Games> Games { get; set; }
    }
}
