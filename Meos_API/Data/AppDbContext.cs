using System;
using Meos_Shared;
using Microsoft.EntityFrameworkCore;

namespace Meos_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PersonClass> Persons { get; set; }
        public DbSet<LicenseClass> Licenses { get; set; }
        public DbSet<FinesClass> Fines { get; set; }
        public DbSet<VehicleClass> Vehicles { get; set; }
        public DbSet<PersonNoteClass> PersonNotes { get; set; }
        public DbSet<VehicleNoteClass> VehicleNotes { get; set; }
        public DbSet<ArrestWarrantClass> ArrestWarrants { get; set; }
        public DbSet<IncidentClass> Incidents { get; set; }
        public DbSet<IncidentNoteClass> IncidentNotes { get; set; }
        public DbSet<UsersClass> Users { get; set; }
        public DbSet<SentenceClass> Sentence { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabellen
            modelBuilder.Entity<PersonClass>().ToTable("users");
            modelBuilder.Entity<LicenseClass>().ToTable("user_licenses");
            modelBuilder.Entity<FinesClass>().ToTable("billing");
            modelBuilder.Entity<VehicleClass>().ToTable("owned_vehicles");
            modelBuilder.Entity<PersonNoteClass>().ToTable("meos_person_notes");
            modelBuilder.Entity<VehicleNoteClass>().ToTable("meos_vehicle_notes");
            modelBuilder.Entity<ArrestWarrantClass>().ToTable("meos_arrestwarrants");
            modelBuilder.Entity<IncidentClass>().ToTable("meos_incidents");
            modelBuilder.Entity<IncidentNoteClass>().ToTable("meos_incident_notes");
            modelBuilder.Entity<UsersClass>().ToTable("meos_users");
            modelBuilder.Entity<SentenceClass>().ToTable("meos_incident_sentence");

            // Persoon entity
            modelBuilder.Entity<PersonClass>()
                .HasKey(p => p.Identifier);

            modelBuilder.Entity<PersonClass>()
                .HasIndex(p => p.Identifier)
                .IsUnique();
            modelBuilder.Entity<PersonClass>()
                .Property(p => p.Identifier)
                .HasColumnName("identifier");

            // Licentie entity
            modelBuilder.Entity<LicenseClass>()
                .HasKey(l => l.Owner);

            modelBuilder.Entity<LicenseClass>()
                .Property(l => l.Owner)
                .HasColumnName("owner");

            // Relatie Persoon -> Licenties
            modelBuilder.Entity<PersonClass>()
                .HasMany(p => p.Licenses)
                .WithOne(l => l.Person)
                .HasForeignKey(l => l.Owner)
                .HasPrincipalKey(p => p.Identifier);

            // Boete entity
            modelBuilder.Entity<FinesClass>()
                .HasKey(f => f.Identifier);

            modelBuilder.Entity<FinesClass>()
                .Property(f => f.Identifier)
                .HasColumnName("identifier");

            // Relatie Persoon -> Boetes
            modelBuilder.Entity<FinesClass>()
                .HasOne(f => f.Person)
                .WithMany(p => p.Fines)
                .HasForeignKey(f => f.Identifier)
                .HasPrincipalKey(p => p.Identifier);

            modelBuilder.Entity<VehicleClass>()
                .HasKey(v => new { v.Owner, v.Plate });

            modelBuilder.Entity<VehicleClass>()
                .Property(v => v.Owner)
                .HasColumnName("owner");

            modelBuilder.Entity<VehicleClass>()
               .HasMany(v => v.Notes)
               .WithOne(n => n.Vehicle)
               .HasForeignKey(n => n.Plate)
               .HasPrincipalKey(v => v.Plate);
            
            modelBuilder.Entity<VehicleClass>()
                .HasOne<PersonClass>()                  // of jouw echte User-modelnaam
                .WithMany(p => p.Vehicles)                          // als User een collectie van voertuigen heeft, vervang dit door .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.Owner)          // de kolom in VehicleClass
                .HasPrincipalKey(p => p.Identifier); 

            modelBuilder.Entity<PersonClass>()
                .HasMany(p => p.Vehicles)
                .WithOne(v => v.Person)
                .HasForeignKey(v => v.Owner)
                .HasPrincipalKey(p => p.Identifier);

            modelBuilder.Entity<PersonClass>()
               .HasMany(p => p.PersonNotes)
               .WithOne(n => n.Person)
               .HasForeignKey(n => n.Identifier)
               .HasPrincipalKey(p => p.Identifier);

            modelBuilder.Entity<PersonNoteClass>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<VehicleNoteClass>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<PersonNoteClass>()
               .Property(n => n.Identifier)
               .HasColumnName("identifier");

            modelBuilder.Entity<VehicleNoteClass>()
               .Property(n => n.Plate)
               .HasColumnName("plate");

            modelBuilder.Entity<ArrestWarrantClass>()
                .Property(a => a.Identifier)
                .HasColumnName("identifier");

            modelBuilder.Entity<ArrestWarrantClass>()
                .HasOne(a => a.Person)
                .WithMany(p => p.ArrestWarrant)
                .HasForeignKey(a => a.Identifier)
                .HasPrincipalKey(p => p.Identifier);

            modelBuilder.Entity<IncidentClass>()
                .HasKey(n => n.IncidentId);

            modelBuilder.Entity<IncidentClass>()
                .Property(i => i.Identifier)
                .HasColumnName("identifier");

            modelBuilder.Entity<IncidentClass>()
                .HasOne(i => i.Person)
                .WithMany(p => p.Incident)
                .HasForeignKey(i => i.Identifier)
                .HasPrincipalKey(p => p.Identifier);
            
            modelBuilder.Entity<IncidentNoteClass>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<IncidentNoteClass>()
               .Property(n => n.IncidentId)
               .HasColumnName("incidentid");

            modelBuilder.Entity<IncidentClass>()
                 .HasMany(i => i.Users)
                 .WithMany(u => u.Incidents)
                 .UsingEntity(j => j.ToTable("meos_incident_users")); // koppel-tabel

            modelBuilder.Entity<SentenceClass>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SentenceClass>()
               .Property(s => s.IncidentId)
               .HasColumnName("incidentid");
        }
    }
}
