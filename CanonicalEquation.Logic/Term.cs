using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CanonicalEquation.Logic
{
    /// <summary>
    ///     Слагаемое вида ax^k
    /// </summary>
    public class Term
    {
        /// <summary>
        /// Текущее состоянии при анализе слагаемого
        /// </summary>
        private enum TermPart
        {
            /// <summary>
            /// Анализируется числовой коэффициент в начала
            /// </summary>
            A,
            /// <summary>
            /// Анализируется блок с переменными
            /// </summary>
            Variable
        }

        /// <summary>
        /// Создаем объект-слагаемое на основе его текстового представления
        /// </summary>
        /// <param name="predicate">Выражение в виде ax^k</param>
        public Term(string predicate)
        {
            Variables = new Dictionary<char, int>();

            var a = new StringBuilder();
            var exponent = new StringBuilder();
            var currentState = TermPart.A;
            var prevVariable = char.MaxValue;
            foreach (var current in predicate)
            {
                if ((char.IsNumber(current) || current == '.' || current == '-' || current == '+') && currentState == TermPart.A)
                {
                    a.Append(current);
                }
                else
                {
                    currentState = TermPart.Variable;

                    if (char.IsNumber(current) || current=='-')
                    {
                        exponent.Append(current);
                    }
                    else if (char.IsLetter(current))
                    {
                        if (!Variables.ContainsKey(current))
                        {
                            Variables[current] = 1;
                        }
                        else
                        {
                            Variables[current]++;
                        }
                        if (exponent.Length > 0)
                        {
                            Variables[prevVariable] += int.Parse(exponent.ToString()) - 1;
                            exponent.Clear();
                        }
                        prevVariable = current;
                    }
                }
            }

            A = a.Length == 1  ? (a[0]=='-'? -1 : 1) : double.Parse(a.ToString(), CultureInfo.InvariantCulture);

            if (exponent.Length > 0)
            {
                Variables[prevVariable] += int.Parse(exponent.ToString()) - 1;
                exponent.Clear();
            }

            if (Variables.Any())
            {
                Variables = Variables.Where(t => t.Value != 0).ToDictionary(t => t.Key, t => t.Value);
    
            }

        }

        /// <summary>
        /// Коэффициент А
        /// </summary>
        public double A { get; set; }

        /// <summary>
        ///     Переменные и их степени [x1]=k1
        /// </summary>
        public Dictionary<char, int> Variables { get; set; }

        /// <summary>
        /// Определяет, являются ли два слагаемых подобными
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public bool HasSameVariables(Term term)
        {
            if (Variables.Count != term.Variables.Count)
            {
                return false;
            }
            foreach (KeyValuePair<char, int> pair in Variables)
            {
                if (!term.Variables.ContainsKey(pair.Key) || term.Variables[pair.Key]!=pair.Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Строковое представление слагаемого
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            if (A > 0)
            {
                result.Append("+");
            }
            if (Math.Abs(A - (-1)) < EquationLogic.Tolerance)
            {
                result.Append('-');
            }
            else if (Math.Abs(A - 1) > EquationLogic.Tolerance)
            {
                result.Append(A.ToString("G15", CultureInfo.InvariantCulture));
            }
            
            foreach (var pair in Variables)
            {
               
                result.Append(pair.Key);
                if (pair.Value != 1)
                {
                    result.Append('^');
                    result.Append(pair.Value);
                }
            }
            return result.ToString();
        }
    }
}