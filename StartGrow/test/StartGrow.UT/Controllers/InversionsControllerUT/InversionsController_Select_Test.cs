using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StartGrow.Models;
using StartGrow.Data;
using StartGrow.Models.InversionViewModels;
using StartGrow.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace StartGrow.UT.Controllers.InversionsControllerUT
{
    public class InversionsController_Select_Test
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
         
            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("StartGrow")
                    .UseInternalServiceProvider(serviceProvider);
         
            return builder.Options;
        }

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;        
        Microsoft.AspNetCore.Http.DefaultHttpContext inversionContext;

        public InversionsController_Select_Test()
        {

            _contextOptions = CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);

            // Insert seed data into the database using one instance of the context
                
            //Areas Temáticas
            context.Areas.Add(new Areas { Nombre = "Sanidad" });

            //Rating
            var rating = new Rating { Nombre = "A" };
            context.Rating.Add(rating);

            //Tipos de Inversiones
            context.TiposInversiones.Add(new TiposInversiones { Nombre = "Crowdfunding" });

            //Proyecto
            context.Proyecto.Add (new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, Rating = rating});                       
            context.Proyecto.Add (new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, Rating = rating});
            //context.Proyecto.Add (new Proyecto { ProyectoId = 3, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 93000, Interes = (float) 4.50, MinInversion = 100, Nombre = "SUBSOLE", NumInversores = 0, Plazo = 6, Progreso = 0, RatingId = 1 });

            //Inversor
            context.Users.Add(new Inversor { UserName = "yasin@uclm.com", NIF = "47446245M", PhoneNumber = "684010548", Email = "yasin@uclm.com",
                Nombre = "Yasin", Apellido1 = "Muñoz", Apellido2 = "El Merabety", Domicilio = "Gabriel Ciscar, 26", Nacionalidad = "Española",
                PaisDeResidencia = "España", Provincia = "Albacete"});

            context.SaveChanges();

            foreach (var proyecto in context.Proyecto.ToList())
            {
                context.ProyectoAreas.Add(new ProyectoAreas { Proyecto = proyecto, Areas = context.Areas.First() });
                context.ProyectoTiposInversiones.Add(new ProyectoTiposInversiones { Proyecto = proyecto, TiposInversiones = context.TiposInversiones.First() });                
            }
            context.SaveChanges();

            //Simulación conexión de un usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("yasin@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            inversionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            inversionContext.User = identity;

        }

        [Fact]
        public async Task Select_camino_SinFiltro()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 }};

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" }};

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" }};
                
                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };                


                // ACT
                var result = controller.SelectProyectosForInversion(new string [0], new string[0], new string[0], null, null, null);

                //ASSERT                
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));                                                               
                // Check that both collections (expected and result returned) have the same elements with the same name
            }

        }

        [Fact]
        public async Task Select_camino_ConPlazo()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 14), Importe = 70000, Interes = (float)7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 } };
                                                          
                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT
                var result = controller.SelectProyectosForInversion(new string[0], new string[0], new string[0], null, null, 30);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));                   
                // Check that both collections (expected and result returned) have the same elements with the same name
            }
        }

        [Fact]
        public async Task Select_camino_ConInteres()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 }};



                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT
                var result = controller.SelectProyectosForInversion(new string[0], new string[0], new string[0], null, (float) 3.50, null);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task Select_camino_ConMinimoInversion()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3 } };                                                          

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT
                var result = controller.SelectProyectosForInversion(new string[0], new string[0], new string[0], 50, null, null);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name
            }
        }       

        [Fact]
        public async Task Select_caminoRating()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 1 }};

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT                
                var result = controller.SelectProyectosForInversion(new string[0], new string[0], new string[] {"A"}, null, null, null);
                
                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }
       

        [Fact]
        public async Task Select_caminoAreas()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 }};

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT                
                var result = controller.SelectProyectosForInversion(new string[0], new string[1] { "Sanidad" }, new string[0], null, null, null);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }        

        [Fact]
        public async Task Select_caminoTiposInversiones()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 }};

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                // ACT                
                var result = controller.SelectProyectosForInversion(new string[] { "Crowdfunding" }, new string[0], new string[0], null, null, null);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

                                                                        //PRUEBAS POST


        [Fact]
        public async Task Select_ProyectoSeleccionados()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                String[] ids = new string[1] { "1" };                

                SelectedProyectosForInversionViewModel Proyectos = new SelectedProyectosForInversionViewModel { IdsToAdd = ids };


                // ACT                
                var result = controller.SelectProyectosForInversion(Proyectos);

                //ASSERT  
                
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");
                var resultadoproyectos = viewResult.RouteValues.Values.First();
                Assert.Equal(Proyectos.IdsToAdd, resultadoproyectos);
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task Select_NoProyectoSeleccionados()
        {
            using (context)
            {
                // ARRANGE: Set the test case will use the inMemoty database created in the constructor
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 3},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 2 }};

                //Areas
                var expectedAreas = new Areas[1] { new Areas { Nombre = "Sanidad" } };

                //Tipos de Inversiones
                var expectedTiposInversiones = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };

                //Rating
                var expectedRating = new Rating[1] { new Rating { Nombre = "A" } };

                SelectedProyectosForInversionViewModel Proyectos = new SelectedProyectosForInversionViewModel { IdsToAdd = null };

                // ACT                
                var result = controller.SelectProyectosForInversion(Proyectos);

                //ASSERT  
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForInversionViewModel model = viewResult.Model as SelectProyectosForInversionViewModel;

                Assert.Equal(expectedProyectos, model.Proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));

                Assert.Equal(expectedTiposInversiones, model.TiposInversiones, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(expectedAreas, model.Areas, Comparer.Get<Areas>((a1, b2) => a1.Nombre == a1.Nombre));
                Assert.Equal(expectedRating, model.Rating, Comparer.Get<Rating>((r1, r2) => r1.Nombre == r2.Nombre));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }




    }
}
