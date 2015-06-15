using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanonicalEquation.Logic
{
    /// <summary>
    /// Логика обработки уравнений
    /// </summary>
    public class EquationLogic
    {
        /// <summary>
        /// Дельта для сравнения дробных величин
        /// </summary>
        public const double Tolerance = 0.00001;

        /// <summary>
        /// Приводит уравнение к каноническому виду
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        public string Process(string equation)
        {
            Stack<TermCollection> collection = SimplifyEquation(equation);

            IList<Term> terms = GetTermCollection(collection);

            ReduceCollection(terms);

            return MakeSimplifiedEquation(terms);
        }

        /// <summary>
        /// Обрабатываем строку с исходным уравнением
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        private Stack<TermCollection> SimplifyEquation(string equation)
        {
            var currentTerm = new StringBuilder();
            var collection = new Stack<TermCollection>();
            collection.Push(new TermCollection(false));
            var isMinus = false;
            bool exponentaPart = false;
            foreach (var current in equation)
            {
                if (!(Char.IsNumber(current) || (current == '-' && currentTerm.Length > 0 && currentTerm[currentTerm.Length - 1] == '^')))
                {
                    exponentaPart = false;
                }
                switch (current)
                {
                    case ' ':
                        continue;
                    case '(':
                        collection.Push(new TermCollection(isMinus ^ collection.First().IsMinus));
                        currentTerm.Clear();
                        isMinus = false;
                        break;
                    case ')':
                        collection.First().TryAddTerm(currentTerm);
                        var currentCollection = collection.Pop();
                        collection.First().AddCollection(currentCollection);
                        break;
                    case '+':
                        isMinus = false;
                        collection.First().TryAddTerm(currentTerm);
                        currentTerm.Append(current);
                        break;
                    case '-':
                        if (exponentaPart)//Этот минус - показатель отрицательной степени K
                        {
                            currentTerm.Append(current);
                        }
                        else
                        {//Это минус в выражении, между Term
                            isMinus = true;
                            collection.First().TryAddTerm(currentTerm);
                            currentTerm.Append(current);
                        }
                        break;
                    case '=':
                        isMinus = false;
                        collection.First().TryAddTerm(currentTerm);
                        collection.Push(new TermCollection(true));
                        break;
                    case '^':
                        exponentaPart = true;
                        currentTerm.Append(current);
                        break;
                    default:
                        if (currentTerm.Length == 0)
                        {
                            currentTerm.Append("+");
                        }
                        currentTerm.Append(current);
                        break;
                }
            }
            collection.First().TryAddTerm(currentTerm);

            return collection;
        }

        /// <summary>
        /// Склеиваем оставшиеся последовательности и получаем необходимые слагаемые.
        /// Они уже имеют правильные знаки
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private IList<Term> GetTermCollection(Stack<TermCollection> collection)
        {
            List<Term> terms = new List<Term>();
            for (int i = collection.Count() - 1; i >= 0; i--)
            {
                foreach (var term in collection.Skip(i).First().Terms)
                {
                    terms.Add(term);
                }
            }
            return terms;
        }

        /// <summary>
        /// Приводим подобные члены
        /// </summary>
        private void ReduceCollection(IList<Term> terms)
        {
            for (int i = 0; i < terms.Count; i++)
            {
                for (int j = i + 1; j < terms.Count; j++)
                {
                    if (terms[i].HasSameVariables(terms[j]))
                    {
                        terms[i].A += terms[j].A;
                        terms.Remove(terms[j]);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// Формируем строку по созданному уравнению
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        private string MakeSimplifiedEquation(IList<Term> terms)
        {
            var result = new StringBuilder();
            foreach (Term term in terms)
            {
                if (Math.Abs(term.A) > Tolerance)
                {
                    result.Append(term);
                }
            }
            result.Append("=0");

            return result[0] == '+' ? result.ToString(1, result.Length - 1) : result.ToString();
        }
    }
}