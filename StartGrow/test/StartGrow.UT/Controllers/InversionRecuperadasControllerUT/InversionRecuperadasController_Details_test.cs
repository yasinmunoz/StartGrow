using System;
using System.Collections.Generic;
using System.Text;
using StartGrow.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StartGrow.Models;
using StartGrow.Data;
using StartGrow.Models.InversionRecuperadaViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace StartGrow.UT.Controllers.InversionRecuperadasControllerUT
{

    public class InversionRecuperadasController_Details_test
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            //Crear un nuevo proveedor de servicios, y por tanto, una nueva instancia
            //de base de datos InMemory
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

            //Crear una nueva instancia de opciones indicando el contexto que use
            //una base de datos InMemory y el nuevo proovedor de servicios.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("StartGrow")
                    .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext inversionRecuperadaContext;
        private DefaultHttpContext inversionRecuepradaContext;

        public InversionRecuperadasController_Details_test()
        {
            _contextOptions = CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);

            //Insertar datos semilla en la base de datos usando una instancia de contexto   
            var rating = new Rating { Nombre = "A" };
            context.Rating.Add(rating);
            var area = new Areas { Nombre = "Sanidad" };
            context.Areas.Add(area);
            var tipo = new TiposInversiones { Nombre = "Crownfunding" };
            context.TiposInversiones.Add(tipo);



            Proyecto proyecto1 = new Proyecto
            {
                ProyectoId = 1,
                FechaExpiracion = new DateTime(2020, 1, 1),
                Importe = 12,
                Interes = 50,
                MinInversion = 5,
                Nombre = "Pruebas en sanidad",
                NumInversores = 0,
                Plazo = 12,
                Progreso = 34,
                Rating = rating
            };
            context.Proyecto.Add(proyecto1);

            context.ProyectoAreas.Add(new ProyectoAreas { Proyecto = proyecto1, Areas = area });


            Monedero monedero1 = new Monedero
            {
                MonederoId = 1,
                Dinero = 500
            };
            context.Monedero.Add(monedero1);

            Inversor inversor1 = new Inversor
            {
                Id = "1",
                Nombre = "david@uclm.es",
                Email = "david@uclm.es",
                Apellido1 = "Girón",
                Apellido2 = "López",
                Domicilio = "C/Cuenca",
                Municipio = "Albacete",
                NIF = "48259596",
                Nacionalidad = "Española",
                PaisDeResidencia = "España",
                Provincia = "Albacete",
                PasswordHash = "hola",
                UserName = "david@uclm.es",
                Monedero = monedero1
            };
            context.Users.Add(inversor1);

            
            Inversion inversion1 = new Inversion
            {
                InversionId = 1,
                Cuota = 150,
                EstadosInversiones = "En_Curso",
                Intereses = 50,
                Inversor = inversor1,
                Proyecto = proyecto1,
                TipoInversionesId = 1,
                Total = 200
            };
            context.Inversion.Add(inversion1);

            Inversion inversion2 = new Inversion
            {
                InversionId = 2,
                Cuota = 150,
                EstadosInversiones = "Finalizado",
                Intereses = 50,
                Inversor = inversor1,
                Proyecto = proyecto1,
                TipoInversionesId = 1,
                Total = 200
            };
            context.Inversion.Add(inversion2);


            InversionRecuperada invRec1 = new InversionRecuperada
            {
                InversionRecuperadaId = 1,
                CantidadRecuperada = 5,
                Inversion = inversion1,
                Comentario = "OK 1",
                FechaRecuperacion = DateTime.Now,
            };

            InversionRecuperada invRec2 = new InversionRecuperada
            {
                InversionRecuperadaId = 2,
                CantidadRecuperada = 10,
                Inversion = inversion1,
                Comentario = "OK 2",
                FechaRecuperacion = DateTime.Now,
            };

            context.InversionRecuperada.Add(invRec1);
            context.InversionRecuperada.Add(invRec2);

            context.SaveChanges();

            //Para simular la conexión:
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("david@uclm.es");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            inversionRecuperadaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            inversionRecuperadaContext.User = identity;

        }











        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //PRUEBAS DEL MÉTODO GET -----> UNO POR CADA CAMINO INDEPENDIENTE
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        //En este caso, no le pasamos ninguna Lista de IdsToAdd, por lo que creamos un ViewModel "inversiones" sin declarar ningun IdsToAdd.
        //En el Assert debemos comprobar que lo que devuelve el método GET es un RedirectToAction hacia "SelectInversionForRecuperarInversion".
        [Fact]
        public async Task Details_GET_SinIdsToAdd()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;
                InversionRecuperadaDetailsViewModel detailsVM = new InversionRecuperadaDetailsViewModel();

                //ACT (Actuar) --> Realización de la prueba
                var result = await controller.Details(detailsVM);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");

            }
        }



        [Fact]
        public async Task Details_GET_SinInversionesRecuperadas()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;
                InversionRecuperadaDetailsViewModel detailsVM = new InversionRecuperadaDetailsViewModel();
                detailsVM.IdsToAdd = new int[0];

                //ACT (Actuar) --> Realización de la prueba
                var result = await controller.Details(detailsVM);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "Create");

            }
        }










        [Fact]
        public async Task Details_GET_ConInversionesRecuperadas()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                int[] ids = new int[2] { 1, 2 };
                InversionRecuperadaDetailsViewModel detailsVM = new InversionRecuperadaDetailsViewModel() { IdsToAdd = ids };


                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 50,
                    MinInversion = 5,
                    Nombre = "Pruebas en sanidad",
                    NumInversores = 0,
                    Plazo = 12,
                    Progreso = 34,
                    Rating = rating
                };

                Inversor inversor1 = new Inversor
                {
                    Id = "1",
                    Nombre = "david@uclm.es",
                    Email = "david@uclm.es",
                    Apellido1 = "Girón",
                    Apellido2 = "López",
                    Domicilio = "C/Cuenca",
                    Municipio = "Albacete",
                    NIF = "48259596",
                    Nacionalidad = "Española",
                    PaisDeResidencia = "España",
                    Provincia = "Albacete",
                    PasswordHash = "hola",
                    UserName = "david@uclm.es"
                };



                Inversion inversion1 = new Inversion
                {
                    InversionId = 1,
                    Cuota = 150,
                    EstadosInversiones = "En_Curso",
                    Intereses = 50,
                    Inversor = inversor1,
                    Proyecto = proyecto1,
                    TipoInversionesId = 1,
                    Total = 200
                };

                var invRecEsperadas = new InversionRecuperada[]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        InversionId = 1,
                        CantidadRecuperada = 5,
                        Inversion = inversion1,
                        Comentario = "OK 1",
                        FechaRecuperacion = DateTime.Now,
                        
                    },

                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 2,
                        InversionId = 1,
                        CantidadRecuperada = 10,
                        Inversion = inversion1,
                        Comentario = "OK 2",
                        FechaRecuperacion = DateTime.Now,

                    }
                };



                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Details(detailsVM);

                
            
                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result); //Comprueba si el controlador devuelve una vista
                var model = viewResult.Model as IEnumerable<InversionRecuperada>;

                Assert.Equal(invRecEsperadas, model, Comparer.Get<InversionRecuperada>((i1, i2) => i1.Equals(i2)));


            }
        }


    }
}















