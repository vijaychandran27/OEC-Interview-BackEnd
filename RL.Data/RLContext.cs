﻿using Microsoft.EntityFrameworkCore;
using RL.Data.DataModels;
using RL.Data.DataModels.Common;

namespace RL.Data;
public class RLContext : DbContext
{
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanProcedure> PlanProcedures { get; set; }
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPlanProcedure> UserPlanProcedure { get; set; }

    public RLContext() { }
    public RLContext(DbContextOptions<RLContext> options) : base(options) { }

    protected override async void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PlanProcedure>(typeBuilder =>
        {
            typeBuilder.HasKey(pp => new { pp.PlanId, pp.ProcedureId });
            typeBuilder.HasOne(pp => pp.Plan).WithMany(p => p.PlanProcedures);
            typeBuilder.HasOne(pp => pp.Procedure).WithMany();
        });

        builder.Entity<UserPlanProcedure>(typeBuilder =>
        {
            typeBuilder.HasKey(p => p.UserPlanProcedureId);

            // Relationships
            typeBuilder.HasOne(p => p.User)
                    .WithMany(u => u.UserPlanProcedures)
                    .HasForeignKey(p => p.UserId);

            typeBuilder.HasOne(p => p.Plan)
                    .WithMany(plan => plan.UserPlanProcedures)
                    .HasForeignKey(p => p.PlanId);

            typeBuilder.HasOne(p => p.Procedure)
                    .WithMany(proc => proc.UserPlanProcedures)
                    .HasForeignKey(p => p.ProcedureId);

            // Index
            typeBuilder.HasIndex(p => new { p.UserId, p.ProcedureId, p.PlanId, p.CreateDate, p.UpdateDate }).IsUnique();
        });


        //Add procedure Seed Data
        var seedData = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "ProcedureSeedData.csv"));
        builder.Entity<Procedure>(p =>
        {
            var seedProcedures = seedData.Select((sd, i) => new Procedure
            {
                ProcedureId = i + 1,
                ProcedureTitle = sd
            });
            p.HasData(seedProcedures);
        });

        //Add User Seed Data
        builder.Entity<User>(u =>
        {
            u.HasData(new List<User> {
                new User {
                    UserId = 1,
                    Name = "Nick Morrison",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
                new User {
                    UserId = 2,
                    Name = "Scott Cassidy",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
                new User {
                    UserId = 3,
                    Name = "Tony Bidner",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
                new User {
                    UserId = 4,
                    Name = "Patryk Skwarko",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                }
            });
        });
    }


    #region TimeStamps
    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries().Where(x => x.Entity is IChangeTrackable && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((IChangeTrackable)entity.Entity).CreateDate = DateTime.UtcNow;
            }

            ((IChangeTrackable)entity.Entity).UpdateDate = DateTime.UtcNow;
        }
    }
    #endregion
}
