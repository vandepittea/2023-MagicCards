using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<CardColor> CardColors { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Models.Type> Types { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardColor>().HasKey(cc => new { cc.CardId, cc.ColorId });
            modelBuilder.Entity<CardType>().HasKey(ct => new { ct.CardId, ct.TypeId });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("cards");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.ManaCost).HasColumnName("mana_cost");
                entity.Property(e => e.ConvertedManaCost).HasColumnName("converted_mana_cost").IsRequired();
                entity.Property(e => e.Type).HasColumnName("type").IsRequired();
                entity.Property(e => e.RarityCode).HasColumnName("rarity_code");
                entity.Property(e => e.SetCode).HasColumnName("set_code").IsRequired();
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.Flavor).HasColumnName("flavor");
                entity.Property(e => e.ArtistId).HasColumnName("artist_id");
                entity.Property(e => e.Number).HasColumnName("number").IsRequired();
                entity.Property(e => e.Power).HasColumnName("power");
                entity.Property(e => e.Toughness).HasColumnName("toughness");
                entity.Property(e => e.Layout).HasColumnName("layout").IsRequired();
                entity.Property(e => e.MultiverseId).HasColumnName("multiverse_id");
                entity.Property(e => e.OriginalImageUrl).HasColumnName("original_image_url");
                entity.Property(e => e.Image).HasColumnName("image").IsRequired();
                entity.Property(e => e.OriginalText).HasColumnName("original_text");
                entity.Property(e => e.OriginalType).HasColumnName("original_type");
                entity.Property(e => e.MtgId).HasColumnName("mtg_id").IsRequired();
                entity.Property(e => e.Variations).HasColumnName("variations");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("cards_fk_artist_id");

                entity.HasOne(d => d.Rarity)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.RarityCode)
                    .HasConstraintName("cards_fk_rarity_code");

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.SetCode)
                    .HasConstraintName("cards_fk_set_code");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("artists");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired();
            });

            modelBuilder.Entity<CardColor>(entity =>
            {
                entity.ToTable("card_colors");

                entity.Property(e => e.CardId).HasColumnName("card_id");
                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.HasOne(cc => cc.Card)
                    .WithMany(c => c.CardColors)
                    .HasForeignKey(cc => cc.CardId)
                    .HasConstraintName("card_colors_fk_card_id");

                entity.HasOne(cc => cc.Color)
                    .WithMany(c => c.CardColors)
                    .HasForeignKey(cc => cc.ColorId)
                    .HasConstraintName("card_colors_fk_color_id");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.ToTable("card_types");

                entity.Property(ct => ct.CardId).HasColumnName("card_id");
                entity.Property(ct => ct.TypeId).HasColumnName("type_id");

                entity.HasOne(ct => ct.Card)
                    .WithMany(c => c.CardTypes)
                    .HasForeignKey(ct => ct.CardId)
                    .HasConstraintName("card_types_fk_card_id");

                entity.HasOne(ct => ct.Type)
                    .WithMany(t => t.CardTypes)
                    .HasForeignKey(ct => ct.TypeId)
                    .HasConstraintName("card_types_fk_type_id");
            });

            modelBuilder.Entity<Rarity>(entity =>
            {
                entity.ToTable("rarities");

                entity.Property(r => r.Code).HasColumnName("code").IsRequired();
                entity.Property(r => r.Name).HasColumnName("name").IsRequired();
            });
        }
    }
}
