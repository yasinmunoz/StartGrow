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
    public class SolicitudesController_Create_Test
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

        public SolicitudesController_Create_Test()
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

            context.Proyecto.Add(new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null });
            context.Proyecto.Add(new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "GRE-GYM", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null });
            context.Proyecto.Add(new Proyecto { ProyectoId = 3, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = null, MinInversion = 50, Nombre = "EINSTEIN-MANIA", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = 1 });


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
        public async Task Create_ProyectosNULL()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel();
                //Select List Rating
                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                Trabajador trabajadorEsperado = new Trabajador
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
                SolicitudesCreateViewModel solicitudEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2
                };

                //Act
                var result = controller.Create(proyectos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["ProyectoNoSeleccionado"].Errors.FirstOrDefault();
                Assert.Equal(currentSolicitud, solicitudEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal("Por favor, selecciona un proyecto para poder crear la solicitud", error.ErrorMessage);
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }
        [Fact]
        public async Task Create_SinProyectos()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = new string [0] };
                //Select List Rating
                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                Trabajador trabajadorEsperado = new Trabajador
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
                SolicitudesCreateViewModel solicitudEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2
                };

                //Act
                var result = controller.Create(proyectos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["ProyectoNoSeleccionado"].Errors.FirstOrDefault();
                Assert.Equal(currentSolicitud, solicitudEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal("Por favor, selecciona un proyecto para poder crear la solicitud", error.ErrorMessage);
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }
        [Fact]
        public async Task Create_ConProyectos()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(proyectos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }
        }


        //========================================================================================================================================
        //===============================================PRUEBAS DEL METODO POST==================================================================
        //========================================================================================================================================
        [Fact]
        public async Task CreatePost_Aceptada_RatingF()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "F" } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["SolicitudIncorrecta"].Errors.FirstOrDefault();
                Assert.Equal("La solicitud de  POCHOLO RULES, no puede estar aprobada y tener una calificacion de F o viceversa", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }

        }
        [Fact]
        public async Task CreatePost_Rechazada_RatingA()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Rechazada, rating = "A" } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["SolicitudIncorrecta"].Errors.FirstOrDefault();
                Assert.Equal("La solicitud de  POCHOLO RULES, no puede estar aprobada y tener una calificacion de F o viceversa", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }

        }
        [Fact]
        public async Task CreatePost_Aceptada_Rating_SinInteresPlazo()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "A" } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["NoInteresPlazo"].Errors.FirstOrDefault();
                Assert.Equal("No se ha introducido correctamente el plazo o el interes del proyecto POCHOLO RULES", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }

        }
        [Fact]
        public async Task CreatePost_Aceptada_Rating_InteresPlazo_0()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "A", interes = 0, plazo = 0 } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["NoInteresPlazo"].Errors.FirstOrDefault();
                Assert.Equal("No se ha introducido correctamente el plazo o el interes del proyecto POCHOLO RULES", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }

        }
        [Fact]
        public async Task CreatePost_Aceptada_Rating_Plazo_0()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "A", interes = 5, plazo = 0 } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["NoInteresPlazo"].Errors.FirstOrDefault();
                Assert.Equal("No se ha introducido correctamente el plazo o el interes del proyecto POCHOLO RULES", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }
        }
        [Fact]
        public async Task CreatePost_Aceptada_Rating_Interes_0()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = null, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = null, Progreso = 0, RatingId = null };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "A", interes = 0, plazo = 1 } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                SolicitudesCreateViewModel currentSolicitud = viewResult.Model as SolicitudesCreateViewModel;
                var error = viewResult.ViewData.ModelState["NoInteresPlazo"].Errors.FirstOrDefault();
                Assert.Equal("No se ha introducido correctamente el plazo o el interes del proyecto POCHOLO RULES", error.ErrorMessage);
                Assert.Equal(currentSolicitud, solicitudCVEsperada, Comparer.Get<SolicitudesCreateViewModel>((p1, p2) =>
                p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud, solicitudCVEsperada.Solicitudes[0].solicitud, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId));
                Assert.Equal(currentSolicitud.Solicitudes[0].solicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion && p1.ProyectoId == p2.ProyectoId));
                Assert.Equal(ratingEsperados, (SelectList)viewResult.ViewData["Rating"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(estadosEsperados, (SelectList)viewResult.ViewData["Estados"], Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }
        }

        [Fact]
        public async Task CreatePost_Rechazada_Rating_F()
        {
            using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;

                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = 0, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = 0, Progreso = 0, Rating = new Rating { Nombre = "F" } };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Rechazada, rating = "F", interes = 0, plazo = 0 } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                var currentSolicitud = context.Solicitud.Include(s => s.Proyecto).FirstOrDefault();

                Assert.Equal(currentSolicitud, solicitudEsperada, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId
                 && p1.Trabajador == p2.Trabajador));

                Assert.Equal(currentSolicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion
                && p1.ProyectoId == p2.ProyectoId
                && p1.Rating == p2.Rating
                && p1.Plazo == p2.Plazo
                && p1.Interes == p2.Interes
                && p1.Importe == p2.Importe
                && p1.MinInversion == p2.MinInversion
                && p1.Progreso == p2.Progreso));

                Assert.Equal(viewResult.ActionName, "Details");
            }

        }
        [Fact]
        public async Task CreatePost_Aceptada_Rating_ConInteresPlazo()
        {
            //var host= Program.BuildWebHost(null);


           
            //    using (var scope = host.Services.CreateScope())
            //    {
            //        var services = scope.ServiceProvider;
            //    var contexto = services.GetRequiredService(Type.GetType("Microsoft.AspNetCore.Session.DistributedSession")); //<Microsoft.AspNetCore.Session.DistributedSession>();

            //    }

                using (context)
            {
                // Arrenge
                var controller = new SolicitudesController(context);


                controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(solicitudContext, new
Microsoft.AspNetCore.Mvc.ViewFeatures.SessionStateTempDataProvider());
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = solicitudContext;
                
                Rating rating1 = new Rating { Nombre = "A" };
                Rating rating2 = new Rating { Nombre = "F" };

                var ratings = new List<Rating> { rating1, rating2 };

                var ratingEsperados = new SelectList(ratings.Select(r => r.Nombre.ToList()));
                var estados = new List<Estados> { Estados.Aceptada, Estados.Rechazada };

                var estadosEsperados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));

                String[] ids = new string[1] { "1" };
                SelectedProyectosForSolicitudViewModel proyectos = new SelectedProyectosForSolicitudViewModel() { IdsToAdd = ids };
                Proyecto proyectoEsperado = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 70000, Interes = 1, MinInversion = 50, Nombre = "POCHOLO RULES", NumInversores = 0, Plazo = 1, Progreso = 0, Rating = new Rating { Nombre = "F" } };
                Trabajador trabajadorEsperado = new Trabajador
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
                Solicitud solicitudEsperada = new Solicitud { Proyecto = proyectoEsperado, Trabajador = trabajadorEsperado, FechaSolicitud = DateTime.Now };
                IList<SolicitudCreateViewModel> solicitudes = new SolicitudCreateViewModel[1] { new SolicitudCreateViewModel { solicitud = solicitudEsperada, estados = Estados.Aceptada, rating = "A", interes = 1, plazo = 1 } };
                SolicitudesCreateViewModel solicitudCVEsperada = new SolicitudesCreateViewModel
                {
                    Name = trabajadorEsperado.Nombre,
                    FirstSurname = trabajadorEsperado.Apellido1,
                    SecondSurname = trabajadorEsperado.Apellido2,
                    Solicitudes = solicitudes

                };

                //Act
                var result = controller.Create(solicitudCVEsperada);

                //Assert

                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var currentSolicitud = context.Solicitud.Include(s => s.Proyecto).FirstOrDefault();

                Assert.Equal(currentSolicitud, solicitudEsperada, Comparer.Get<Solicitud>((p1, p2) => p1.Estado == p2.Estado
                 && p1.SolicitudId == p2.SolicitudId
                 && p1.Trabajador == p2.Trabajador));

                Assert.Equal(currentSolicitud.Proyecto, solicitudCVEsperada.Solicitudes[0].solicitud.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
                && p1.FechaExpiracion == p2.FechaExpiracion
                && p1.ProyectoId == p2.ProyectoId
                && p1.Rating == p2.Rating
                && p1.Plazo == p2.Plazo
                && p1.Interes == p2.Interes
                && p1.Importe == p2.Importe
                && p1.MinInversion == p2.MinInversion
                && p1.Progreso == p2.Progreso));

                Assert.Equal(viewResult.ActionName, "Details");
            }
            
        }
    }

}