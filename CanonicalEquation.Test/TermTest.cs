using System;
using System.Linq;
using CanonicalEquation.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CanonicalEquation.Test
{
    /// <summary>
    /// Логика парсинга слагаемых
    /// </summary>
    [TestClass]
    public class TermTest
    {
        /// <summary>
        /// Проверяем простое слагаемое с одной переменной и числовым коэффициентом
        /// </summary>
        [TestMethod]
        public void SimpleTerm()
        {
            const string term = "+5x^2";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(1, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.Single().Key);
            Assert.AreEqual(2, termData.Variables.Single().Value);
            Assert.AreEqual("+5x^2", termData.ToString());
        }

        /// <summary>
        /// Проверяем обработку нескольких переменных
        /// </summary>
        [TestMethod]
        public void ManyVariableTerm()
        {
            const string term = "-10x^2yz^4";
            var termData = new Term(term);
            Assert.AreEqual(-10, termData.A);
            Assert.AreEqual(3, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.First().Key);
            Assert.AreEqual(2, termData.Variables.First().Value);

            Assert.AreEqual('y', termData.Variables.Skip(1).First().Key);
            Assert.AreEqual(1, termData.Variables.Skip(1).First().Value);

            Assert.AreEqual('z', termData.Variables.Skip(2).First().Key);
            Assert.AreEqual(4, termData.Variables.Skip(2).First().Value);

            Assert.AreEqual(term, termData.ToString());
        }

        /// <summary>
        /// Проверяем сокращение переменных при умножении
        /// </summary>
        [TestMethod]
        public void DuplicateVariableTerm()
        {
            const string term = "-127.58x^2yz^4x";
            var termData = new Term(term);
            Assert.AreEqual(-127.58, termData.A, 0);
            Assert.AreEqual(3, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.First().Key);
            Assert.AreEqual(3, termData.Variables.First().Value);

            Assert.AreEqual('y', termData.Variables.Skip(1).First().Key);
            Assert.AreEqual(1, termData.Variables.Skip(1).First().Value);

            Assert.AreEqual('z', termData.Variables.Skip(2).First().Key);
            Assert.AreEqual(4, termData.Variables.Skip(2).First().Value);

            Assert.AreEqual("-127.58x^3yz^4", termData.ToString());
        }

        /// <summary>
        /// Проверяем обработку слагаемого без коэффициент А
        /// </summary>
        [TestMethod]
        public void TermWithoutA()
        {
            const string term = "+x^2";
            var termData = new Term(term);
            Assert.AreEqual(1, termData.A);
            Assert.AreEqual(1, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.Single().Key);
            Assert.AreEqual(2, termData.Variables.Single().Value);
            Assert.AreEqual("+x^2", termData.ToString());
        }

        /// <summary>
        /// Проверяем обработку слагаемого без X и K
        /// </summary>
        [TestMethod]
        public void TermWithoutVariable()
        {
            const string term = "+5";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(0, termData.Variables.Count);
            Assert.AreEqual("+5", termData.ToString());
        }

        /// <summary>
        /// Проверяем обработку отрицательной степени
        /// </summary>
        [TestMethod]
        public void NegativeKTerm()
        {
            const string term = "+5x^-2";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(1, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.Single().Key);
            Assert.AreEqual(-2, termData.Variables.Single().Value);
            Assert.AreEqual("+5x^-2", termData.ToString());
        }

        /// <summary>
        /// Проверяем сокращение переменных при умножении
        /// </summary>
        [TestMethod]
        public void NegativeKReduceVariable()
        {
            const string term = "+5yx^-2x^2";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(1, termData.Variables.Count);
            Assert.AreEqual('y', termData.Variables.Single().Key);
            Assert.AreEqual(1, termData.Variables.Single().Value);
            Assert.AreEqual("+5y", termData.ToString());
        }

        /// <summary>
        /// Проверяем сокращение всех переменных при умножении
        /// </summary>
        [TestMethod]
        public void NegativeKReduceTerm()
        {
            const string term = "+5x^-2x^2";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(0, termData.Variables.Count);
            Assert.AreEqual("+5", termData.ToString());
        }

        /// <summary>
        /// Проверяем отрицательные степени, состоящие из нескольких цирф
        /// </summary>
        [TestMethod]
        public void NegativeKTermTwoDigit()
        {
            const string term = "+5x^-52";
            var termData = new Term(term);
            Assert.AreEqual(5, termData.A);
            Assert.AreEqual(1, termData.Variables.Count);
            Assert.AreEqual('x', termData.Variables.Single().Key);
            Assert.AreEqual(-52, termData.Variables.Single().Value);
            Assert.AreEqual("+5x^-52", termData.ToString());
        }
    }
}
