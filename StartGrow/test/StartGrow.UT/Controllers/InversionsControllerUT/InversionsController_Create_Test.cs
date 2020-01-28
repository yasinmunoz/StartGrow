using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StartGrow.Controllers;
using StartGrow.Data;
using StartGrow.Models;
using StartGrow.Models.InversionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StartGrow.UT.Controllers.InversionsControllerUT
{
    public class InversionsController_Create_Test
    {
        //Aislamiento del controlador utilizando la base de datos InMemory.
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
        private DefaultHttpContext inversionContext;

        //Constructor -> Iniciamos la base de datos InMemory.
        public InversionsController_Create_Test()
        {
            _contextOptions = CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);

            // Insert seed data into the database using one instance of the context

            //Tipos de Inversiones        
            TiposInversiones tipo1 = new TiposInversiones { Nombre = "Crowdfunding" };
            TiposInversiones tipo2 = new TiposInversiones { Nombre = "Venture Capital" };
            context.TiposInversiones.Add(tipo1);
            context.TiposInversiones.Add(tipo2);

            //Rating
            Rating rating = new Rating { Nombre = "A" };
            context.Rating.Add(rating);

            //Monedero
            Monedero monedero = new Monedero
            {
                MonederoId = 1,
                Dinero = 8000
            };
            context.Monedero.Add(monedero);

            //Proyectos           
            Proyecto proyecto1 = new Proyecto
            {
                ProyectoId = 1,
                FechaExpiracion = new DateTime(2019, 01, 23),
                Importe = 30000,
                Interes = (float)5.90,
                MinInversion = 50,
                Nombre = "E-MEDICA",
                NumInversores = 0,
                Plazo = 12,
                Progreso = 0,
                Rating = rating
            };
            context.Proyecto.Add(proyecto1);
           
            //Inversor
            Inversor inversor = new Inversor
            {
                Id = "1",
                Nombre = "Yasin",
                Email = "yasin@uclm.es",
                Apellido1 = "Muñoz",
                Apellido2 = "El Merabety",
                Domicilio = "C/Gabriel Ciscar",
                Municipio = "Albacete",
                NIF = "47446245",
                Nacionalidad = "Española",
                PaisDeResidencia = "España",
                Provincia = "Albacete",
                PhoneNumber = "684010548",
                PasswordHash = "password",
                UserName = "yasin@uclm.com",
                Monedero = monedero
            };
            context.Users.Add(inversor);

            context.SaveChanges();

            //Simulación conexión de un usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("yasin@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            inversionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            inversionContext.User = identity;
        }

                                                                        //METODO GET        

        [Fact]
        public async Task CreateGet_ConProyectosEnIdsToAdd()
        {
            using (context)
            {
                                                                              // ARRANGE                
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Array ids con un proyecto de id 1
                String[] ids = new string[1] { "1" };
                SelectedProyectosForInversionViewModel proyectos = new SelectedProyectosForInversionViewModel() { IdsToAdd = ids};

                //Rating esperado
                var expectedrating = new Rating { RatingId = 1, Nombre = "A" };

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };

                //SelectList TiposInversiones Esperado
                TiposInversiones tipo1 = new TiposInversiones { Nombre = "Crowdfunding" };
                TiposInversiones tipo2 = new TiposInversiones { Nombre = "Venture Capital" };

                var tipos = new List<TiposInversiones> { tipo1, tipo2 };
                var expectedTipos = new SelectList(tipos.Select(r => r.Nombre.ToList()));
                
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"                    
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    Intereses = (float)expectedProyectos[0].Interes,
                    Proyecto = expectedProyectos[0],
                    EstadosInversiones = "En Curso",
                    TipoInversionesId = 1,
                    Inversor = expectedinversor,
                    ProyectoId = expectedProyectos[0].ProyectoId,
                    InversionId = 1,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel { inversion = expectedInversion } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                //Act
                var result = controller.Create(proyectos);

                                                                        //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1,p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));                
            }
        }

        [Fact]
        public async Task CreateGet_SinIdsToAdd()
        {
            using (context)
            {
                // Arrange
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                SelectedProyectosForInversionViewModel proyectos = new SelectedProyectosForInversionViewModel();

                //Act
                var result = controller.Create(proyectos);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "SelectProyectosForInversion");
            }
        }

        [Fact]
        public async Task CreateGet_SinProyectosEnIdsToAdd()
        {
            using (context)
            {
                // Arrange
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                SelectedProyectosForInversionViewModel proyectos = new SelectedProyectosForInversionViewModel();
                proyectos.IdsToAdd = new string[0]; //IdsToAdd sin Proyectos seleccionados

                //Act
                var result = controller.Create(proyectos);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "SelectProyectosForInversion");
            }
        }

                                                                         
                                                                           //METODO POST        

        [Fact]
        //No se selecciona ningún tipo de inversión. 
        //No se introduce una couta válida. Cuota inferior a la inversión mínima.
        public async Task CreatePost_MalCuotaInferiorInvMin_MalTipoInversion() 
        {
            using (context)
            {
                                                                                            // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };                                                          

                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    EstadosInversiones = null,
                    Intereses = 0,
                    InversionId = 0,
                    Inversor = null,
                    InversorId = null,
                    Proyecto = expectedProyectos[0],                    
                    TipoInversiones = null,
                    TipoInversionesId = 0,                                        
                    Total = 0,                    
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {                    
                    Cantidad = 8000,
                    Cuota = 0,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = null,
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Mensaje de Error al introducir una couta y un tipo de inversion incorrecto                
                var error = viewResult.ViewData.ModelState["Cuota y Tipo de Inversión incorrecto"].Errors.FirstOrDefault();
                Assert.Equal($"Cuota y Tipo de Inversión incorrectos en {currentInversion.inversiones[0].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.", error.ErrorMessage);

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1, p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));                
            }
        }

        [Fact]
        //Se selecciona un tipo de inversión.
        //No se introduce una couta válida. Cuota inferior a la inversion mínima.
        public async Task CreatePost_MalCuotaInferiorInvMin_BienTipoInversion() 
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    EstadosInversiones = null,
                    Intereses = 0,
                    InversionId = 0,
                    Inversor = null,
                    InversorId = null,
                    Proyecto = expectedProyectos[0],
                    TipoInversiones = null,
                    TipoInversionesId = 0,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 0,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = "Crowdfunding",
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Mensaje de Error al introducir una couta incorrecta.                
                var error = viewResult.ViewData.ModelState["Ha introducido una cuota incorrecta"].Errors.FirstOrDefault();
                Assert.Equal($"Ha introducido una cuota incorrecta en {currentInversion.inversiones[0].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.", error.ErrorMessage);

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1, p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));
            }
        }

        [Fact]
        //No se selecciona ningún tipo de inversión. 
        //No se introduce una couta válida. Cuota superior al Monedero.
        public async Task CreatePost_MalCuotaSuperiorMonedero_MalTipoInversion()
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    EstadosInversiones = null,
                    Intereses = 0,
                    InversionId = 0,
                    Inversor = null,
                    InversorId = null,
                    Proyecto = expectedProyectos[0],
                    TipoInversiones = null,
                    TipoInversionesId = 0,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 8200,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = null,
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Mensaje de Error al introducir una couta y un tipo de inversion incorrecto                
                var error = viewResult.ViewData.ModelState["Cuota y Tipo de Inversión incorrecto"].Errors.FirstOrDefault();
                Assert.Equal($"Cuota y Tipo de Inversión incorrectos en {currentInversion.inversiones[0].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.", error.ErrorMessage);

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1, p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));
            }
        }

        [Fact]
        //Se selecciona un tipo de inversión.
        //No se introduce una couta válida. Cuota superior al monerdero.
        public async Task CreatePost_MalCuotaSuperiorMonedero_BienTipoInversion()
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    EstadosInversiones = null,
                    Intereses = 0,
                    InversionId = 0,
                    Inversor = null,
                    InversorId = null,
                    Proyecto = expectedProyectos[0],
                    TipoInversiones = null,
                    TipoInversionesId = 0,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 9000,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = "Crowdfunding",
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Mensaje de Error al introducir una couta incorrecta.                
                var error = viewResult.ViewData.ModelState["Ha introducido una cuota incorrecta"].Errors.FirstOrDefault();
                Assert.Equal($"Ha introducido una cuota incorrecta en {currentInversion.inversiones[0].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.", error.ErrorMessage);

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1, p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));
            }
        }

        [Fact]
        //No se selecciona ningún tipo de inversión.
        //Se introduce una couta válida. 
        public async Task CreatePost_BienCuota_MalTipoInversion() 
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 0,
                    EstadosInversiones = null,
                    Intereses = 0,
                    InversionId = 0,
                    Inversor = null,
                    InversorId = null,
                    Proyecto = expectedProyectos[0],
                    TipoInversiones = null,
                    TipoInversionesId = 0,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 5000,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = null,
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
                InversionesCreateViewModel currentInversion = viewResult.Model as InversionesCreateViewModel;

                //Mensaje de Error al no seleccionar ningún tipo de inversión.                
                var error = viewResult.ViewData.ModelState["No ha seleccionado un tipo de inversión"].Errors.FirstOrDefault();
                Assert.Equal($"No ha seleccionado un tipo de inversión en {currentInversion.inversiones[0].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.", error.ErrorMessage);

                //Datos del Inversor
                Assert.Equal(currentInversion, expectedInversionCV, Comparer.Get<InversionesCreateViewModel>
                    ((p1, p2) => p1.Name == p2.Name && p1.FirstSurname == p2.FirstSurname && p1.SecondSurname == p2.SecondSurname && p1.Cantidad == p2.Cantidad));

                //Datos del Proyecto
                Assert.Equal(currentInversion.inversiones[0].inversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>
                    ((p1, p2) => p1.Nombre == p2.Nombre && p1.Plazo == p2.Plazo && p1.RatingId == p2.RatingId));
            }
        }

        [Fact]
        //Se selecciona Business Angels como tipo de inversión.
        //Se introduce una couta válida. 
        public async Task CreatePost_BienCuota_BusinessAngelsTipoInversion()
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(inversionContext, new
                Microsoft.AspNetCore.Mvc.ViewFeatures.SessionStateTempDataProvider());

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 350,
                    Intereses = (float)expectedProyectos[0].Interes,
                    Proyecto = expectedProyectos[0],
                    EstadosInversiones = "En Curso",
                    TipoInversionesId = 1,
                    Inversor = expectedinversor,
                    ProyectoId = expectedProyectos[0].ProyectoId,
                    InversionId = 1,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 350,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = "Business Angels",
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var currentInversion = context.Inversion.Include(s => s.Proyecto).FirstOrDefault();

                Assert.Equal(currentInversion, expectedInversion, Comparer.Get<Inversion>((p1, p2) => p1.TipoInversionesId == p2.TipoInversionesId 
                && p1.Cuota == p2.Cuota
                && p1.Intereses == p2.Intereses
                && p1.Inversor == p2.Inversor
                && p1.Total == p2.Total));

                Assert.Equal(currentInversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
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
        //Se selecciona Crownfunding como tipo de inversión.
        //Se introduce una couta válida. 
        public async Task CreatePost_BienCuota_CrownfundingTipoInversion()
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(inversionContext, new
                Microsoft.AspNetCore.Mvc.ViewFeatures.SessionStateTempDataProvider());

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 350,
                    Intereses = (float)expectedProyectos[0].Interes,
                    Proyecto = expectedProyectos[0],
                    EstadosInversiones = "En Curso",
                    TipoInversionesId = 2,
                    Inversor = expectedinversor,
                    ProyectoId = expectedProyectos[0].ProyectoId,
                    InversionId = 1,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 350,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = "Crownfunding",
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var currentInversion = context.Inversion.Include(s => s.Proyecto).FirstOrDefault();

                Assert.Equal(currentInversion, expectedInversion, Comparer.Get<Inversion>((p1, p2) => p1.TipoInversionesId == p2.TipoInversionesId
                && p1.Cuota == p2.Cuota
                && p1.Intereses == p2.Intereses
                && p1.Inversor == p2.Inversor
                && p1.Total == p2.Total));

                Assert.Equal(currentInversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
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
        //Se selecciona Venture Capital como tipo de inversión.
        //Se introduce una couta válida. 
        public async Task CreatePost_BienCuota_VentureCapitalTipoInversion()
        {
            using (context)
            {
                // ARRANGE
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(inversionContext, new
                Microsoft.AspNetCore.Mvc.ViewFeatures.SessionStateTempDataProvider());

                //Proyectos esperados
                var expectedProyectos = new Proyecto[1] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 } };
                //Monedero esperado
                var expectedMonedero = new Monedero { Dinero = 8000 };

                //Inversor esperado
                Inversor expectedinversor = new Inversor
                {
                    Id = "1",
                    Nombre = "Yasin",
                    Email = "yasin@uclm.es",
                    Apellido1 = "Muñoz",
                    Apellido2 = "El Merabety",
                    Domicilio = "C/Gabriel Ciscar",
                    Municipio = "Albacete",
                    NIF = "47446245",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PhoneNumber = "684010548",
                    PasswordHash = "password",
                    UserName = "yasin@uclm.com"
                };

                //inversion Esperada
                Inversion expectedInversion = new Inversion
                {
                    Cuota = 350,
                    Intereses = (float)expectedProyectos[0].Interes,
                    Proyecto = expectedProyectos[0],
                    EstadosInversiones = "En Curso",
                    TipoInversionesId = 3,
                    Inversor = expectedinversor,
                    ProyectoId = expectedProyectos[0].ProyectoId,
                    InversionId = 1,
                    Total = 0,
                };
                IList<InversionCreateViewModel> inversiones = new InversionCreateViewModel[1] { new InversionCreateViewModel
                {
                    Cantidad = 8000,
                    Cuota = 350,
                    Interes = (float) 5.9,
                    MinInver = 50,
                    NombreProyecto = "E-MEDICA",
                    Plazo = 12,
                    Proyecto = null,
                    ProyectoId = 1,
                    Rating = "A",
                    TiposInversionSelected = "Venture Capital",
                    inversion = expectedInversion

                } };
                InversionesCreateViewModel expectedInversionCV = new InversionesCreateViewModel
                {
                    Name = expectedinversor.Nombre,
                    FirstSurname = expectedinversor.Apellido1,
                    SecondSurname = expectedinversor.Apellido2,
                    Cantidad = expectedMonedero.Dinero,
                    inversiones = inversiones
                };

                // ACT 
                var result = controller.Create(expectedInversionCV);

                // ASSERT
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var currentInversion = context.Inversion.Include(s => s.Proyecto).FirstOrDefault();

                Assert.Equal(currentInversion, expectedInversion, Comparer.Get<Inversion>((p1, p2) => p1.TipoInversionesId == p2.TipoInversionesId
                && p1.Cuota == p2.Cuota
                && p1.Intereses == p2.Intereses
                && p1.Inversor == p2.Inversor
                && p1.Total == p2.Total));

                Assert.Equal(currentInversion.Proyecto, expectedInversionCV.inversiones[0].inversion.Proyecto, Comparer.Get<Proyecto>((p1, p2) => p1.Nombre == p2.Nombre
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

