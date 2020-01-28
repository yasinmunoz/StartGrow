using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StartGrow.Models;

namespace StartGrow.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Areas> Areas { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Inversion> Inversion { get; set; }
        public DbSet<InversionRecuperada> InversionRecuperada { get; set; }
        public DbSet<Inversor> Inversor { get; set; }
        public DbSet<Monedero> Monedero { get; set; }
        public DbSet<Particular> Particular { get; set; }
        public DbSet<Preferencias> Preferencias { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<ProyectoAreas> ProyectoAreas { get; set; }
        public DbSet<ProyectoTiposInversiones> ProyectoTiposInversiones { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Solicitud> Solicitud { get; set; }
        public DbSet<TiposInversiones> TiposInversiones { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
        }
    }
}