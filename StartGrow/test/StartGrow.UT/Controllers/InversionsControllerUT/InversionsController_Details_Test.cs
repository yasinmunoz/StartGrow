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
    public class InversionsController_Details_Test
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
        public InversionsController_Details_Test()
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

            //Inversiones
            context.Inversion.Add(new Inversion
            {
                InversionId = 1,
                Cuota = 750,
                Intereses = (float)5.9,
                Total = (float)794.25,
                EstadosInversiones = "En Curso",
                TipoInversionesId = 1,
                Proyecto = new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime(2019, 01, 23), Importe = 30000, Interes = (float)5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1 },
                Inversor = new Inversor
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
                }

            });
            context.Inversion.Add(new Inversion
            {
                InversionId = 2,
                Cuota = 850,               
                Intereses = (float) 5.9,
                Total = (float) 911.625,
                EstadosInversiones = "En Curso",
                TipoInversionesId = 2,                
                Proyecto = new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime(2019, 01, 14), Importe = 70000, Interes = (float)7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 1 },
                Inversor = new Inversor
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
                }

            });

            context.SaveChanges();

            //Simulación conexión de un usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("yasin@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            inversionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            inversionContext.User = identity;
        }


        [Fact]       
        //Details sin ids
        public async Task Details_withoutIds()
        {
            using (context)
            {
                // Arrenge
                var controller = new InversionsController(context);  
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = inversionContext;
                InversionDetailsViewModel detailsViewModel = new InversionDetailsViewModel();

                //Act
                var result = await controller.Details(detailsViewModel);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");

            }
        }

        [Fact]        
        //Details con ids pero vacío.
        public async Task Details_Inversion_NotFound()
        {
            using (context)
            {
                //Arrange 
                var controller = new InversionsController(context);
                controller.ControllerContext.HttpContext = inversionContext;
                InversionDetailsViewModel detailsViewModel = new InversionDetailsViewModel();
                detailsViewModel.ids = new int[0];

                //Act 
                var result = await controller.Details(detailsViewModel);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");

            }
        }

        [Fact]        
        public async Task Details_Inversion_Found()
        {
            using (context)
            {
                // Arrenge
                var controller = new InversionsController(context);
                //Simular una conexion de usuario
                controller.ControllerContext.HttpContext = inversionContext;

                int[] ids = new int[2] { 1, 2 };
                InversionDetailsViewModel detailsViewModel = new InversionDetailsViewModel() { ids = ids };                

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

                //Proyectos esperados
                var expectedProyectos = new Proyecto[2] { new Proyecto { ProyectoId = 1, FechaExpiracion = new DateTime (2019, 01, 23), Importe = 30000, Interes = (float) 5.90, MinInversion = 50, Nombre = "E-MEDICA", NumInversores = 0, Plazo = 12, Progreso = 0, RatingId = 1},
                                                          new Proyecto { ProyectoId = 2, FechaExpiracion = new DateTime (2019, 01, 14), Importe = 70000, Interes = (float) 7.25, MinInversion = 0, Nombre = "PROTOS", NumInversores = 0, Plazo = 48, Progreso = 0, RatingId = 1 }};

                //Inversion esperada
                var expectedInversiones = new Inversion[]
                {
                    new Inversion
                    {
                        InversionId = 1,
                        Cuota = 750,
                        EstadosInversiones = "En Curso",
                        Intereses = (float) 5.9,
                        InversorId = "1",
                        Proyecto = expectedProyectos[0],
                        Inversor = expectedinversor,
                        TipoInversionesId = 2,
                        Total = (float) 794.25
                    },
                    new Inversion
                    {
                        InversionId = 2,
                        Cuota = 850,
                        EstadosInversiones = "En Curso",
                        Intereses = (float) 7.25,
                        InversorId = "2",
                        Proyecto = expectedProyectos[1],
                        Inversor = expectedinversor,
                        TipoInversionesId = 1,
                        Total = (float) 911.625
                    }
                };

                //Act
                var result = await controller.Details(detailsViewModel);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                var model = viewResult.Model as IEnumerable<Inversion>;

                Assert.Equal(expectedInversiones, model, Comparer.Get<Inversion>((i1, i2) => i1.Equals(i2)));
            }
        }
    }
}
