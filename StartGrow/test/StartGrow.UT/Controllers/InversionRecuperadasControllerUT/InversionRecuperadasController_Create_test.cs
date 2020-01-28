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

    public class InversionRecuperadasController_Create_test
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

        public InversionRecuperadasController_Create_test()
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
        public async Task Create_GET_SinIdsToAdd()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel();

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversiones);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "SelectInversionForRecuperarInversion");
            }
        }







        //En este caso, le pasamos una Lista de IdsToAdd pero esa lista estará vacía, por lo que crea un ViewModel "inversiones"
        //declarando un IdsToAdd pero sin ningún elemento.
        //En el Assert debemos comprobar que lo que devuelve el método GET es un RedirectToAction hacia "SelectInversionForRecuperarInversion".
        [Fact]
        public async Task Create_GET_SinInversiones()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel();
                inversiones.IdsToAdd = new string[0];

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversiones);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(viewResult.ActionName, "SelectInversionForRecuperarInversion");
                
            }
        }







        //En este caso, le pasamos una Lista de IdsToAdd con un elemento (el id "1"), por lo que creamos un ViewModel "inversiones"
        //declarando un IdsToAdd donde el único elemento será el id "1".
        //Ademaás nos tenemos que crear una inversión esperada.
        //En el Assert debemos comprobar que lo que devuelve el método GET es la inversión esperada (con id "1").
        [Fact]
        public async Task Create_GET_ConInversiones()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

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

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 150,
                        EstadosInversiones = "En_Curso",
                        Intereses = 50,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 200
                    }
                };

                InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions
                };


                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversiones);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                ViewResult viewResult = Assert.IsType<ViewResult>(result); //Comprueba si el controlador devuelve una vista
                InversionRecuperadaCreateViewModel model = viewResult.Model as InversionRecuperadaCreateViewModel;

                Assert.Equal(inversionEsperada.Inversiones, model.Inversiones, Comparer.Get<Inversion>((i1, i2) => i1.Equals(i2)));


            }
        }












        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //PRUEBAS DEL MÉTODO POST -----> UNO POR CADA CAMINO INDEPENDIENTE
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        [Fact]
        public async Task Create_POST_CantidadRecuperada_0()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 2,
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

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 6,
                        EstadosInversiones = "En_Curso",
                        Intereses = 12,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 50
                    }
                };

                IList<InversionRecuperada> listaInvRec = new InversionRecuperada[1]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        Inversion = inversions[0],
                        CantidadRecuperada = 0,
                        Comentario = "OK",
                        FechaRecuperacion = DateTime.Now
                    }
                };

                InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions,
                    InversionesRecuperadas = listaInvRec
                };

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversionEsperada);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result); //Comprueba si el controlador devuelve una vista

                InversionRecuperadaCreateViewModel model = viewResult.Model as InversionRecuperadaCreateViewModel;
                var error = viewResult.ViewData.ModelState["CantidadNoIndicada"].Errors.FirstOrDefault();

                Assert.Equal("Debe indicar una cantidad mayor que 0 en la inversión 1.", error.ErrorMessage);

                Assert.Equal(inversionEsperada.Inversiones, model.Inversiones, Comparer.Get<Inversion>((i1, i2) => i1.Equals(i2)));

            }
        }



        [Fact]
        public async Task Create_POST_SinComentario()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 2,
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

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 6,
                        EstadosInversiones = "En_Curso",
                        Intereses = 12,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 50
                    }
                };

                IList<InversionRecuperada> listaInvRec = new InversionRecuperada[1]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        Inversion = inversions[0],
                        CantidadRecuperada = 50,
                        Comentario = null,
                        FechaRecuperacion = DateTime.Now
                    }
                };

                InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions,
                    InversionesRecuperadas = listaInvRec
                };

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversionEsperada);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result); //Comprueba si el controlador devuelve una vista

                InversionRecuperadaCreateViewModel model = viewResult.Model as InversionRecuperadaCreateViewModel;
                var error = viewResult.ViewData.ModelState["ComentarioNoIndicado"].Errors.FirstOrDefault();

                Assert.Equal("Debe indicar un comentario para la inversión 1.", error.ErrorMessage);

                Assert.Equal(inversionEsperada.Inversiones, model.Inversiones, Comparer.Get<Inversion>((i1, i2) => i1.Equals(i2)));

            }
        }





        [Fact]
        public async Task Create_POST_CantidadNoPermitida()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 2,
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

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 6,
                        EstadosInversiones = "En_Curso",
                        Intereses = 12,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 50
                    }
                };

                IList<InversionRecuperada> listaInvRec = new InversionRecuperada[1]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        Inversion = inversions[0],
                        CantidadRecuperada = 50000,
                        Comentario = "OK",
                        FechaRecuperacion = DateTime.Now
                    }
                };

                InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions,
                    InversionesRecuperadas = listaInvRec
                };

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversionEsperada);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result); //Comprueba si el controlador devuelve una vista

                InversionRecuperadaCreateViewModel model = viewResult.Model as InversionRecuperadaCreateViewModel;
                var error = viewResult.ViewData.ModelState["CantidadNoPermitida"].Errors.FirstOrDefault();

                Assert.Equal("No se puede recuperar dicha cantidad para la inversión 1, eliga una cantidad menor.", error.ErrorMessage);

                Assert.Equal(inversionEsperada.Inversiones, model.Inversiones, Comparer.Get<Inversion>((i1, i2) => i1.Equals(i2)));

            }
        }








        [Fact]
        public async Task Create_POST_DatosCorrectos_Interes()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Monedero monedero1 = new Monedero
                {
                    MonederoId = 1,
                    Dinero = 500
                };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 2,
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
                    UserName = "david@uclm.es",
                    Monedero = monedero1
                };

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 150,
                        EstadosInversiones = "En_Curso",
                        Intereses = 50,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 200
                    }
                };



                IList<InversionRecuperada> listaInvRec = new InversionRecuperada[1]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        Inversion = inversions[0],
                        CantidadRecuperada = 5,
                        Comentario = "OK",
                        FechaRecuperacion = DateTime.Now
                    }
                };



                InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions,
                    InversionesRecuperadas = listaInvRec
                };

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversionEsperada);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var current = context.InversionRecuperada.FirstOrDefault();

                Assert.Equal(current, inversionEsperada.InversionesRecuperadas[0], Comparer.Get<InversionRecuperada>((i1, i2) => i1.Equals(i2)));

                Assert.Equal(viewResult.ActionName, "Details");


            }
        }















        [Fact]
        public async Task Create_POST_DatosCorrectos_Cuota()
        {

            using (context) //Base SQL ya generada con datos incluidos
            {
                //ARRANGE (Organizar) --> Creación de condiciones para la prueba.
                var controller = new InversionRecuperadasController(context);
                controller.ControllerContext.HttpContext = inversionRecuperadaContext;

                String[] ids = new string[1] { "1" };
                SelectedInversionForRecuperarInversionViewModel inversiones = new SelectedInversionForRecuperarInversionViewModel() { IdsToAdd = ids };

                var rating = new Rating { RatingId = 1, Nombre = "A" };

                Monedero monedero1 = new Monedero
                {
                    MonederoId = 1,
                    Dinero = 500
                };

                Proyecto proyecto1 = new Proyecto
                {
                    ProyectoId = 1,
                    FechaExpiracion = new DateTime(2020, 1, 1),
                    Importe = 12,
                    Interes = 2,
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
                    UserName = "david@uclm.es",
                    Monedero = monedero1
                };

                IList<Inversion> inversions = new Inversion[1]
                {
                    new Inversion {
                        InversionId = 1,
                        Cuota = 150,
                        EstadosInversiones = "En_Curso",
                        Intereses = 50,
                        Inversor = inversor1,
                        Proyecto = proyecto1,
                        TipoInversionesId = 1,
                        Total = 200
                    }
                };



                IList<InversionRecuperada> listaInvRec = new InversionRecuperada[1]
                {
                    new InversionRecuperada
                    {
                        InversionRecuperadaId = 1,
                        Inversion = inversions[0],
                        CantidadRecuperada = 100,
                        Comentario = "OK",
                        FechaRecuperacion = DateTime.Now
                    }
                };



            InversionRecuperadaCreateViewModel inversionEsperada = new InversionRecuperadaCreateViewModel
                {
                    Inversiones = inversions,
                    InversionesRecuperadas = listaInvRec
                };

                //ACT (Actuar) --> Realización de la prueba
                var result = controller.Create(inversionEsperada);

                //ASSERT --> Verificación de que el resultado fue el que se esperaba
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                var current = context.InversionRecuperada.FirstOrDefault();

                Assert.Equal(current, inversionEsperada.InversionesRecuperadas[0], Comparer.Get<InversionRecuperada>((i1, i2) => i1.Equals(i2)));

                Assert.Equal(viewResult.ActionName, "Details");


            }
        }




    }
}















