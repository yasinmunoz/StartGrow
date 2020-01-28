using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StartGrow.Controllers;
using StartGrow.Data;
using StartGrow.Models;
using StartGrow.Models.SolicitudViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StartGrow.UT.Controllers.SolicitudesControllerUT
{
    public class SolicitudesController_Details_test
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
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;
        private DefaultHttpContext solicitudContext;

        public SolicitudesController_Details_test()
        {
            _contextOptions = CreateNewContextOptions();
            // Insert seed data into the database using one instance of the
            context = new ApplicationDbContext(_contextOptions);


            context.Users.Add(new Trabajador
            {
                UserName = "sergio@uclm.es",
                Email = "sergio@uclm.es",
                Apellido1 = "Ruiz",
                Apellido2 = "Villafranca",
                Domicilio = "C/Hellin",
                Municipio = "Albacete",
                NIF = "06290424",
                Nacionalidad = "Española",
                PaisDeResidencia = "España",
                Provincia
            = "Albacete"
            });


            Areas area = new Areas { Nombre = "TIC" };
            context.Areas.Add(area);

            TiposInversiones tipo = new TiposInversiones { Nombre = "Crowdfunding" };

            context.TiposInversiones.Add(tipo);
            Rating rating1 = new Rating { Nombre = "A" };
            Rating rating2 = new Rating { Nombre = "F" };

            context.Rating.Add(rating1);
            context.Rating.Add(rating2);

            context.Proyecto.Add(new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = 10, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = 5, Progreso = 0, RatingId = 1 });
            context.Proyecto.Add(new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 2 });
            context.Proyecto.Add(new Proyecto { ProyectoId = 3, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "EINSTEIN-MANIA", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 1 });

            context.Solicitud.Add(new Solicitud
            {
                SolicitudId = 1,
                Estado = Estados.Aceptada,
                Proyecto = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = 10, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = 5, Progreso = 0, RatingId = 1 },
                Trabajador = new Trabajador
                {
                    UserName = "sergio@uclm.es",
                    Email = "sergio@uclm.es",
                    Apellido1 = "Ruiz",
                    Apellido2 = "Villafranca",
                    Domicilio = "C/Hellin",
                    Municipio = "Albacete",
                    NIF = "06290424",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete"
                }

            });

            context.Solicitud.Add(new Solicitud
            {
                SolicitudId = 2,
                Estado = Estados.Rechazada,
                Proyecto = new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 2 },
                Trabajador = new Trabajador
                {
                    UserName = "sergio@uclm.es",
                    Email = "sergio@uclm.es",
                    Apellido1 = "Ruiz",
                    Apellido2 = "Villafranca",
                    Domicilio = "C/Hellin",
                    Municipio = "Albacete",
                    NIF = "06290424",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete"
                }

            });

            context.SaveChanges();

            foreach (var proyecto in context.Proyecto.ToList())
            {
                context.ProyectoAreas.Add(new ProyectoAreas { Proyecto = proyecto, Areas = context.Areas.First() });
                context.ProyectoTiposInversiones.Add(new ProyectoTiposInversiones { Proyecto = proyecto, TiposInversiones = context.TiposInversiones.First() });

            }
            context.SaveChanges();

            //how to simulate the connection 
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("sergio@uclm.es");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            solicitudContext = new DefaultHttpContext();
            solicitudContext.User = identity;

        }

        //=======================================================================================================================================
        //===============================================PRUEBAS DEL METODO GET==================================================================
        //=======================================================================================================================================

        [Fact]
        public async Task DetailsSinID()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                DetailsViewModel detailsView = new DetailsViewModel();
                detailsView.ids = new int[0];

                //Act
                var result = await controller.Details(detailsView);

                //Assert

                var viewResult = Assert.IsType<NotFoundResult>(result);



            }
        }
        [Fact]
        public async Task DetailsSolicitudNoEncontrada()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;
                DetailsViewModel detailsView = new DetailsViewModel();
                detailsView.ids = new int[] { context.Solicitud.Last().SolicitudId + 1 };
                //Act
                var result = await controller.Details(detailsView);

                //Assert

                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }
        [Fact]
        public async Task DetailsSolicitudEncontrada()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                DetailsViewModel detailsView = new DetailsViewModel();
                detailsView.ids = new int[] { 1, 2 };

                Trabajador trabajador = new Trabajador
                {
                    UserName = "sergio@uclm.es",
                    Email = "sergio@uclm.es",
                    Apellido1 = "Ruiz",
                    Apellido2 = "Villafranca",
                    Domicilio = "C/Hellin",
                    Municipio = "Albacete",
                    NIF = "06290424",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia
            = "Albacete"
                };

                var solicitudesEsperadas = new Solicitud[]
                {
                new Solicitud
            {
                SolicitudId = 1,
                Estado = Estados.Aceptada,
                Proyecto = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = 10, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = 5, Progreso = 0, RatingId = 1, Rating = new Rating{  Nombre = "A"} },
            },

                    new Solicitud
            {
                SolicitudId = 2,
                Estado = Estados.Rechazada,
                Proyecto = new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 2, Rating = new Rating { Nombre = "F"} },

                }

                    };

                //Act
                var result = await controller.Details(detailsView);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as IEnumerable<Solicitud>;

                Assert.Equal(solicitudesEsperadas, model, Comparer.Get<Solicitud>((p1,p2) => p1.SolicitudId == p2.SolicitudId 
                && p1.Estado == p2.Estado && p1.FechaSolicitud == p2.FechaSolicitud));
                Assert.Equal(solicitudesEsperadas[0].Proyecto, model.ElementAt(0).Proyecto, Comparer.Get<Proyecto>((p1,p2) => p1.ProyectoId == p2.ProyectoId
                && p1.RatingId == p2.RatingId
                && p1.FechaExpiracion == p2.FechaExpiracion
                && p1.Importe == p2.Importe
                && p1.Interes == p2.Interes
                && p1.Nombre == p2.Nombre
                && p1.Plazo == p2.Plazo
                && p1.Progreso == p2.Progreso
                && p1.NumInversores == p2.NumInversores));

                Assert.Equal(solicitudesEsperadas[1].Proyecto, model.ElementAt(1).Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.ProyectoId == p2.ProyectoId
                && p1.RatingId == p2.RatingId
                && p1.FechaExpiracion == p2.FechaExpiracion
                && p1.Importe == p2.Importe
                && p1.Interes == p2.Interes
                && p1.Nombre == p2.Nombre
                && p1.Plazo == p2.Plazo
                && p1.Progreso == p2.Progreso
                && p1.NumInversores == p2.NumInversores));
            }
        }
    }
}