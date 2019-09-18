using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace iWasHere.Domain.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<DictionaryConstructionType> DictionaryConstructionType { get; set; }
        public virtual DbSet<DictionaryCurrencyType> DictionaryCurrencyType { get; set; }
        public virtual DbSet<DictionaryGuideType> DictionaryGuideType { get; set; }
        public virtual DbSet<DictionaryHistoricalPeriodType> DictionaryHistoricalPeriodType { get; set; }
        public virtual DbSet<DictionaryLandmarkType> DictionaryLandmarkType { get; set; }
        public virtual DbSet<DictionarySeasonType> DictionarySeasonType { get; set; }
        public virtual DbSet<DictionaryTicketType> DictionaryTicketType { get; set; }
        public virtual DbSet<ExchangeRates> ExchangeRates { get; set; }
        public virtual DbSet<Landmark> Landmark { get; set; }
        public virtual DbSet<LandmarkXticket> LandmarkXticket { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ts-internship-2019.database.windows.net;Initial Catalog=WonderWoman;Persist Security Info=False;User ID=sa_admin;Password=A123456a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__City__CountyId__55BFB948");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.OwnerName).HasMaxLength(100);

                entity.Property(e => e.SubmitedDate).HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Landmark)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.LandmarkId)
                    .HasConstraintName("FK__Comment__Landmar__39237A9A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserId");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.County)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__County__CountryI__54CB950F");
            });

            modelBuilder.Entity<DictionaryConstructionType>(entity =>
            {
                entity.HasKey(e => e.ConstructionTypeId)
                    .HasName("PK__Dictiona__5793AC771DF98B58");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DictionaryCurrencyType>(entity =>
            {
                entity.HasKey(e => e.CurrencyTypeId)
                    .HasName("PK__Dictiona__1DD4BB3EFCB2FDF4");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.CurrencyCountry)
                    .WithMany(p => p.DictionaryCurrencyType)
                    .HasForeignKey(d => d.CurrencyCountryId)
                    .HasConstraintName("FK__Dictionar__Curre__7E02B4CC");
            });

            modelBuilder.Entity<DictionaryGuideType>(entity =>
            {
                entity.HasKey(e => e.GuideTypeId)
                    .HasName("PK__Dictiona__75125080979405F2");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<DictionaryHistoricalPeriodType>(entity =>
            {
                entity.HasKey(e => e.HistoricalPeriodTypeId)
                    .HasName("PK__Dictiona__178B18729848D2F8");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<DictionaryLandmarkType>(entity =>
            {
                entity.HasKey(e => e.LandmarkTypeId)
                    .HasName("PK__Dictiona__A5900F207A51FD1C");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<DictionarySeasonType>(entity =>
            {
                entity.HasKey(e => e.SeasonTypeId)
                    .HasName("PK__Dictiona__F409B70EA0FDC008");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<DictionaryTicketType>(entity =>
            {
                entity.HasKey(e => e.TicketTypeId)
                    .HasName("PK__Dictiona__6CD68431F6C733A0");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ExchangeRates>(entity =>
            {
                entity.HasKey(e => e.ExchangeRateId)
                    .HasName("PK__Exchange__B0560449E26CDB79");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 4)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.ExchangeRates)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .HasConstraintName("FK__ExchangeR__Curre__5B78929E");
            });

            modelBuilder.Entity<Landmark>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 8)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 8)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CityId");

                entity.HasOne(d => d.ConstructionType)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.ConstructionTypeId)
                    .HasConstraintName("FK__Landmark__Constr__0880433F");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_CountryId");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("FK_CountyId");

                entity.HasOne(d => d.HistoricalPeriodType)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.HistoricalPeriodTypeId)
                    .HasConstraintName("FK__Landmark__Histor__0A688BB1");

                entity.HasOne(d => d.LandmarkType)
                    .WithMany(p => p.Landmark)
                    .HasForeignKey(d => d.LandmarkTypeId)
                    .HasConstraintName("FK__Landmark__Landma__0B5CAFEA");
            });

            modelBuilder.Entity<LandmarkXticket>(entity =>
            {
                entity.HasKey(e => e.LandmarkDetailId)
                    .HasName("PK__Landmark__CC94AF9CAEC35CA8");

                entity.ToTable("LandmarkXTicket");

                entity.HasOne(d => d.Landmark)
                    .WithMany(p => p.LandmarkXticket)
                    .HasForeignKey(d => d.LandmarkId)
                    .HasConstraintName("FK__LandmarkD__Landm__32767D0B");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.LandmarkXticket)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK__LandmarkD__Ticke__336AA144");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.ImagePath).HasMaxLength(1024);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Landmark)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.LandmarkId)
                    .HasConstraintName("FK__Photo__LandmarkI__36470DEF");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(14, 4)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .HasConstraintName("FK__Ticket__Currency__2704CA5F");

                entity.HasOne(d => d.GuideType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.GuideTypeId)
                    .HasConstraintName("FK__Ticket__GuideTyp__28ED12D1");

                entity.HasOne(d => d.SeasonType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.SeasonTypeId)
                    .HasConstraintName("FK__Ticket__SeasonTy__27F8EE98");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.TicketTypeId)
                    .HasConstraintName("FK__Ticket__TicketTy__2610A626");
            });
        }
    }
}
