using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanonicalEquation.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CanonicalEquation.Test
{
    [TestClass]
    public class EquationTest
    {
        [TestMethod]
        public void SimpleEquation()
        {
           var logic = new EquationLogic();
           string result = logic.Process("x^2 + 3.5xy + y = y^2 - xy + y");
           Assert.AreEqual("x^2+4.5xy-y^2=0", result);
        }

        [TestMethod]
        public void SimpleEquationWithBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("(x^2 + 3.5xy + y) =-( y^2 - xy + y)");
            Assert.AreEqual("x^2+2.5xy+2y+y^2=0", result);
        }
    }
}
