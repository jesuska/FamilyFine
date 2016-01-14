using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamilyFinance;
using FamilyFinance.Controllers;

namespace FamilyFinance.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        /*
         1. Tranzakciókat (költségeket és bevételeket) fel lehet venni, módosítani, törölni:
         *     - összeg 
         *     - tranzakció napja
         *     - kategória (pl. Auchan, rezsi, tankolás, lakáshitel, Anett suli, Ajándékok, Kölcsön, stb...)
         *     - megjegyzés (nem kötelező, pl: Főtáv, Árpinak kölcsön, Petinek ajcsi, stb... )
         *     - limitbe számolódik-e
         *     - Anett költésébe számolódik-e
         2. Meg lehet adni havi limitet (időszakosan)
         3. Meg lehet adni Anett havi limitjét (időszakosan)
         4. Meg lehet adni bármely naphoz egy pillanatnyi pénzmennyiséget (és Anett egyenleget), ami van akkor éppen
         5. Kimutatások bármely időszakra:
         *     - Aktuális pénzmennyiség alakulása (mérleg):
         *          - Költés / nap (átlagban)
         *          - Kategóriánként mennyi az összköltés
         *          - Tételesen milyen költések tartoznak a kategória alá (megjegyzés/dátum páros)
         *     - Havi limit eléréséhez:
         *          - mennyi költés / nap engedélyezett
         *          - mennyi a hátralevő összeg
         *     - Anett havi limit eléréséhez:
         *          - mennyi költés / nap engedélyezett
         *          - mennyi a hátralevő összeg
        */

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }
    }
}
