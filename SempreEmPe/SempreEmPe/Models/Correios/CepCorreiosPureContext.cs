using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace SempreEmPe.Models
{
    public partial class CepCorreiosPureContext : DbContext
    {
        public CepCorreiosPureContext(DbContextOptions<CepCorreiosPureContext> options)
            : base(options)
        {}

        public virtual DbSet<LogBairro> LogBairros { get; set; }
        public virtual DbSet<LogGrandeUsuario> LogGrandeUsuarios { get; set; }
        public virtual DbSet<LogLocalidade> LogLocalidades { get; set; }
        public virtual DbSet<LogLogradouro> LogLogradouros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_100_BIN");

            modelBuilder.Entity<LogBairro>(entity =>
            {
                entity.HasKey(e => new { e.BaiNu, e.UfeSg })
                    .HasName("PK_LOG_BAIRRO_1");

                entity.ToTable("LOG_BAIRRO");

                entity.Property(e => e.BaiNu).HasColumnName("BAI_NU");

                entity.Property(e => e.UfeSg)
                    .HasMaxLength(2)
                    .HasColumnName("UFE_SG")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.BaiNo)
                    .IsRequired()
                    .HasMaxLength(72)
                    .HasColumnName("BAI_NO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.BaiNoAbrev)
                    .HasMaxLength(36)
                    .HasColumnName("BAI_NO_ABREV")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNu).HasColumnName("LOC_NU");
            });

            modelBuilder.Entity<LogGrandeUsuario>(entity =>
            {
                entity.HasKey(e => e.GruNu);

                entity.ToTable("LOG_GRANDE_USUARIO");

                entity.HasIndex(e => new { e.GruNu, e.UfeSg }, "IX_LOG_GRANDE_USUARIO")
                    .IsUnique();

                entity.Property(e => e.GruNu)
                    .ValueGeneratedNever()
                    .HasColumnName("GRU_NU");

                entity.Property(e => e.BaiNu).HasColumnName("BAI_NU");

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("CEP")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.GruEndereco)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("GRU_ENDERECO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.GruNo)
                    .IsRequired()
                    .HasMaxLength(72)
                    .HasColumnName("GRU_NO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.GruNoAbrev)
                    .HasMaxLength(36)
                    .HasColumnName("GRU_NO_ABREV")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNu).HasColumnName("LOC_NU");

                entity.Property(e => e.LogNu).HasColumnName("LOG_NU");

                entity.Property(e => e.UfeSg)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("UFE_SG")
                    .UseCollation("Latin1_General_CI_AS");

                entity.HasOne(d => d.LogBairro)
                    .WithMany(p => p.LogGrandeUsuarios)
                    .HasForeignKey(d => new { d.BaiNu, d.UfeSg })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOG_GRANDE_USUARIO_LOG_BAIRRO");

                entity.HasOne(d => d.LogLocalidade)
                    .WithMany(p => p.LogGrandeUsuarios)
                    .HasForeignKey(d => new { d.LocNu, d.UfeSg })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOG_GRANDE_USUARIO_LOG_LOCALIDADE");

                entity.HasOne(d => d.LogLogradouro)
                    .WithMany(p => p.LogGrandeUsuarios)
                    .HasPrincipalKey(p => new { p.LogNu, p.UfeSg })
                    .HasForeignKey(d => new { d.LogNu, d.UfeSg })
                    .HasConstraintName("FK_LOG_GRANDE_USUARIO_LOG_LOGRADOURO");
            });

            modelBuilder.Entity<LogLocalidade>(entity =>
            {
                entity.HasKey(e => new { e.LocNu, e.UfeSg });

                entity.ToTable("LOG_LOCALIDADE");

                entity.Property(e => e.LocNu).HasColumnName("LOC_NU");

                entity.Property(e => e.UfeSg)
                    .HasMaxLength(2)
                    .HasColumnName("UFE_SG")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Cep)
                    .HasMaxLength(8)
                    .HasColumnName("CEP")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocInSit)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("LOC_IN_SIT")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocInTipoLoc)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("LOC_IN_TIPO_LOC")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNo)
                    .IsRequired()
                    .HasMaxLength(72)
                    .HasColumnName("LOC_NO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNoAbrev)
                    .HasMaxLength(36)
                    .HasColumnName("LOC_NO_ABREV")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNuSub).HasColumnName("LOC_NU_SUB");

                entity.Property(e => e.MunNu).HasColumnName("MUN_NU");
            });

            modelBuilder.Entity<LogLogradouro>(entity =>
            {
                entity.HasKey(e => e.LogNu);

                entity.ToTable("LOG_LOGRADOURO");

                entity.HasIndex(e => new { e.LogNu, e.UfeSg }, "IX_LOG_LOGRADOURO")
                    .IsUnique();

                entity.Property(e => e.LogNu)
                    .ValueGeneratedNever()
                    .HasColumnName("LOG_NU");

                entity.Property(e => e.BaiNuFim).HasColumnName("BAI_NU_FIM");

                entity.Property(e => e.BaiNuIni).HasColumnName("BAI_NU_INI");

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("CEP")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LocNu).HasColumnName("LOC_NU");

                entity.Property(e => e.LogComplemento)
                    .HasMaxLength(100)
                    .HasColumnName("LOG_COMPLEMENTO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LogNo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LOG_NO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LogNoAbrev)
                    .HasMaxLength(36)
                    .HasColumnName("LOG_NO_ABREV")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.LogStaTlo)
                    .HasMaxLength(1)
                    .HasColumnName("LOG_STA_TLO")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.TloTx)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("TLO_TX")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UfeSg)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("UFE_SG")
                    .UseCollation("Latin1_General_CI_AS");

                entity.HasOne(d => d.LogBairro)
                    .WithMany(p => p.LogLogradouros)
                    .HasForeignKey(d => new { d.BaiNuIni, d.UfeSg })
                    .HasConstraintName("FK_LOG_LOGRADOURO_LOG_BAIRRO");

                entity.HasOne(d => d.LogLocalidade)
                    .WithMany(p => p.LogLogradouros)
                    .HasForeignKey(d => new { d.LocNu, d.UfeSg })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOG_LOGRADOURO_LOG_LOCALIDADE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
