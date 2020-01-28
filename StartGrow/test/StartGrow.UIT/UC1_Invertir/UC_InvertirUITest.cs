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
using StartGrow.UIT.UC1_Invertir.UC_InvertirUIMapClasses;

namespace StartGrow.UIT.UC1_Invertir
{
    /// <summary>
    /// Descripción resumida de UC_InvertirUITest
    /// </summary>
    [CodedUITest]
    public class UC_InvertirUITest
    {
        public UC_InvertirUITest()
        {
        }       

        [TestMethod]
        public void InvertirUITest_UCI_1()
        {
            //El inversor realiza la inversión correctamente sin utilizar el filtro.            
            this.UIMap.InversionRealizadaSinFiltro();
            this.UIMap.AssertInversionRealizadaSinFiltro();

        }

        [TestMethod]
        public void InvertirUITest_UCI_2()
        {
            //El inversor realiza la inversión correctamente haciendo un filtrado.         
            this.UIMap.InversionRealizadaConFiltro();
            this.UIMap.AssertInversionRealizadaConFiltro();

        }

        [TestMethod]
        public void InvertirUITest_UCI_3()
        {
            //El inversor realiza un filtrado pero no se encuentran inversiones.       
            this.UIMap.NoEncuentraProyectosConFiltro();
            this.UIMap.AssertNoEncuentraProyectosConFiltro();

        }        

        [TestMethod]
        public void InvertirUITest_UCI_4()
        {
            //El Inversor introduce una cantidad inferior a la mínima y selecciona un tipo de inversión.
            this.UIMap.CuotaInferiorInvMin();
            this.UIMap.AssertCuotaInferiorInvMin();
        }

        [TestMethod]
        public void InvertirUITest_UCI_5()
        {
            //El Inversor introduce una cantidad superior a la del monedero y selecciona un tipo de inversión.
            this.UIMap.CuotaSuperiorMonedero();
            this.UIMap.AssertCuotaSuperiorMonedero();
            
        }

        [TestMethod]
        public void InvertirUITest_UCI_6()
        {
            //El inversor introduce una cantidad válida y no selecciona un tipo de inversión. 
            this.UIMap.SinTipoInversionSeleccionado();
            this.UIMap.AssertSinTipoInversionSeleccionado();

        }
        

        [TestMethod]
        public void InvertirUITest_UCI_7()
        {
            //El Inversor introduce una cantidad inferior a la mínima y no selecciona un tipo de inversión.
            this.UIMap.CuotaInferiorInvMinSinTipo();
            this.UIMap.AssertCuotaInferiorInvMinSinTipo();
        }

        [TestMethod]
        public void InvertirUITest_UCI_8()
        {
            //El inversor introduce una cantidad superior a lo disponible en monedero y no selecciona un tipo de inversión.
            this.UIMap.CuotaSuperiorMonederoSinTipo();
            this.UIMap.AssertCuotaSuperiorMonederoSinTipo();
        }

        [TestMethod]
        public void InvertirUITest_UCI_9()
        {
            //El inversor selecciona un proyecto y no introduce ninguna cantidad ni selecciona un tipo de inversión.
            this.UIMap.SinCuotaSinTipo();
            this.UIMap.AssertSinCuotaSinTipo();
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

        public UC_InvertirUIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new UC_InvertirUIMap();
                }

                return this.map;
            }
        }

        private UC_InvertirUIMap map;
    }
}
