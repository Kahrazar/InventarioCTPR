using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Inventario.Models
{
    public class ApplicationDbContext : DbContext
    {
        //Modelos que se van a tomar para hacer tablas por entity frameWork   
        public DbSet<Especialidad> Especialidad { get; set; }
        public DbSet<Bienes> Bienes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        //Configuraciones de la base de datos
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Properties<string>().Where(p => p.Name.StartsWith("numeroDePatrimonio")).Configure(p => p.IsKey());  //Configura cual atributo sera llave primaria
            modelBuilder.Entity<Bienes>().HasRequired(x => x.Especialidad);//Configura la llave foranea
           base.OnModelCreating(modelBuilder);     
        }

        //Este metodo le dice a EntityFramework cual Connection String usar
        public ApplicationDbContext() : base("con")
        {
        }


    }
}