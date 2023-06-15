using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dievas.Models;
using Dievas.Models.Auth;
using Dievas.Models.Messages;
using Dievas.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Dievas.Data {

    /// <summary>
    ///     Application Database <c>ApDBContext</c> Provides Access to 
    ///     persistant datastores
    ///
    /// </summary>
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }
        public DbSet<UserModel> Users { get; set; }

        // Messageing Tables
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        // System Settings Tables
        public DbSet<SettingType> SettingTypes { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Map Roles from list<string> to and from Json
            modelBuilder.Entity<UserModel>()
                .Property(x => x.Roles)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<string>>(v)), // Convert to List<String> for use
                        new ValueComparer<List<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()));

            // Seed MEssage Types
            modelBuilder.Entity<MessageType>().HasData(new MessageType(1,"full-page"));
            modelBuilder.Entity<MessageType>().HasData(new MessageType(2,"marquee"));

            // Seed SettingType
            modelBuilder.Entity<SettingType>().HasData(new SettingType(1,"text","text"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(2,"button","button"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(3,"checkbox","checkbox"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(4,"color","color"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(5,"date","date"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(6,"datetime-local","datetime-local"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(7,"email","email"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(8,"file","file"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(9,"hidden","hidden"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(10,"image","image"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(11,"month","month"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(12,"number","number"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(13,"password","password"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(14,"radio","radio"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(15,"range","range"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(16,"reset","reset"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(17,"search","search"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(18,"submit","submit"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(19,"tel","tel"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(20,"time","time"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(21,"url","url"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(22,"week","week"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(23,"int","int"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(24,"float","float"));
            modelBuilder.Entity<SettingType>().HasData(new SettingType(25,"double","double"));
        }
    }
}