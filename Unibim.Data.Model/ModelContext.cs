using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Unibim.Data.Model
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        {
        }

        public virtual DbSet<Altkategori> Altkategori { get; set; }
        public virtual DbSet<Bayi> Bayi { get; set; }
        public virtual DbSet<Grup> Grup { get; set; }
        public virtual DbSet<İlceler> İlceler { get; set; }
        public virtual DbSet<iller> iller { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<KullaniciYetki> KullaniciYetki { get; set; }
        public virtual DbSet<Musteri> Musteri { get; set; }
        public virtual DbSet<Ozellik> Ozellik { get; set; }
        public virtual DbSet<OzellikTur> OzellikTur { get; set; }
        public virtual DbSet<SatisSekil> SatisSekil { get; set; }
        public virtual DbSet<Sepet> Sepet { get; set; }
        public virtual DbSet<SepetUrun> SepetUrun { get; set; }
        public virtual DbSet<SepetUrunOzellik> SepetUrunOzellik { get; set; }
        public virtual DbSet<Siparis> Siparis { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Takim> Takim { get; set; }
        public virtual DbSet<TakimGorsel> TakimGorsel { get; set; }
        public virtual DbSet<TakimSatis> TakimSatis { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }
        public virtual DbSet<UrunGorsel> UrunGorsel { get; set; }
        public virtual DbSet<UrunOzellik> UrunOzellik { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bayi>()
                .Property(e => e.telefon1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Bayi>()
                .Property(e => e.telefon2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Bayi>()
                .Property(e => e.adres)
                .IsUnicode(false);

            modelBuilder.Entity<Grup>()
                .Property(e => e.kod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.tc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.cinsiyet)
                .IsFixedLength();

            modelBuilder.Entity<Musteri>()
                .Property(e => e.medeniHal)
                .IsFixedLength();

            modelBuilder.Entity<Musteri>()
                .Property(e => e.cep1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.cep2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.evtel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.istel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Takim>()
                .Property(e => e.aciklama)
                .IsUnicode(false);

            modelBuilder.Entity<Urun>()
                .Property(e => e.aciklama)
                .IsUnicode(false);
        }
    }
}
