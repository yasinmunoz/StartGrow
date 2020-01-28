using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StartGrow.Models;
using StartGrow.Data;
using StartGrow.Models.PreferenciasViewModel;
using StartGrow.Controllers;

namespace StartGrow.UT.Controllers.AccountController_test
{
    public class Account_SelectPreferenciasForInversor_test
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("StartGrow").UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext accountContext;

        public Account_SelectPreferenciasForInversor_test()
        {
            _contextOptions = CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            
            // Insert seed data into the database using one instance of the context

            context.Areas.Add(new Areas { AreasId = 1, Nombre = "Sanidad",  });
            context.Areas.Add(new Areas { AreasId = 2, Nombre = "Consultoria" });
            context.Areas.Add(new Areas { AreasId = 3, Nombre = "Educación" });
            context.Areas.Add(new Areas { AreasId = 4, Nombre = "Seguridad" });
            context.Areas.Add(new Areas { AreasId = 5, Nombre = "Construcción" });
            context.Areas.Add(new Areas { AreasId = 6, Nombre = "Transporte" });
            context.Areas.Add(new Areas { AreasId = 7, Nombre = "TIC" });
            context.Areas.Add(new Areas { AreasId = 8, Nombre = "Ingeniería" });
            context.Areas.Add(new Areas { AreasId = 9, Nombre = "Hogar" });
            context.Areas.Add(new Areas { AreasId = 10, Nombre = "Alimentación" });
            context.Areas.Add(new Areas { AreasId = 11, Nombre = "Textil" });
            context.Areas.Add(new Areas { AreasId = 12, Nombre = "Comercio" });
            context.Areas.Add(new Areas { AreasId = 13, Nombre = "Hosteleria" });
            context.Areas.Add(new Areas { AreasId = 14, Nombre = "Administración" });
            context.Areas.Add(new Areas { AreasId = 15, Nombre = "Automóviles" });
            context.Areas.Add(new Areas { AreasId = 16, Nombre = "Reparaciones" });
            context.Areas.Add(new Areas { AreasId = 17, Nombre = "Banca" });
            context.Areas.Add(new Areas { AreasId = 18, Nombre = "Maquinaría" });

            context.TiposInversiones.Add(new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" });
            context.TiposInversiones.Add(new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" });
            context.TiposInversiones.Add(new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" });

            context.Rating.Add(new Rating { RatingId = 1, Nombre = "A" });
            context.Rating.Add(new Rating { RatingId = 2, Nombre = "B" });
            context.Rating.Add(new Rating { RatingId = 3, Nombre = "C" });
            context.Rating.Add(new Rating { RatingId = 4, Nombre = "D" });

            context.SaveChanges();

            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("David@startgrow.inversor.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            accountContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            accountContext.User = identity;
        }


        [Fact]
        public async Task Select_SinParametro()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null,null,null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = null,
                    IdsToAddRating = null,
                    IdsToAddTiposInversion = null
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }


        [Fact]
        public async Task Select_ConParametros()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = idAreas,
                    IdsToAddRating = idRating,
                    IdsToAddTiposInversion = idTiposInversion
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentAreas = viewResult.RouteValues.Values.First();
                var currentRating = viewResult.RouteValues.Values.First();
                var currentTiposInversiones = viewResult.RouteValues.Values.First();

                Assert.Equal(preferencias.IdsToAddAreas, currentAreas);
                Assert.Equal(preferencias.IdsToAddRating, currentRating);
                Assert.Equal(preferencias.IdsToAddTiposInversion, currentTiposInversiones);
            }
        }


        [Fact]
        public async Task Select_FallandoAreas()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = null,
                    IdsToAddRating = idRating,
                    IdsToAddTiposInversion = idTiposInversion
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }


        [Fact]
        public async Task Select_FallandoRating()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = idAreas,
                    IdsToAddRating = null,
                    IdsToAddTiposInversion = idTiposInversion
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }

        [Fact]
        public async Task Select_FallandoTiposInversiones()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = idAreas,
                    IdsToAddRating = idRating,
                    IdsToAddTiposInversion = null
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }


        [Fact]
        public async Task Select_FallandoAreasyRating()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = null,
                    IdsToAddRating = null,
                    IdsToAddTiposInversion = idTiposInversion
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }


        [Fact]
        public async Task Select_FallandoRatingyTiposInversiones()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = idAreas,
                    IdsToAddRating = null,
                    IdsToAddTiposInversion = null
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }

        [Fact]
        public async Task Select_FallandoAreasyTiposInversiones()
        {
            // Arrange
            using (context) //Set the test case will use the inMemory database created in the constructor
            {

                var controller = new AccountController(null, null, null, null, context);

                //Areas
                var areaEsperada = new Areas[18] { new Areas { AreasId = 1, Nombre = "Sanidad" }, new Areas { AreasId = 2, Nombre = "Consultoria" }, new Areas { AreasId = 3, Nombre = "Educación" }, new Areas { AreasId = 4, Nombre = "Seguridad" }, new Areas { AreasId = 5, Nombre = "Construcción" }, new Areas { AreasId = 6, Nombre = "Transporte" }, new Areas { AreasId = 7, Nombre = "TIC" }, new Areas { AreasId = 8, Nombre = "Ingeniería" }, new Areas { AreasId = 9, Nombre = "Hogar" }, new Areas { AreasId = 10, Nombre = "Alimentación" }, new Areas { AreasId = 11, Nombre = "Textil" }, new Areas { AreasId = 12, Nombre = "Comercio" }, new Areas { AreasId = 13, Nombre = "Hosteleria" }, new Areas { AreasId = 14, Nombre = "Administración" }, new Areas { AreasId = 15, Nombre = "Automóviles" }, new Areas { AreasId = 16, Nombre = "Reparaciones" }, new Areas { AreasId = 17, Nombre = "Banca" }, new Areas { AreasId = 18, Nombre = "Maquinaría" } };

                //TiposInversiones
                var tipoEsperado = new TiposInversiones[3] { new TiposInversiones { TiposInversionesId = 1, Nombre = "Business Angels" }, new TiposInversiones { TiposInversionesId = 2, Nombre = "Crownfunding" }, new TiposInversiones { TiposInversionesId = 3, Nombre = "Venture Capital" } };

                //Rating
                var ratingEsperado = new Rating[4] { new Rating { Nombre = "A" }, new Rating { RatingId = 2, Nombre = "B" }, new Rating { RatingId = 3, Nombre = "C" }, new Rating { RatingId = 4, Nombre = "D" } };

                string[] idAreas = new string[1] { "1" };
                string[] idRating = new string[1] { "1" };
                string[] idTiposInversion = new string[1] { "1" };

                SelectedPreferenciasForInversor preferencias = new SelectedPreferenciasForInversor
                {
                    IdsToAddAreas = null,
                    IdsToAddRating = idRating,
                    IdsToAddTiposInversion = null
                };

                //Act
                var result = controller.SelectPreferenciasForInversor(preferencias);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                SelectPreferenciasForInversorViewModel model = viewResult.Model as SelectPreferenciasForInversorViewModel;

                Assert.Equal(areaEsperada, model.Areas, Comparer.Get<Areas>((a1, a2) => a1.Nombre == a2.Nombre));
                Assert.Equal(tipoEsperado, model.TiposInversiones, Comparer.Get<TiposInversiones>((t1, t2) => t1.Nombre == t2.Nombre));
                Assert.Equal(ratingEsperado, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
            }
        }
    }
}
