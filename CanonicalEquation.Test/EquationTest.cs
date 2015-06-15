using CanonicalEquation.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CanonicalEquation.Test
{
    /// <summary>
    /// Тестируем приведение уравнения к каноническому виду
    /// </summary>
    [TestClass]
    public class EquationTest
    {
        /// <summary>
        /// Простое уравнение без скобок
        /// </summary>
        [TestMethod]
        public void SimpleEquation()
        {
           var logic = new EquationLogic();
           string result = logic.Process("x^2 + 3.5xy + y = y^2 - xy + y");
           Assert.AreEqual("x^2+4.5xy-y^2=0", result);
        }

        /// <summary>
        /// Простое уравнение с одноуровневыми скобками
        /// </summary>
        [TestMethod]
        public void SimpleEquationWithBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("(x^2 + 3.5xy + y) =-( y^2 - xy + y)");
            Assert.AreEqual("x^2+2.5xy+2y+y^2=0", result);
        }

        /// <summary>
        /// Простое уравнение с вложенными скобками, которые дублируются
        /// </summary>
        [TestMethod]
        public void SimpleEquationDoubleBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(x^2 + 3.5xy + y) =-(( y^2 - xy + y))");
            Assert.AreEqual("-x^2-4.5xy+y^2=0", result);
        }

        /// <summary>
        /// Уравнение со вложенными на два уровня скобками
        /// </summary>
        [TestMethod]
        public void ComplexEquationOneLevelOneBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(x^2 - (3.5xy + y)) =-( y^2 - xy + y)");
            Assert.AreEqual("-x^2+2.5xy+2y+y^2=0", result);
        }

        /// <summary>
        /// Уравнение со вложенными скобками, находящимися на одном уровне
        /// </summary>
        [TestMethod]
        public void ComplexEquationOneLevelTwoBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(-(x^2 + 2xy) - (3.5xy + y)) =-( y^2 - xy + y)");
            Assert.AreEqual("x^2+4.5xy+2y+y^2=0", result);
        }

        /// <summary>
        /// Уравнение со вложенными на несколько уровней скобками
        /// </summary>
        [TestMethod]
        public void ComplexEquationTwoLevelOneBrace()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(x^2 - (2xy - (3.5xy + y))) =-( y^2 - xy + y)");
            Assert.AreEqual("-x^2-2.5xy+y^2=0", result);
        }

        /// <summary>
        /// Уравнение с отрицательной степенью
        /// </summary>
        [TestMethod]
        public void EquationWithNegativeExponent()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(x^2 - (2x^-52y - (3.5x^-52y + y))) =-( y^2 - x^-52y + y)");
            Assert.AreEqual("-x^2-2.5x^-52y+y^2=0", result);
        }

        /// <summary>
        /// Уравнение с сокращением переменных внутри слагаемого
        /// </summary>
        [TestMethod]
        public void EquationWithReducedP()
        {
            var logic = new EquationLogic();
            string result = logic.Process("-(x^2 - 2x^-52x^52 - 3.5x^-52x^52 + y) =-( y^2 - x^-52x^52 + y)");
            Assert.AreEqual("-x^2+4.5+y^2=0", result);
        }
    }
}
