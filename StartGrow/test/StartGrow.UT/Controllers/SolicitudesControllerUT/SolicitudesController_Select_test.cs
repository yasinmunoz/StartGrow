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
using StartGrow.Models.SolicitudViewModels;
using StartGrow.Controllers;
namespace StartGrow.UT.Controllers.SolicitudesControllerUT
{
    public class SolicitudesController_Select_test
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
        Microsoft.AspNetCore.Http.DefaultHttpContext solicitudContext;
        public SolicitudesController_Select_test()
        {
            _contextOptions = CreateNewContextOptions();
            // Insert seed data into the database using one instance of the
            context = new ApplicationDbContext(_contextOptions);
            
            
            context.Users.Add(new Trabajador { UserName = "sergio@uclm.es", Email = "sergio@uclm.es", Apellido1 = "Ruiz", Apellido2 = "Villafranca",
            Domicilio ="C/Hellin", Municipio ="Albacete" , NIF = "06290424" ,Nacionalidad = "Española", PaisDeResidencia = "España" , Provincia
            = "Albacete"} );
            

            Areas area = new Areas { Nombre = "TIC" };
            context.Areas.Add(area);

            TiposInversiones tipo = new TiposInversiones { Nombre = "Crowdfunding" };

           // TiposInversiones tipo2 = new TiposInversiones { Nombre = "TIC" };

            context.TiposInversiones.Add(tipo);
       //     context.TiposInversiones.Add(tipo2);

            context.Proyecto.Add(new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null });
            context.Proyecto.Add(new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null });
            context.Proyecto.Add(new Proyecto { ProyectoId = 3, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "EINSTEIN-MANIA", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 1 });


            

           
            
            context.SaveChanges();

            foreach (var proyecto in context.Proyecto.ToList())
            {
                context.ProyectoAreas.Add(new ProyectoAreas { Proyecto = proyecto, Areas = context.Areas.First() });
                context.ProyectoTiposInversiones.Add(new ProyectoTiposInversiones { Proyecto = proyecto, TiposInversiones = context.TiposInversiones.First() });

            }
            /*
            Proyecto proyecto1 = new Proyecto { ProyectoId = 4, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "EINSTEIN-MANIA", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
            context.Proyecto.Add(proyecto1);
            context.ProyectoTiposInversiones.Add(new ProyectoTiposInversiones { Proyecto = proyecto1, TiposInversiones = tipo2 });
           */
            context.SaveChanges();

            //how to simulate the connection 
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("sergio@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            solicitudContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            solicitudContext.User = identity;
            
        }

        //=======================================================================================================================================
        //===============================================PRUEBAS DEL METODO GET==================================================================
        //=======================================================================================================================================


        [Fact]
        public async Task Select_SinParametros()
        {
          
            
            using (context)
            {
                //Arrenge
                //Proyectos esperados de que se devuelvan 
                var proyectosesperados = new Proyecto[2]
                {
                    new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null },
                    new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null }

                };
                
                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones [1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas [1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0];
                var result = controller.SelectProyectosForSolicitud(null, vacia, vacia, null, null);

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso==p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }
        }

        [Fact]
        public async Task Select_FiltroDeNombre()
        {
            //Arrenge 
            //Base SQL ya generada con datos incluidos 
            using (context)
            {
                var proyectosesperados = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null } };

                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0] ;
                var result = controller.SelectProyectosForSolicitud("POCHOLO RULES", vacia, vacia, null, null);

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }
        }
        [Fact]
        public async Task Select_FiltroDeCapital()
        {
            //Arrenge 
            //Base SQL ya generada con datos incluidos 
            using (context)
            {
                var proyectosesperados = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null } };

                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0] ;
                var result = controller.SelectProyectosForSolicitud(null, vacia, vacia, 65000, null);

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }
        }
        [Fact]
        public async Task Select_FiltroDeTipo()
        {
            //Arrenge 
            //Base SQL ya generada con datos incluidos 
            using (context)
            {
                var proyectosesperados = new Proyecto[2]
                {
                    new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null },
                    new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null }

                };
                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0];
                var result = controller.SelectProyectosForSolicitud(null, new string[] { "Crowdfunding" }, vacia, null, null);

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }
        }
        [Fact]
        public async Task Select_FiltroDeAreas()
        {
            //Arrenge 
            //Base SQL ya generada con datos incluidos 
            using (context)
            {
                var proyectosesperados = new Proyecto[2]
                {
                    new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null },
                    new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null }

                };
                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0];
                var result = controller.SelectProyectosForSolicitud(null, vacia, new string[1] {"TIC" }, null, null);

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }
        }
        [Fact]
        public async Task Select_FiltroDeFecha()
        {
            //Arrenge 
            //Base SQL ya generada con datos incluidos 
            using (context)
            {
                var proyectosesperados = new Proyecto[2]
                {
                    new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null },
                    new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null }

                };
                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                //Act
                string[] vacia = new string[0];
                var result = controller.SelectProyectosForSolicitud(null, vacia, vacia,null, new DateTime(2019,1,23));

                //Assert 
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre)); Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso));
            }
        }
        //=======================================================================================================================================
        //===============================================PRUEBAS DEL METODO POST==================================================================
        //=======================================================================================================================================

        [Fact]
        public async Task Select_ProyectoSeleccionados()
        {
            using (context)
            {
                //Arrange

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
                String[] ids = new string[1] { "1" };

                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel { IdsToAdd = ids };

                // Act

                var result = controller.SelectProyectosForSolicitud(proyectos);

                // Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");
                var resultadoproyectos = viewResult.RouteValues.Values.First();
                Assert.Equal(proyectos.IdsToAdd, resultadoproyectos);
            }





        }

        [Fact]
        public async Task Select_NoProyectoSeleccionados()
        {
            using (context)
            {
                //Arrange

                var controller = new SolicitudesController(context);
                controller.ControllerContext.HttpContext = solicitudContext;
               var proyectosesperados = new Proyecto[2]
                {
                    new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null },
                    new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null }

                };
                //Areas y Tipos que se espera que se retornen

                var tiposEsperados = new TiposInversiones[1] { new TiposInversiones { Nombre = "Crowdfunding" } };
                var areasEsperadas = new Areas[1] { new Areas { Nombre = "TIC" } };

                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel { IdsToAdd = null };

                // Act

                var result = controller.SelectProyectosForSolicitud(proyectos);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                SelectProyectosForSolicitudViewModel model = viewResult.Model as SelectProyectosForSolicitudViewModel;
                //Comprobamos los proyectos devueltos
                Assert.Equal(proyectosesperados, model.proyectos, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.Importe == p2.Importe && p1.MinInversion == p2.MinInversion && p1.Progreso == p2.Progreso && p1.ProyectoId == p2.ProyectoId));
                //Comprobamos las areas y tipos devueltos 
                Assert.Equal(tiposEsperados, model.Tipos, Comparer.Get<TiposInversiones>((p1, p2) => p1.Nombre == p2.Nombre));
                Assert.Equal(areasEsperadas, model.areas, Comparer.Get<Areas>((p1, p2) => p1.Nombre == p2.Nombre));
            }





        }

    }
}
