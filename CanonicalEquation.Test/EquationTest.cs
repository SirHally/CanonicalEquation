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

        }
    }
}
