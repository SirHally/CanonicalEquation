using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CanonicalEquation.Logic
{
    public class EquationLogic
    {
        public string Process(string equation)
        {
            var currentTerm = new StringBuilder();
            var collection = new Stack<TermCollection>();
            collection.Push(new TermCollection(false));
            var isMinus = false;
            foreach (var current in equation)
            {
                switch (current)
                {
                    case ' ':
                        continue;
                    case '(':
                        collection.Push(new TermCollection(isMinus ^ collection.Last().IsMinus));
                        break;
                    case ')':
                        collection.First().AddTerm(currentTerm);
                        var currentCollection = collection.Pop();
                        collection.Last().AddCollection(currentCollection);
                        break;
                    case '+':
                        isMinus = false;
                        collection.First().AddTerm(currentTerm);
                        currentTerm.Append(current);
                        break;
                    case '-':
                        isMinus = true;
                        collection.First().AddTerm(currentTerm);
                        currentTerm.Append(current);
                        break;
                    case '=':
                        collection.First().AddTerm(currentTerm);
                        collection.Push(new TermCollection(true));
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
            collection.First().AddTerm(currentTerm);

            List<Term> terms =  new List<Term>();
            foreach (var termCollection in collection)
            {
                foreach (var term in termCollection.Terms)
                {
                    terms.Add(term);
                }
            }

            for (int i = 0; i < terms.Count; i++)
            {
                for (int j = i+1; j < terms.Count; j++)
                {
                    if (terms[i].HasSameVariables(terms[j]))
                    {
                        terms[i].A += terms[j].A;
                        terms.Remove(terms[j]);
                        j--;
                    }
                }
            }


            var result = new StringBuilder();
            foreach (Term term in terms)
            {
                result.Append(term);
            }
            result.Append("=0");

            return result[0] == '+' ? result.ToString(1, result.Length - 1) : result.ToString();
        }
    }


    public class TermCollection
    {
        public TermCollection(bool isMinus)
        {
            Terms = new List<Term>();
            IsMinus = isMinus;
        }

        public List<Term> Terms { get; set; }
        public bool IsMinus { get; set; }

        public int Multiplicator
        {
            get { return IsMinus ? -1 : 1; }
        }

        public void AddTerm(StringBuilder currentTerm)
        {
            if (currentTerm.Length > 0)
            {
                var term = new Term(currentTerm.ToString());
                term.A *= Multiplicator;
                Terms.Add(term);
                currentTerm.Clear();
            }
        }

        public void AddCollection(TermCollection collection)
        {
            Terms.AddRange(collection.Terms);
        }
    }

    /// <summary>
    ///     Слагаемое вида ax^k
    /// </summary>
    public class Term
    {
        private enum TermPart
        {
            A,
            Variable
        }

        /// <summary>
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

                    if (char.IsNumber(current))
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
        }

        public double A { get; set; }

        /// <summary>
        ///     Переменные и их степени [x1]=k1
        /// </summary>
        public Dictionary<char, int> Variables { get; set; }

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

        public override string ToString()
        {
            var result = new StringBuilder();
            if (A > 0)
            {
                result.Append("+");
            }
            if (A == -1)
            {
                result.Append('-');
            }
            else if (A != 1)
            {
                result.Append(A.ToString("G15", CultureInfo.InvariantCulture));
            }
            
            foreach (var pair in Variables)
            {
               
                result.Append(pair.Key);
                if (pair.Value > 1)
                {
                    result.Append('^');
                    result.Append(pair.Value);
                }
            }
            return result.ToString();
        }
    }
}