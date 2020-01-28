using StartGrow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StartGrow.Data
{
    public static class SeedData
    {
        //public static void Initialize(UserManager<ApplicationUser> userManager,
        //            RoleManager<IdentityRole> roleManager)
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
 
            
            List<string> rolesNames = new List<string> { "Trabajador", "Inversor" };

            SeedRoles(roleManager, rolesNames);
            SeedUsers(userManager, rolesNames);
            //SeedMovies(dbContext);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {    

            foreach (string roleName in roles) { 
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB

            if (userManager.FindByNameAsync("trabajador@startgrow.com").Result == null)
            {
                ApplicationUser user = new Trabajador();
                user.UserName = "trabajador@startgrow.com";
                user.Email = "trabajador@startgrow.com";
                user.Nombre = "Sergio";
                user.Apellido1 = "Ruiz";
                user.Apellido2 = "Villafranca";
                user.CodPost = 2000;
                user.Domicilio = "Calle";
                user.Municipio = "Albacete";
                user.Nacionalidad = "Española";
                user.NIF = "123";
                user.PaisDeResidencia = "España";
                user.Provincia = "Albacete";
               

                IdentityResult result = userManager.CreateAsync(user, "TPassword1234%").Result;
 
                if (result.Succeeded)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user,roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("inversor@startgrow.com").Result == null)
            {
                ApplicationUser user = new Inversor();
                user.UserName = "inversor@startgrow.com";
                user.Email = "inversor@startgrow.com";
                user.Nombre = "Gregorio";
                 user.Apellido1= "Diaz";
                user.Apellido2 = "Descalzo";
                user.CodPost = 2000;
                user.Domicilio = "Calle";
                user.Municipio = "Albacete";
                user.Nacionalidad = "Española";
                user.NIF = "123";
                user.PaisDeResidencia = "España";
                user.Provincia = "Albacete";

                IdentityResult result = userManager.CreateAsync(user, "IPassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }
            
            if (userManager.FindByNameAsync("peter@uclm.com").Result == null)
            {
                ApplicationUser user = new Inversor();
                user.UserName = "peter@uclm.com";
                user.Email = "peter@uclm.com";
                user.Nombre = "Peter";
                user.Apellido1 = "Diaz";
                user.Apellido2 = "Descalzo";
                user.CodPost = 2000;
                user.Domicilio = "Calle";
                user.Municipio = "Albacete";
                user.Nacionalidad = "Española";
                user.NIF = "123";
                user.PaisDeResidencia = "España";
                user.Provincia = "Albacete";

                IdentityResult result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }
            
        }
/*
        public static void SeedMovies(ApplicationDbContext dbContext)
        {
            //Genres and movies are created so that they are available whenever the system is run
            Movie movie;
            Genre genre = dbContext.Genre.FirstOrDefault(m => m.Name.Contains("The Lord of the Rings"));
            if (genre == null) { 
            genre = new Genre()
            {
                Name = "Drama"
            };
            dbContext.Genre.Add(genre);
        }
          
            if (!dbContext.Movie.Any(m => m.Title.Contains("The Lord of the Rings"))) {
                movie = new Movie()
                {
                    Title = "The Lord of the Rings",
                    QuantityForRenting = 10,
                    PriceForRenting = 1,
                    QuantityForPurchase = 12,
                    PriceForPurchase = 15,
                    Genre = genre
                };
                dbContext.Movie.Add(movie);
            }
            
            genre = dbContext.Genre.FirstOrDefault(m => m.Name.Contains("The Lord of the Rings"));
            if (genre == null) { 
                genre = new Genre()
                {
                    Name = "Action"
                };
                dbContext.Genre.Add(genre);
            }
            if (!dbContext.Movie.Any(m => m.Title.Contains("Star Wars"))) {
                movie = new Movie()
                {
                    Title = "Star Wars",
                    QuantityForRenting = 10,
                    PriceForRenting = 1,
                    QuantityForPurchase = 12,
                    PriceForPurchase = 10,
                    Genre = genre
                };
                dbContext.Movie.Add(movie);
            }
            genre = dbContext.Genre.FirstOrDefault(m => m.Name.Contains("The Lord of the Rings"));
            if (genre == null) { 
                genre = new Genre()
                {
                    Name = "Commedy"
                };
                dbContext.Genre.Add(genre);
            }
            if (!dbContext.Movie.Any(m => m.Title.Contains("Campeones"))) {
                movie = new Movie()
                {
                    Title = "Campeones",
                    QuantityForRenting = 10,
                    PriceForRenting = 2,
                    QuantityForPurchase = 12,
                    PriceForPurchase = 20,
                    Genre = genre
                };
                dbContext.Movie.Add(movie);
            }
            dbContext.SaveChanges();
        }
        */
    }
   


}


