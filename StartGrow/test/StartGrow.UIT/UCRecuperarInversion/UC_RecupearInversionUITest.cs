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
using StartGrow.UIT.UCRecuperarInversion.UC_RecuperarInversionUIMapClasses;

namespace StartGrow.UIT.UCRecuperarInversion
{
    /// <summary>
    /// Descripción resumida de CodedUITest1
    /// </summary>
    [CodedUITest]
    public class UC_RecupearInversionUITest
    {
        public UC_RecupearInversionUITest()
        {
        }

        [TestMethod]
        public void TestUsuarioNoLogeado_UCI1()
        {
            // Para generar código para esta prueba, seleccione "Generar código para prueba automatizada de IU" en el menú contextual y seleccione uno de los elementos de menú.
            this.UIMap.AccesoAplicacion();
            this.UIMap.NoLogin();
            this.UIMap.AssertNoLogin();
      //      this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void TestInversorSinInversiones_UCI2()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.RecuperarSinInversiones();
            this.UIMap.AssertRecuperarSinInversiones();
           // this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void TestInversorFiltraSinResultado_UCI3()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.FiltrarSinResultados();
            this.UIMap.AssertFiltrarSinResultados();
         //   this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void TestRecuperarEnRecaudacion_UCI4()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.RecuperarEnRecaudacion();
            this.UIMap.AssertRecuperarEnRecaudacion();
            //this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void SinSeñalarInversion_UCI5()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.SinMarcar();
            this.UIMap.AssertSinMarcar();
            //  this.UIMap.CerrarAplicacion();

        }

        [TestMethod]
        public void TestSinComentario_UCI6()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.FiltradoDeInversor();
            this.UIMap.SinComentario();
            this.UIMap.AssertSinComentario();
          //  this.UIMap.CerrarAplicacion();

        }
        [TestMethod]
        public void TestCantidad_0_UCI7()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.FiltradoDeInversor();
            this.UIMap.Cantidad_0();
            this.UIMap.AssertCantidad_0();
          //  this.UIMap.CerrarAplicacion();

        }
        [TestMethod]
        public void TestCantidad_Mayor_UCI8()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.FiltradoDeInversor();
            this.UIMap.Cantidad_Mayor();
            this.UIMap.AssertCantidad_Mayor();
          //  this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void TestInversionRecuperada_UCI9()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.FiltradoDeInversor();
            this.UIMap.RecuperarInversion();
            this.UIMap.AssertRecuperarInversion();
       //     this.UIMap.CerrarAplicacion();
        }
        [TestMethod]
        public void TestInversionRecuperadaSinFiltrar_UCI10()
        {
            this.UIMap.AccesoAplicacion();
            this.UIMap.RecuperarSinFiltrar();
            this.UIMap.AssertRecuperarSinFiltrar();
       //     this.UIMap.CerrarAplicacion();
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

        public UC_RecuperarInversionUIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new UC_RecuperarInversionUIMap();
                }

                return this.map;
            }
        }

        private UC_RecuperarInversionUIMap map;
    }
}
