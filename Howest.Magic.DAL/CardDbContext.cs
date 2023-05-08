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
        }
    }
}
