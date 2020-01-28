using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using StartGrow.UIT.PublicarProyectos.PublicarProyectoUIMapClasses;

namespace StartGrow.UIT.PublicarProyectos
{
    /// <summary>
    /// Descripción resumida de PublicarProyectosUITest
    /// </summary>
    [CodedUITest]
    public class PublicarProyectosUITest
    {
        public PublicarProyectosUITest()
        {
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_1()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

       

        //[TestMethod]
        //public void PublicarProyectoUITest_UCI_2()
        //{
        //    this.UIMap.AccesoAplicacion();
        //    this.UIMap.AssertBotonLogin();
        //    this.UIMap.LoginEnApp();
        //    this.UIMap.AsercionLoginCorrecto();
        //    this.UIMap.AsercionNoHayProyectos();
        //    // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        //}

        [TestMethod]
        public void PublicarProyectoUITest_UCI_3()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.SeleccionarProyectoMYRED();
            this.UIMap.AsercionBotonCreate();
            this.UIMap.CreateCorrecto();
            this.UIMap.AssertDetailsMYRED();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_4()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.AsercionNombreProyecto();
            this.UIMap.FiltrarProyectos3();
            this.UIMap.AsercionProyectosFiltradosSTOYKE();
            this.UIMap.SeleccionarProyecto();
            this.UIMap.AsercionBotonCreate();
            this.UIMap.CreateCorrecto();
            this.UIMap.AsercionDetails();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_5()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.AsercionNombreProyecto();
            this.UIMap.FiltrarProyectos2();
            this.UIMap.AsercionErrorNoProyecto();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_6()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.AsercionNombreProyecto();
            this.UIMap.FiltrarProyectosJUSTNOW();
            this.UIMap.AsercionProyectosFiltradosJUSTNOW();
            this.UIMap.SeleccionarProyectoJUSTNOW();
            this.UIMap.AsercionBotonCreate();
            this.UIMap.EstadoRechazadoRatingB();
            this.UIMap.AsercionErrorCreateJUSTNOW();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_7()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.AsercionNombreProyecto();
            this.UIMap.FiltrarProyectosJUSTNOW();
            this.UIMap.AsercionProyectosFiltradosJUSTNOW();
            this.UIMap.SeleccionarProyectoJUSTNOW();
            this.UIMap.AsercionBotonCreate();
            this.UIMap.EstadoAceptadoRatingA();
            this.UIMap.AsercionErrorInteresPlazoJUSTNOW();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        [TestMethod]
        public void PublicarProyectoUITest_UCI_8()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.AssertBotonLogin();
            this.UIMap.LoginEnApp();
            this.UIMap.AsercionLoginCorrecto();
            this.UIMap.AsercionNombreProyecto();
            this.UIMap.FiltrarProyectosPOCHOLO();
            this.UIMap.AsercionProyectosFiltradosPOCHOLO();
            this.UIMap.SeleccionarProyectoPOCHOLO();
            this.UIMap.AsercionBotonCreate();
            this.UIMap.CreateCorrectoPOCHOLO();
            this.UIMap.AsercionDetailsPOCHOLO();
            this.UIMap.CerrarSesion();
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        }

        #region Atributos de prueba adicionales

        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:

        ////Use TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        //}

        ////Use TestCleanup para ejecutar el código después de ejecutar cada prueba
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
        //}

        #endregion

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la serie de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public PublicarProyectoUIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new PublicarProyectoUIMap();
                }

                return this.map;
            }
        }

        private PublicarProyectoUIMap map;
    }
}
